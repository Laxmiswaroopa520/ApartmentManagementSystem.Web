// wwwroot/js/apartment-visualizer-3d.js

let scene, camera, renderer, controls;
let buildingGroup;
let animationId;

function render3DPreview() {
    const container = document.getElementById('preview3D');
    container.innerHTML = '';

    initThreeJS(container);
    createBuilding3D();
    animate();
}

function initThreeJS(container) {
    // Scene setup
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0x0a0e27);
    scene.fog = new THREE.Fog(0x0a0e27, 50, 200);

    // Camera
    const width = container.clientWidth;
    const height = 600;
    camera = new THREE.PerspectiveCamera(45, width / height, 0.1, 1000);
    camera.position.set(40, 30, 40);
    camera.lookAt(0, 0, 0);

    // Renderer
    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(width, height);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    container.appendChild(renderer.domElement);

    // Lights
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
    scene.add(ambientLight);

    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight.position.set(50, 100, 50);
    directionalLight.castShadow = true;
    directionalLight.shadow.camera.left = -50;
    directionalLight.shadow.camera.right = 50;
    directionalLight.shadow.camera.top = 50;
    directionalLight.shadow.camera.bottom = -50;
    directionalLight.shadow.mapSize.width = 2048;
    directionalLight.shadow.mapSize.height = 2048;
    scene.add(directionalLight);

    // Accent lights
    const pointLight1 = new THREE.PointLight(0x00d4ff, 1, 100);
    pointLight1.position.set(-20, 20, 20);
    scene.add(pointLight1);

    const pointLight2 = new THREE.PointLight(0xff00ff, 0.5, 100);
    pointLight2.position.set(20, 10, -20);
    scene.add(pointLight2);

    // Ground
    const groundGeometry = new THREE.PlaneGeometry(100, 100);
    const groundMaterial = new THREE.MeshStandardMaterial({
        color: 0x1a1f3a,
        roughness: 0.8,
        metalness: 0.2
    });
    const ground = new THREE.Mesh(groundGeometry, groundMaterial);
    ground.rotation.x = -Math.PI / 2;
    ground.receiveShadow = true;
    scene.add(ground);

    // Grid helper
    const gridHelper = new THREE.GridHelper(100, 50, 0x00d4ff, 0x1a1f3a);
    gridHelper.material.opacity = 0.3;
    gridHelper.material.transparent = true;
    scene.add(gridHelper);

    // Controls (simple rotation)
    setupControls();
}

function setupControls() {
    let isDragging = false;
    let previousMousePosition = { x: 0, y: 0 };

    renderer.domElement.addEventListener('mousedown', (e) => {
        isDragging = true;
        previousMousePosition = { x: e.clientX, y: e.clientY };
    });

    renderer.domElement.addEventListener('mousemove', (e) => {
        if (isDragging && buildingGroup) {
            const deltaX = e.clientX - previousMousePosition.x;
            buildingGroup.rotation.y += deltaX * 0.01;
            previousMousePosition = { x: e.clientX, y: e.clientY };
        }
    });

    renderer.domElement.addEventListener('mouseup', () => {
        isDragging = false;
    });

    renderer.domElement.addEventListener('wheel', (e) => {
        e.preventDefault();
        const delta = e.deltaY * 0.01;
        camera.position.z += delta;
        camera.position.z = Math.max(20, Math.min(80, camera.position.z));
    });
}

function createBuilding3D() {
    const floors = parseInt(document.getElementById('totalFloors').value);
    const flatsPerFloor = parseInt(document.getElementById('flatsPerFloor').value);

    buildingGroup = new THREE.Group();

    const floorHeight = 3;
    const flatWidth = 4;
    const flatDepth = 4;
    const wallThickness = 0.2;

    // Building dimensions
    const buildingWidth = flatsPerFloor * flatWidth;
    const buildingHeight = floors * floorHeight;

    // Create each floor
    for (let floor = 0; floor < floors; floor++) {
        const floorGroup = new THREE.Group();
        const yPosition = floor * floorHeight;

        // Floor slab
        const slabGeometry = new THREE.BoxGeometry(buildingWidth, 0.3, flatDepth);
        const slabMaterial = new THREE.MeshStandardMaterial({
            color: 0x2d3561,
            roughness: 0.7,
            metalness: 0.3
        });
        const slab = new THREE.Mesh(slabGeometry, slabMaterial);
        slab.position.y = yPosition;
        slab.castShadow = true;
        slab.receiveShadow = true;
        floorGroup.add(slab);

        // Create flats on this floor
        for (let flat = 0; flat < flatsPerFloor; flat++) {
            const flatGroup = createFlat3D(flatWidth, floorHeight, flatDepth, floor, flat);
            flatGroup.position.x = (flat - flatsPerFloor / 2) * flatWidth + flatWidth / 2;
            flatGroup.position.y = yPosition;
            floorGroup.add(flatGroup);
        }

        buildingGroup.add(floorGroup);
    }

    // Building exterior walls (glass facade)
    createGlassFacade(buildingWidth, buildingHeight, flatDepth);

    // Roof
    createRoof(buildingWidth, flatDepth, buildingHeight);

    // Center the building
    buildingGroup.position.y = 0.5;
    scene.add(buildingGroup);

    // Auto-rotate
    buildingGroup.rotation.y = Math.PI / 6;
}

function createFlat3D(width, height, depth, floorNum, flatNum) {
    const flatGroup = new THREE.Group();

    // Flat number for color variation
    const flatNumber = `${floorNum + 1}${(flatNum + 1).toString().padStart(2, '0')}`;
    const isOccupied = Math.random() > 0.7; // Random occupancy for demo

    // Interior walls
    const wallMaterial = new THREE.MeshStandardMaterial({
        color: isOccupied ? 0xffd700 : 0x3d4575,
        roughness: 0.6,
        metalness: 0.2,
        emissive: isOccupied ? 0x443300 : 0x000000,
        emissiveIntensity: isOccupied ? 0.3 : 0
    });

    const wallGeometry = new THREE.BoxGeometry(width - 0.4, height - 0.6, 0.1);
    const backWall = new THREE.Mesh(wallGeometry, wallMaterial);
    backWall.position.z = -depth / 2 + 0.05;
    flatGroup.add(backWall);

    // Window (glowing if occupied)
    if (isOccupied) {
        const windowGeometry = new THREE.BoxGeometry(width * 0.6, height * 0.4, 0.05);
        const windowMaterial = new THREE.MeshStandardMaterial({
            color: 0xffffaa,
            emissive: 0xffffaa,
            emissiveIntensity: 0.8,
            transparent: true,
            opacity: 0.9
        });
        const window = new THREE.Mesh(windowGeometry, windowMaterial);
        window.position.z = -depth / 2 + 0.1;
        flatGroup.add(window);
    }

    return flatGroup;
}

function createGlassFacade(width, height, depth) {
    const glassMaterial = new THREE.MeshPhysicalMaterial({
        color: 0x88ccff,
        transparent: true,
        opacity: 0.2,
        roughness: 0.1,
        metalness: 0.9,
        reflectivity: 1,
        clearcoat: 1,
        clearcoatRoughness: 0.1
    });

    // Front facade
    const frontGeometry = new THREE.PlaneGeometry(width, height);
    const frontGlass = new THREE.Mesh(frontGeometry, glassMaterial);
    frontGlass.position.z = depth / 2;
    frontGlass.position.y = height / 2;
    buildingGroup.add(frontGlass);

    // Side facades
    const sideGeometry = new THREE.PlaneGeometry(depth, height);

    const leftGlass = new THREE.Mesh(sideGeometry, glassMaterial);
    leftGlass.position.x = -width / 2;
    leftGlass.position.y = height / 2;
    leftGlass.rotation.y = Math.PI / 2;
    buildingGroup.add(leftGlass);

    const rightGlass = new THREE.Mesh(sideGeometry, glassMaterial);
    rightGlass.position.x = width / 2;
    rightGlass.position.y = height / 2;
    rightGlass.rotation.y = -Math.PI / 2;
    buildingGroup.add(rightGlass);
}

function createRoof(width, depth, height) {
    const roofGeometry = new THREE.BoxGeometry(width + 0.5, 0.4, depth + 0.5);
    const roofMaterial = new THREE.MeshStandardMaterial({
        color: 0x1a1f3a,
        roughness: 0.8,
        metalness: 0.4
    });
    const roof = new THREE.Mesh(roofGeometry, roofMaterial);
    roof.position.y = height;
    roof.castShadow = true;
    buildingGroup.add(roof);

    // Helipad (decorative)
    const helipadGeometry = new THREE.CylinderGeometry(2, 2, 0.1, 32);
    const helipadMaterial = new THREE.MeshStandardMaterial({
        color: 0xff6600,
        emissive: 0xff3300,
        emissiveIntensity: 0.3
    });
    const helipad = new THREE.Mesh(helipadGeometry, helipadMaterial);
    helipad.position.y = height + 0.25;
    buildingGroup.add(helipad);
}

function animate() {
    animationId = requestAnimationFrame(animate);

    // Gentle auto-rotation
    if (buildingGroup) {
        buildingGroup.rotation.y += 0.002;
    }

    renderer.render(scene, camera);
}

// Cleanup on view switch
function cleanup3D() {
    if (animationId) {
        cancelAnimationFrame(animationId);
    }
    if (renderer) {
        renderer.dispose();
    }
}

// Handle window resize
window.addEventListener('resize', () => {
    if (renderer && camera) {
        const container = document.getElementById('preview3D');
        if (container && container.style.display !== 'none') {
            const width = container.clientWidth;
            const height = 600;
            camera.aspect = width / height;
            camera.updateProjectionMatrix();
            renderer.setSize(width, height);
        }
    }
});