/* ============================================================
   apartment-builder.js
   Place in: wwwroot/js/apartment-builder.js
   ============================================================ */

let abCur = 1;

/* ── NAVIGATION ── */
function abNext(n) { if (!abValid()) return; abCur = n; abDraw(); if (n === 3) { abSummary(); abRender2D(); } }
function abPrev(n) { abCur = n; abDraw(); }

function abDraw() {
    [1, 2, 3].forEach(n => {
        const el = document.getElementById('ab-s' + n);
        if (el) el.style.display = n === abCur ? 'block' : 'none';
    });
    document.querySelectorAll('.ab-step').forEach(s => {
        s.classList.remove('is-active', 'is-done');
        const n = +s.dataset.step;
        const num = s.querySelector('.ab-step-num');
        if (n === abCur) { s.classList.add('is-active'); if (num) num.textContent = n; }
        else if (n < abCur) { s.classList.add('is-done'); if (num) num.textContent = '✓'; }
        else { if (num) num.textContent = n; }
    });
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

/* ── VALIDATION ── */
function abValid() {
    if (abCur === 1) {
        const n = document.getElementById('apartmentName')?.value.trim();
        const a = document.getElementById('address')?.value.trim();
        if (!n || !a) { abToast('Please fill in Building Name and Address', 'err'); return false; }
    }
    if (abCur === 2) {
        const f = +document.getElementById('totalFloors')?.value;
        const p = +document.getElementById('flatsPerFloor')?.value;
        if (!f || f < 1 || f > 50) { abToast('Floors must be 1–50', 'err'); return false; }
        if (!p || p < 1 || p > 20) { abToast('Flats per floor must be 1–20', 'err'); return false; }
    }
    return true;
}

/* ── COUNTER BUTTONS ── */
function abChFloors(d) {
    const i = document.getElementById('totalFloors');
    i.value = Math.max(1, Math.min(50, (+i.value || 1) + d));
    abStats(); abPulse('ab-df');
}
function abChFlats(d) {
    const i = document.getElementById('flatsPerFloor');
    i.value = Math.max(1, Math.min(20, (+i.value || 1) + d));
    abStats(); abPulse('ab-dp');
}

/* ── STATS UPDATE ── */
function abStats() {
    const f = +document.getElementById('totalFloors')?.value || 0;
    const p = +document.getElementById('flatsPerFloor')?.value || 0;
    const df = document.getElementById('ab-df'); if (df) df.textContent = f;
    const dp = document.getElementById('ab-dp'); if (dp) dp.textContent = p;
    const dt = document.getElementById('ab-dt');
    if (dt) { dt.textContent = f * p; abPulse('ab-dt'); }
}

function abPulse(id) {
    const e = document.getElementById(id);
    if (!e) return;
    e.style.transform = 'scale(1.35)';
    e.style.color = '#6366f1';
    setTimeout(() => { e.style.transform = ''; e.style.color = ''; }, 220);
}

/* ── SUMMARY (Step 3) ── */
function abSummary() {
    const f = +document.getElementById('totalFloors')?.value || 0;
    const p = +document.getElementById('flatsPerFloor')?.value || 0;
    const addr = [
        document.getElementById('address')?.value,
        document.getElementById('city')?.value,
        document.getElementById('state')?.value
    ].filter(Boolean).join(', ');
    const set = (id, v) => { const e = document.getElementById(id); if (e) e.textContent = v; };
    set('ab-ss-name', document.getElementById('apartmentName')?.value || '-');
    set('ab-ss-addr', addr || '-');
    set('ab-ss-fl', f);
    set('ab-ss-fp', p);
    set('ab-ss-tot', f * p);
}

/* ── 2D PREVIEW ── */
function abRender2D() {
    const wrap = document.getElementById('ab-p2d');
    if (!wrap) return;
    wrap.innerHTML = '';

    const fl = +document.getElementById('totalFloors')?.value || 1;
    const fp = +document.getElementById('flatsPerFloor')?.value || 1;
    const nm = document.getElementById('apartmentName')?.value || 'Building';

    const title = document.createElement('div');
    title.className = 'ab-bld-title';
    title.textContent = '🏢 ' + nm;
    wrap.appendChild(title);

    const bld = document.createElement('div');
    bld.className = 'ab-bld-wrap';

    for (let f = fl; f >= 1; f--) {
        const row = document.createElement('div');
        row.className = 'ab-floor-row';

        const lbl = document.createElement('div');
        lbl.className = 'ab-floor-lbl';
        lbl.textContent = 'F' + f;
        row.appendChild(lbl);

        const frow = document.createElement('div');
        frow.className = 'ab-flats-row';

        for (let k = 1; k <= fp; k++) {
            const num = f + k.toString().padStart(2, '0');
            const box = document.createElement('div');
            box.className = 'ab-flat';
            box.title = 'Flat ' + num + ' — Vacant';
            box.innerHTML = `<span class="ab-flat-num">${num}</span><i class="fas fa-key ab-flat-ico"></i>`;
            frow.appendChild(box);
        }
        row.appendChild(frow);
        bld.appendChild(row);
    }
    wrap.appendChild(bld);
}

/* ── VIEW TOGGLE (2D / 3D) ── */
function abView(v) {
    const d2 = document.getElementById('ab-p2d');
    const d3 = document.getElementById('ab-p3d');
    const b2 = document.getElementById('ab-t2d');
    const b3 = document.getElementById('ab-t3d');

    if (v === '2d') {
        d2.style.display = 'block';
        d3.style.display = 'none';
        b2.classList.add('on');
        b3.classList.remove('on');
        abRender2D();
    } else {
        d2.style.display = 'none';
        d3.style.display = 'block';
        b2.classList.remove('on');
        b3.classList.add('on');
        if (typeof THREE === 'undefined') {
            d3.innerHTML = '<p style="text-align:center;padding:40px;color:#6366f1;">Three.js not loaded</p>';
            return;
        }
        if (typeof initThreeJS === 'function' && typeof createBuilding3D === 'function') {
            try { d3.innerHTML = ''; initThreeJS(d3); createBuilding3D(); animate(); }
            catch (e) { d3.innerHTML = `<p style="padding:40px;text-align:center;color:#ef4444;">3D error: ${e.message}</p>`; }
        }
    }
}

/* ── SUBMIT ── */
async function abSubmit() {
    const data = {
        name: document.getElementById('apartmentName')?.value || '',
        address: document.getElementById('address')?.value || '',
        city: document.getElementById('city')?.value || '',
        state: document.getElementById('state')?.value || '',
        pinCode: document.getElementById('pinCode')?.value || '',
        totalFloors: +document.getElementById('totalFloors')?.value || 0,
        flatsPerFloor: +document.getElementById('flatsPerFloor')?.value || 0
    };

    const ov = document.getElementById('ab-ov');
    if (ov) ov.style.display = 'flex';

    try {
        const res = await fetch('/ApartmentBuilder/CreateApartment', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        if (!res.headers.get('content-type')?.includes('application/json')) throw new Error('Non-JSON server response');
        const r = await res.json();
        if (r.success) {
            abToast('🎉 Apartment created!', 'ok');
            setTimeout(() => { window.location.href = '/ApartmentBuilder/ManageApartments'; }, 1700);
        } else {
            abToast(r.message || 'Failed to create apartment', 'err');
        }
    } catch (e) {
        abToast('Error: ' + e.message, 'err');
    } finally {
        if (ov) ov.style.display = 'none';
    }
}

/* ── TOAST ── */
function abToast(msg, type = 'ok') {
    const t = document.createElement('div');
    t.className = 'ab-toast ' + type;
    t.innerHTML = `<i class="fas fa-${type === 'ok' ? 'check-circle' : 'exclamation-circle'}"></i><span>${msg}</span>`;
    document.body.appendChild(t);
    setTimeout(() => t.classList.add('show'), 40);
    setTimeout(() => { t.classList.remove('show'); setTimeout(() => t.remove(), 350); }, 3600);
}

/* ── INIT ── */
document.addEventListener('DOMContentLoaded', () => {
    abStats();

    // JS runtime override — ensures white text even if Bootstrap loads late
    document.querySelectorAll('.ab-wrap input').forEach(inp => {
        const fix = () => {
            inp.style.setProperty('color', '#f1f5f9', 'important');
            inp.style.setProperty('-webkit-text-fill-color', '#f1f5f9', 'important');
            inp.style.setProperty('background', '#0d1535', 'important');
        };
        fix();
        inp.addEventListener('focus', fix);
        inp.addEventListener('input', fix);
    });
});





















// wwwroot/js/apartment-builder.js

//console.log("apartment-builder.js loaded");

//let currentStep = 1;

/* ============ STEP NAVIGATION ============ */
/*
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

// ============ VALIDATION ============ 

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

/* ============ FLOOR / FLAT CONTROLS ============ 

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

/* ============ PREVIEW SUMMARY ============ 

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

/* ============ 2D PREVIEW RENDER ============ 

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

/* ============ VIEW TOGGLE ============ 

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

/* ============ 3D PREVIEW ============ 

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

/* ============ SUBMIT APARTMENT ============ 

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

/* ============ NOTIFICATION SYSTEM ============ 

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

/* ============ INITIALIZATION ============ 

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM loaded, initializing apartment builder...');
    updatePreview();
    updateStepDisplay();
});


*/





















