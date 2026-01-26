// wwwroot/js/apartment-builder.js

console.log("apartment-builder.js loaded");

let currentStep = 1;

/* ============ STEP NAVIGATION ============ */

function nextStep(step) {
    if (validateCurrentStep()) {
        currentStep = step;
        updateStepDisplay();

        if (step === 3) {
            updatePreviewSummary();
            render2DPreview();
        }
    }
}

function previousStep(step) {
    currentStep = step;
    updateStepDisplay();
}

function updateStepDisplay() {
    // Hide all steps
    document.querySelectorAll('.builder-step').forEach(s => s.style.display = 'none');

    // Show current step
    const currentStepElement = document.getElementById(`step${currentStep}`);
    if (currentStepElement) {
        currentStepElement.style.display = 'block';
    }

    // Update step indicators
    document.querySelectorAll('.step').forEach(step => {
        step.classList.remove('active', 'completed');
        const stepNum = parseInt(step.dataset.step);

        if (stepNum === currentStep) {
            step.classList.add('active');
        } else if (stepNum < currentStep) {
            step.classList.add('completed');
        }
    });

    // Scroll to top
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

/* ============ VALIDATION ============ */

function validateCurrentStep() {
    if (currentStep === 1) {
        const apartmentName = document.getElementById('apartmentName');
        const address = document.getElementById('address');

        if (!apartmentName || !address || !apartmentName.value.trim() || !address.value.trim()) {
            showNotification("Please fill in all required fields", "error");
            return false;
        }
    }

    if (currentStep === 2) {
        const floors = parseInt(document.getElementById('totalFloors').value);
        const flats = parseInt(document.getElementById('flatsPerFloor').value);

        if (floors < 1 || floors > 50 || flats < 1 || flats > 20) {
            showNotification('Please enter valid building dimensions', 'error');
            return false;
        }
    }

    return true;
}

/* ============ FLOOR / FLAT CONTROLS ============ */

function changeFloors(delta) {
    const input = document.getElementById('totalFloors');
    if (input) {
        input.value = Math.max(1, Math.min(50, parseInt(input.value || 1) + delta));
        updatePreview();
    }
}

function changeFlats(delta) {
    const input = document.getElementById('flatsPerFloor');
    if (input) {
        input.value = Math.max(1, Math.min(20, parseInt(input.value || 1) + delta));
        updatePreview();
    }
}

function updatePreview() {
    const floors = parseInt(document.getElementById('totalFloors')?.value || 0);
    const flats = parseInt(document.getElementById('flatsPerFloor')?.value || 0);
    const totalFlats = floors * flats;

    const displayFloors = document.getElementById('displayFloors');
    const displayFlatsPerFloor = document.getElementById('displayFlatsPerFloor');
    const displayTotalFlats = document.getElementById('displayTotalFlats');

    if (displayFloors) displayFloors.textContent = floors;
    if (displayFlatsPerFloor) displayFlatsPerFloor.textContent = flats;
    if (displayTotalFlats) displayTotalFlats.textContent = totalFlats;
}

/* ============ PREVIEW SUMMARY ============ */

function updatePreviewSummary() {
    const apartmentName = document.getElementById('apartmentName');
    const address = document.getElementById('address');
    const city = document.getElementById('city');
    const state = document.getElementById('state');
    const totalFloors = document.getElementById('totalFloors');
    const flatsPerFloor = document.getElementById('flatsPerFloor');
    const displayTotalFlats = document.getElementById('displayTotalFlats');

    const summaryName = document.getElementById('summaryName');
    const summaryAddress = document.getElementById('summaryAddress');
    const summaryFloors = document.getElementById('summaryFloors');
    const summaryFlatsPerFloor = document.getElementById('summaryFlatsPerFloor');
    const summaryTotalFlats = document.getElementById('summaryTotalFlats');

    if (summaryName && apartmentName) {
        summaryName.textContent = apartmentName.value;
    }

    if (summaryAddress) {
        const fullAddress = [
            address?.value,
            city?.value,
            state?.value
        ].filter(Boolean).join(', ');
        summaryAddress.textContent = fullAddress || '-';
    }

    if (summaryFloors && totalFloors) {
        summaryFloors.textContent = totalFloors.value;
    }

    if (summaryFlatsPerFloor && flatsPerFloor) {
        summaryFlatsPerFloor.textContent = flatsPerFloor.value;
    }

    if (summaryTotalFlats && displayTotalFlats) {
        summaryTotalFlats.textContent = displayTotalFlats.textContent;
    }
}

/* ============ 2D PREVIEW RENDER ============ */

function render2DPreview() {
    const container = document.getElementById('preview2D');
    if (!container) {
        console.error('preview2D container not found');
        return;
    }

    container.innerHTML = '';

    const floors = parseInt(document.getElementById('totalFloors')?.value || 1);
    const flatsPerFloor = parseInt(document.getElementById('flatsPerFloor')?.value || 1);
    const buildingName = document.getElementById('apartmentName')?.value || 'Apartment Building';

    // Create building title
    const title = document.createElement('div');
    title.className = 'building-title';
    title.innerHTML = `<h3>${buildingName}</h3>`;
    container.appendChild(title);

    // Create building structure
    const building = document.createElement('div');
    building.className = 'building-2d';

    // Render floors (top to bottom)
    for (let floor = floors; floor >= 1; floor--) {
        const floorDiv = document.createElement('div');
        floorDiv.className = 'floor-2d';

        const floorLabel = document.createElement('div');
        floorLabel.className = 'floor-label';
        floorLabel.textContent = `Floor ${floor}`;
        floorDiv.appendChild(floorLabel);

        const flatsContainer = document.createElement('div');
        flatsContainer.className = 'flats-container';

        // Render flats
        for (let flat = 1; flat <= flatsPerFloor; flat++) {
            const flatNumber = `${floor}${flat.toString().padStart(2, '0')}`;
            const flatDiv = document.createElement('div');
            flatDiv.className = 'flat-2d vacant';
            flatDiv.innerHTML = `
                <div class="flat-number">${flatNumber}</div>
                <div class="flat-status">
                    <i class="fas fa-home"></i>
                    <span>Vacant</span>
                </div>
            `;

            flatDiv.addEventListener('mouseenter', function () {
                this.style.transform = 'scale(1.05)';
            });

            flatDiv.addEventListener('mouseleave', function () {
                this.style.transform = 'scale(1)';
            });

            flatsContainer.appendChild(flatDiv);
        }

        floorDiv.appendChild(flatsContainer);
        building.appendChild(floorDiv);
    }

    container.appendChild(building);
}

/* ============ VIEW TOGGLE ============ */

function switchView(view) {
    console.log('Switching view to:', view);

    const preview2D = document.getElementById('preview2D');
    const preview3D = document.getElementById('preview3D');
    const btn2D = document.getElementById('btn2D');
    const btn3D = document.getElementById('btn3D');

    // Update button states
    if (btn2D) btn2D.classList.remove('active');
    if (btn3D) btn3D.classList.remove('active');

    // Toggle views
    if (view === '2d') {
        if (preview2D) preview2D.style.display = 'block';
        if (preview3D) preview3D.style.display = 'none';
        if (btn2D) btn2D.classList.add('active');
        render2DPreview();
    } else if (view === '3d') {
        if (preview2D) preview2D.style.display = 'none';
        if (preview3D) preview3D.style.display = 'block';
        if (btn3D) btn3D.classList.add('active');
        render3DPreview();
    }
}

/* ============ 3D PREVIEW ============ */

function render3DPreview() {
    const container = document.getElementById('preview3D');
    if (!container) {
        console.error('preview3D container not found');
        return;
    }

    // Check if Three.js is loaded
    if (typeof THREE === 'undefined') {
        container.innerHTML = '<p style="text-align: center; padding: 50px; color: #ff6b6b;">Three.js library not loaded. Please refresh the page.</p>';
        console.error('THREE is not defined. Make sure three.js is loaded.');
        return;
    }

    // Check if 3D visualizer functions are available
    if (typeof initThreeJS === 'function' && typeof createBuilding3D === 'function') {
        try {
            container.innerHTML = '';
            initThreeJS(container);
            createBuilding3D();
            animate();
        } catch (error) {
            console.error('Error initializing 3D view:', error);
            container.innerHTML = `<p style="text-align: center; padding: 50px; color: #ff6b6b;">Error loading 3D view: ${error.message}</p>`;
        }
    } else {
        console.error('3D visualizer functions not found');
        container.innerHTML = '<p style="text-align: center; padding: 50px; color: #ff6b6b;">3D visualizer not loaded properly. Please check console.</p>';
    }
}

/* ============ SUBMIT APARTMENT ============ */

async function submitApartment() {
    console.log("Submit clicked");

    const apartmentName = document.getElementById('apartmentName');
    const address = document.getElementById('address');
    const city = document.getElementById('city');
    const state = document.getElementById('state');
    const pinCode = document.getElementById('pinCode');
    const totalFloors = document.getElementById('totalFloors');
    const flatsPerFloor = document.getElementById('flatsPerFloor');

    const data = {
        name: apartmentName?.value || '',
        address: address?.value || '',
        city: city?.value || '',
        state: state?.value || '',
        pinCode: pinCode?.value || '',
        totalFloors: parseInt(totalFloors?.value || 0),
        flatsPerFloor: parseInt(flatsPerFloor?.value || 0)
    };

    console.log('Sending data:', data);

    // Show loading overlay
    const loadingOverlay = document.getElementById('loadingOverlay');
    if (loadingOverlay) {
        loadingOverlay.style.display = 'flex';
    }

    try {
        const response = await fetch('/ApartmentBuilder/CreateApartment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        console.log("Response status:", response.status);

        // Check if response is JSON
        const contentType = response.headers.get('content-type');
        let result;

        if (contentType && contentType.includes('application/json')) {
            result = await response.json();
            console.log("JSON result:", result);
        } else {
            const text = await response.text();
            console.log("Text response:", text);
            throw new Error('Server returned non-JSON response. Check console for details.');
        }

        if (result.success) {
            showNotification('Apartment created successfully!', 'success');
            setTimeout(() => {
                window.location.href = '/ApartmentBuilder/ManageApartments';
            }, 2000);
        } else {
            showNotification('Failed to create apartment: ' + (result.message || 'Unknown error'), 'error');
        }
    } catch (error) {
        console.error('Error details:', error);
        showNotification('An error occurred: ' + error.message, 'error');
    } finally {
        if (loadingOverlay) {
            loadingOverlay.style.display = 'none';
        }
    }
}

/* ============ NOTIFICATION SYSTEM ============ */

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;

    const icon = type === 'success' ? 'check-circle' :
        type === 'error' ? 'exclamation-circle' :
            'info-circle';

    notification.innerHTML = `
        <i class="fas fa-${icon}"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 100);

    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

/* ============ INITIALIZATION ============ */

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded, initializing apartment builder...');
    updatePreview();
    updateStepDisplay();
});
























/*console.log("apartment-builder.js loaded");

let currentStep = 1;


function nextStep(step) {
    if (validateCurrentStep()) {
        currentStep = step;
        updateStepDisplay();

        if (step === 3) {
            updatePreviewSummary();
            render2DPreview();
        }
    }
}

function previousStep(step) {
    currentStep = step;
    updateStepDisplay();
}

function updateStepDisplay() {
    // Hide all steps
    document.querySelectorAll('.builder-step').forEach(s => s.style.display = 'none');

    // Show current step
    const currentStepElement = document.getElementById(`step${currentStep}`);
    if (currentStepElement) {
        currentStepElement.style.display = 'block';
    }

    // Update step indicators
    document.querySelectorAll('.step').forEach(step => {
        step.classList.remove('active', 'completed');
        const stepNum = parseInt(step.dataset.step);

        if (stepNum === currentStep) {
            step.classList.add('active');
        } else if (stepNum < currentStep) {
            step.classList.add('completed');
        }
    });

    // Scroll to top
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

/* ---------------- VALIDATION ---------------- 

function validateCurrentStep() {
    if (currentStep === 1) {
        const apartmentName = document.getElementById('apartmentName');
        const address = document.getElementById('address');

        if (!apartmentName || !address || !apartmentName.value.trim() || !address.value.trim()) {
            showNotification("Please fill in all required fields", "error");
            return false;
        }
    }

    if (currentStep === 2) {
        const floors = parseInt(document.getElementById('totalFloors').value);
        const flats = parseInt(document.getElementById('flatsPerFloor').value);

        if (floors < 1 || floors > 50 || flats < 1 || flats > 20) {
            showNotification('Please enter valid building dimensions', 'error');
            return false;
        }
    }

    return true;
}

/* ---------------- FLOOR / FLAT CONTROLS ---------------- 

function changeFloors(delta) {
    const input = document.getElementById('totalFloors');
    if (input) {
        input.value = Math.max(1, Math.min(50, parseInt(input.value || 1) + delta));
        updatePreview();
    }
}

function changeFlats(delta) {
    const input = document.getElementById('flatsPerFloor');
    if (input) {
        input.value = Math.max(1, Math.min(20, parseInt(input.value || 1) + delta));
        updatePreview();
    }
}

function updatePreview() {
    const floors = parseInt(document.getElementById('totalFloors')?.value || 0);
    const flats = parseInt(document.getElementById('flatsPerFloor')?.value || 0);
    const totalFlats = floors * flats;

    const displayFloors = document.getElementById('displayFloors');
    const displayFlatsPerFloor = document.getElementById('displayFlatsPerFloor');
    const displayTotalFlats = document.getElementById('displayTotalFlats');

    if (displayFloors) displayFloors.textContent = floors;
    if (displayFlatsPerFloor) displayFlatsPerFloor.textContent = flats;
    if (displayTotalFlats) displayTotalFlats.textContent = totalFlats;
}

/* ---------------- PREVIEW SUMMARY ---------------- 

function updatePreviewSummary() {
    const apartmentName = document.getElementById('apartmentName');
    const address = document.getElementById('address');
    const city = document.getElementById('city');
    const state = document.getElementById('state');
    const totalFloors = document.getElementById('totalFloors');
    const flatsPerFloor = document.getElementById('flatsPerFloor');
    const displayTotalFlats = document.getElementById('displayTotalFlats');

    const summaryName = document.getElementById('summaryName');
    const summaryAddress = document.getElementById('summaryAddress');
    const summaryFloors = document.getElementById('summaryFloors');
    const summaryFlatsPerFloor = document.getElementById('summaryFlatsPerFloor');
    const summaryTotalFlats = document.getElementById('summaryTotalFlats');

    if (summaryName && apartmentName) {
        summaryName.textContent = apartmentName.value;
    }

    if (summaryAddress) {
        const fullAddress = [
            address?.value,
            city?.value,
            state?.value
        ].filter(Boolean).join(', ');
        summaryAddress.textContent = fullAddress || '-';
    }

    if (summaryFloors && totalFloors) {
        summaryFloors.textContent = totalFloors.value;
    }

    if (summaryFlatsPerFloor && flatsPerFloor) {
        summaryFlatsPerFloor.textContent = flatsPerFloor.value;
    }

    if (summaryTotalFlats && displayTotalFlats) {
        summaryTotalFlats.textContent = displayTotalFlats.textContent;
    }
}

/* ---------------- 2D PREVIEW RENDER ---------------- 

function render2DPreview() {
    const container = document.getElementById('preview2D');
    if (!container) {
        console.error('preview2D container not found');
        return;
    }

    container.innerHTML = '';

    const floors = parseInt(document.getElementById('totalFloors')?.value || 1);
    const flatsPerFloor = parseInt(document.getElementById('flatsPerFloor')?.value || 1);
    const buildingName = document.getElementById('apartmentName')?.value || 'Apartment Building';

    // Create building title
    const title = document.createElement('div');
    title.className = 'building-title';
    title.innerHTML = `<h3>${buildingName}</h3>`;
    container.appendChild(title);

    // Create building structure
    const building = document.createElement('div');
    building.className = 'building-2d';

    // Render floors (top to bottom)
    for (let floor = floors; floor >= 1; floor--) {
        const floorDiv = document.createElement('div');
        floorDiv.className = 'floor-2d';

        const floorLabel = document.createElement('div');
        floorLabel.className = 'floor-label';
        floorLabel.textContent = `Floor ${floor}`;
        floorDiv.appendChild(floorLabel);

        const flatsContainer = document.createElement('div');
        flatsContainer.className = 'flats-container';

        // Render flats
        for (let flat = 1; flat <= flatsPerFloor; flat++) {
            const flatNumber = `${floor}${flat.toString().padStart(2, '0')}`;
            const flatDiv = document.createElement('div');
            flatDiv.className = 'flat-2d vacant';
            flatDiv.innerHTML = `
                <div class="flat-number">${flatNumber}</div>
                <div class="flat-status">
                    <i class="fas fa-home"></i>
                    <span>Vacant</span>
                </div>
            `;

            flatDiv.addEventListener('mouseenter', function () {
                this.style.transform = 'scale(1.05)';
            });

            flatDiv.addEventListener('mouseleave', function () {
                this.style.transform = 'scale(1)';
            });

            flatsContainer.appendChild(flatDiv);
        }

        floorDiv.appendChild(flatsContainer);
        building.appendChild(floorDiv);
    }

    container.appendChild(building);
}

/* ---------------- VIEW TOGGLE ---------------- 

function switchView(view, event) {
    const preview2D = document.getElementById('preview2D');
    const preview3D = document.getElementById('preview3D');

    // Update button states
    document.querySelectorAll('.btn-toggle').forEach(btn => {
        btn.classList.remove('active');
    });

    if (event && event.target) {
        event.target.closest('.btn-toggle')?.classList.add('active');
    }

    // Toggle views
    if (view === '2d') {
        if (preview2D) preview2D.style.display = 'block';
        if (preview3D) preview3D.style.display = 'none';
        render2DPreview();
    } else {
        if (preview2D) preview2D.style.display = 'none';
        if (preview3D) preview3D.style.display = 'block';
        render3DPreview();
    }
}

/* ---------------- 3D PREVIEW (PLACEHOLDER) ---------------- 

function render3DPreview() {
    const container = document.getElementById('preview3D');
    if (!container) return;

    container.innerHTML = '<p style="text-align: center; padding: 50px; color: #888;">3D Preview - Coming Soon</p>';
}

/* ---------------- SUBMIT ---------------- 

async function submitApartment() {
    console.log("Submit clicked");

    const apartmentName = document.getElementById('apartmentName');
    const address = document.getElementById('address');
    const city = document.getElementById('city');
    const state = document.getElementById('state');
    const pinCode = document.getElementById('pinCode');
    const totalFloors = document.getElementById('totalFloors');
    const flatsPerFloor = document.getElementById('flatsPerFloor');

    const data = {
        name: apartmentName?.value || '',
        address: address?.value || '',
        city: city?.value || '',
        state: state?.value || '',
        pinCode: pinCode?.value || '',
        totalFloors: parseInt(totalFloors?.value || 0),
        flatsPerFloor: parseInt(flatsPerFloor?.value || 0)
    };

    // Show loading overlay
    const loadingOverlay = document.getElementById('loadingOverlay');
    if (loadingOverlay) {
        loadingOverlay.style.display = 'flex';
    }

    try {
        const response = await fetch('/ApartmentBuilder/CreateApartment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        console.log("POST status:", response.status);
        const result = await response.json();

        if (result.success) {
            showNotification('Apartment created successfully!', 'success');
            setTimeout(() => {
                window.location.href = '/ApartmentBuilder/ManageApartments';
            }, 2000);
        } else {
            showNotification('Failed to create apartment: ' + (result.message || 'Unknown error'), 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('An error occurred: ' + error.message, 'error');
    } finally {
        if (loadingOverlay) {
            loadingOverlay.style.display = 'none';
        }
    }
}

/* ---------------- NOTIFICATION SYSTEM ---------------- 

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;

    const icon = type === 'success' ? 'check-circle' :
        type === 'error' ? 'exclamation-circle' :
            'info-circle';

    notification.innerHTML = `
        <i class="fas fa-${icon}"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 100);

    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

/* ---------------- INITIALIZATION ---------------- 

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded, initializing...');
    updatePreview();
    updateStepDisplay();
});
*/




















//// wwwroot/js/apartment-builder.js

//let currentStep = 1;

//// Step Navigation
//function nextStep(step) {
//    if (validateCurrentStep()) {
//        currentStep = step;
//        updateStepDisplay();

//        if (step === 3) {
//            updatePreviewSummary();
//            render2DPreview();
//        }
//    }
//}

//function previousStep(step) {
//    currentStep = step;
//    updateStepDisplay();
//}

//function updateStepDisplay() {
//    // Hide all steps
//    document.querySelectorAll('.builder-step').forEach(step => {
//        step.style.display = 'none';
//    });

//    // Show current step
//    document.getElementById(`step${currentStep}`).style.display = 'block';

//    // Update step indicator
//    document.querySelectorAll('.step').forEach(step => {
//        step.classList.remove('active', 'completed');
//        const stepNum = parseInt(step.dataset.step);

//        if (stepNum === currentStep) {
//            step.classList.add('active');
//        } else if (stepNum < currentStep) {
//            step.classList.add('completed');
//        }
//    });

//    // Scroll to top
//    window.scrollTo({ top: 0, behavior: 'smooth' });
//}

//function validateCurrentStep() {
//    if (currentStep === 1) {
//        const name = document.getElementById('apartmentName').value.trim();
//        const address = document.getElementById('address').value.trim();

//        if (!name || !address) {
//            showNotification('Please fill in all required fields', 'error');
//            return false;
//        }
//    }

//    if (currentStep === 2) {
//        const floors = parseInt(document.getElementById('totalFloors').value);
//        const flats = parseInt(document.getElementById('flatsPerFloor').value);

//        if (floors < 1 || floors > 50 || flats < 1 || flats > 20) {
//            showNotification('Please enter valid building dimensions', 'error');
//            return false;
//        }
//    }

//    return true;
//}

//// Number Input Controls
//function changeFloors(delta) {
//    const input = document.getElementById('totalFloors');
//    let value = parseInt(input.value) + delta;
//    value = Math.max(1, Math.min(50, value));
//    input.value = value;
//    updatePreview();
//}

//function changeFlats(delta) {
//    const input = document.getElementById('flatsPerFloor');
//    let value = parseInt(input.value) + delta;
//    value = Math.max(1, Math.min(20, value));
//    input.value = value;
//    updatePreview();
//}

//function updatePreview() {
//    const floors = parseInt(document.getElementById('totalFloors').value);
//    const flatsPerFloor = parseInt(document.getElementById('flatsPerFloor').value);
//    const totalFlats = floors * flatsPerFloor;

//    document.getElementById('displayFloors').textContent = floors;
//    document.getElementById('displayFlatsPerFloor').textContent = flatsPerFloor;
//    document.getElementById('displayTotalFlats').textContent = totalFlats;
//}
//function updatePreviewSummary() {
//    document.getElementById('summaryName').textContent =
//        document.getElementById('apartmentName').value;
//    const address = document.getElementById('address').value;
//    const city = document.getElementById('city').value;
//    const state = document.getElementById('state').value;
//    const fullAddress = [address, city, state].filter(Boolean).join(', ');
//    document.getElementById('summaryAddress').textContent = fullAddress;

//    document.getElementById('summaryFloors').textContent =
//        document.getElementById('totalFloors').value;
//    document.getElementById('summaryFlatsPerFloor').textContent =
//        document.getElementById('flatsPerFloor').value;
//    document.getElementById('summaryTotalFlats').textContent =
//        document.getElementById('displayTotalFlats').textContent;

//    // 2D Preview Rendering
//    function render2DPreview() {
//        const container = document.getElementById('preview2D');
//        container.innerHTML = '';
//        const floors = parseInt(document.getElementById('totalFloors').value);
//        const flatsPerFloor = parseInt(document.getElementById('flatsPerFloor').value);
//        const buildingName = document.getElementById('apartmentName').value;

//        // Create building title
//        const title = document.createElement('div');
//        title.className = 'building-title';
//        title.innerHTML = `<h3>${buildingName}</h3>`;
//        container.appendChild(title);

//        // Create building structure
//        const building = document.createElement('div');
//        building.className = 'building-2d';

//        // Render floors (top to bottom)
//        for (let floor = floors; floor >= 1; floor--) {
//            const floorDiv = document.createElement('div');
//            floorDiv.className = 'floor-2d';

//            const floorLabel = document.createElement('div');
//            floorLabel.className = 'floor-label';
//            floorLabel.textContent = `Floor ${floor}`;
//            floorDiv.appendChild(floorLabel);

//            const flatsContainer = document.createElement('div');
//            flatsContainer.className = 'flats-container';

//            // Render flats
//            for (let flat = 1; flat <= flatsPerFloor; flat++) {
//                const flatNumber = `${floor}${flat.toString().padStart(2, '0')}`;
//                const flatDiv = document.createElement('div');
//                flatDiv.className = 'flat-2d vacant';
//                flatDiv.innerHTML = `
//            <div class="flat-number">${flatNumber}</div>
//            <div class="flat-status">
//                <i class="fas fa-home"></i>
//                <span>Vacant</span>
//            </div>
//        `;

//                flatDiv.addEventListener('mouseenter', function () {
//                    this.style.transform = 'scale(1.05)';
//                });

//                flatDiv.addEventListener('mouseleave', function () {
//                    this.style.transform = 'scale(1)';
//                });

//                flatsContainer.appendChild(flatDiv);
//            }

//            floorDiv.appendChild(flatsContainer);
//            building.appendChild(floorDiv);
//        }

//        container.appendChild(building);
//        // View Toggle
//        function switchView(view) {
//            document.querySelectorAll('.btn-toggle').forEach(btn => {
//                btn.classList.remove('active');
//            });
//            if (view === '2d') {
//                document.getElementById('preview2D').style.display = 'block';
//                document.getElementById('preview3D').style.display = 'none';
//                event.target.closest('.btn-toggle').classList.add('active');
//                render2DPreview();
//            } else {
//                document.getElementById('preview2D').style.display = 'none';
//                document.getElementById('preview3D').style.display = 'block';
//                event.target.closest('.btn-toggle').classList.add('active');
//                render3DPreview();
//            }
//            // Submit Apartment
//            async function submitApartment() {
//                const data = {
//                    name: document.getElementById('apartmentName').value,
//                    address: document.getElementById('address').value,
//                    city: document.getElementById('city').value,
//                    state: document.getElementById('state').value,
//                    pinCode: document.getElementById('pinCode').value,
//                    totalFloors: parseInt(document.getElementById('totalFloors').value),
//                    flatsPerFloor: parseInt(document.getElementById('flatsPerFloor').value)
//                };
//                document.getElementById('loadingOverlay').style.display = 'flex';

//                try {
//                    const response = await fetch('/ApartmentBuilder/CreateApartment', {
//                        method: 'POST',
//                        headers: {
//                            'Content-Type': 'application/json'
//                        },
//                        body: JSON.stringify(data)
//                    });

//                    const result = await response.json();

//                    if (result.success) {
//                        showNotification('Apartment created successfully!', 'success');
//                        setTimeout(() => {
//                            window.location.href = '/ApartmentBuilder/ManageApartments';
//                        }, 2000);
//                    } else {
//                        showNotification('Failed to create apartment: ' + result.message, 'error');
//                    }
//                } catch (error) {
//                    showNotification('An error occurred: ' + error.message, 'error');
//                } finally {
//                    document.getElementById('loadingOverlay').style.display = 'none';
//                }
//                // Notification System
//                function showNotification(message, type = 'info') {
//                    const notification = document.createElement('div');
//                    notification.className = notification notification - ${ type };
//                    notification.innerHTML = <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'}"></i>         <span>${message}</span>;
//                    document.body.appendChild(notification);

//                    setTimeout(() => {
//                        notification.classList.add('show');
//                    }, 100);

//                    setTimeout(() => {
//                        notification.classList.remove('show');
//                        setTimeout(() => notification.remove(), 300);
//                    }, 3000);
//                    // Initialize
//                    document.addEventListener('DOMContentLoaded', function () {
//                        updatePreview();
//                    });


