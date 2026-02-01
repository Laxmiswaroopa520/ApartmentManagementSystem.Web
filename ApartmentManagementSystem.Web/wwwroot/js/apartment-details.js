/// ═══════════════════════════════════════════════════════════════════════════
// apartment-details.js - SIMPLIFIED MANAGER ASSIGNMENT
// ═══════════════════════════════════════════════════════════════════════════

function assignManager() {
    const apartmentId = getApartmentIdFromUrl();

    const modalHtml = `
        <div class="modal fade" id="assignManagerModal" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content" style="background: #1a1f3a; color: white; border: 1px solid #667eea;">
                    
                    <div class="modal-header" style="border-bottom: 1px solid #2d3748;">
                        <h5 class="modal-title">
                            <i class="fas fa-user-tie" style="color: #667eea;"></i> Assign Building Manager
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>

                    <div class="modal-body">
                        
                        <!-- TAB BUTTONS -->
                        <div class="btn-group w-100 mb-4" role="group" style="background: #0a0e27; border-radius: 10px; padding: 5px;">
                            <button type="button" class="btn-tab active" id="btnTabResident" onclick="switchTab('resident')">
                                <i class="fas fa-home"></i> From This Apartment
                            </button>
                            <button type="button" class="btn-tab" id="btnTabExternal" onclick="switchTab('external')">
                                <i class="fas fa-user-plus"></i> Any Person
                            </button>
                        </div>

                        <!-- PANEL 1: FROM APARTMENT -->
                        <div id="panelResident">
                            <div class="alert" style="background: rgba(102, 126, 234, 0.1); border: 1px solid #667eea; border-radius: 10px; padding: 15px; margin-bottom: 20px;">
                                <i class="fas fa-info-circle"></i> Select a resident owner from this apartment
                            </div>
                            
                            <label class="form-label fw-bold" style="color: white;">
                                <i class="fas fa-users"></i> Select Resident
                            </label>
                            <select class="form-select mb-3" id="residentSelect" style="background: #0a0e27; color: white; border: 1px solid #2d3748; padding: 12px; border-radius: 8px;">
                                <option value="">Loading...</option>
                            </select>
                        </div>

                        <!-- PANEL 2: ANY PERSON -->
                        <div id="panelExternal" style="display:none;">
                            <div class="alert" style="background: rgba(245, 158, 11, 0.1); border: 1px solid #f59e0b; border-radius: 10px; padding: 15px; margin-bottom: 20px;">
                                <i class="fas fa-exclamation-triangle"></i> Assign anyone as manager (they don't need to be a resident)
                            </div>
                            
                            <div class="mb-3">
                                <label class="form-label fw-bold" style="color: white;">
                                    <i class="fas fa-user"></i> Full Name <span style="color: #ef4444;">*</span>
                                </label>
                                <input type="text" class="form-control" id="extName" placeholder="Enter full name" style="background: #0a0e27; color: white; border: 1px solid #2d3748; padding: 12px; border-radius: 8px;">
                            </div>

                            <div class="mb-3">
                                <label class="form-label fw-bold" style="color: white;">
                                    <i class="fas fa-phone"></i> Phone Number <span style="color: #ef4444;">*</span>
                                </label>
                                <input type="tel" class="form-control" id="extPhone" placeholder="10-digit mobile number" maxlength="10" style="background: #0a0e27; color: white; border: 1px solid #2d3748; padding: 12px; border-radius: 8px;">
                            </div>

                            <div class="mb-3">
                                <label class="form-label fw-bold" style="color: white;">
                                    <i class="fas fa-envelope"></i> Email <span style="color: #a0aec0;">(optional)</span>
                                </label>
                                <input type="email" class="form-control" id="extEmail" placeholder="Email address" style="background: #0a0e27; color: white; border: 1px solid #2d3748; padding: 12px; border-radius: 8px;">
                            </div>
                        </div>

                        <div id="errorMsg" class="alert alert-danger d-none mt-3"></div>
                        <div id="successMsg" class="alert alert-success d-none mt-3"></div>
                    </div>

                    <div class="modal-footer" style="border-top: 1px solid #2d3748;">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                            <i class="fas fa-times"></i> Cancel
                        </button>
                        <button type="button" class="btn btn-primary" id="btnSubmit" onclick="submitManager()">
                            <i class="fas fa-check"></i> Assign Manager
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;

    const old = document.getElementById('assignManagerModal');
    if (old) old.remove();

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    loadResidents(apartmentId);

    const modal = new bootstrap.Modal(document.getElementById('assignManagerModal'));
    modal.show();
}

function switchTab(tab) {
    const panelResident = document.getElementById('panelResident');
    const panelExternal = document.getElementById('panelExternal');
    const btnResident = document.getElementById('btnTabResident');
    const btnExternal = document.getElementById('btnTabExternal');

    hideMessages();

    if (tab === 'resident') {
        panelResident.style.display = 'block';
        panelExternal.style.display = 'none';
        btnResident.classList.add('active');
        btnExternal.classList.remove('active');
    } else {
        panelResident.style.display = 'none';
        panelExternal.style.display = 'block';
        btnResident.classList.remove('active');
        btnExternal.classList.add('active');
    }
}

async function loadResidents(apartmentId) {
    const select = document.getElementById('residentSelect');

    try {
        const response = await fetch(`/api/Manager/apartment-residents/${apartmentId}`);
        const data = await response.json();

        if (data && data.success && data.data && data.data.length > 0) {
            select.innerHTML = '<option value="">-- Select Resident --</option>';

            data.data.forEach(r => {
                const option = document.createElement('option');
                option.value = r.userId;
                option.textContent = `${r.fullName} - Flat ${r.flatNumber}`;
                select.appendChild(option);
            });
        } else {
            select.innerHTML = '<option value="">No residents found</option>';
            showError('No resident owners found. Use "Any Person" tab instead.');
        }
    } catch (err) {
        console.error(err);
        select.innerHTML = '<option value="">Error loading residents</option>';
        showError('Failed to load residents. Use "Any Person" tab.');
    }
}

async function submitManager() {
    const apartmentId = getApartmentIdFromUrl();
    const isResident = document.getElementById('btnTabResident').classList.contains('active');

    hideMessages();

    const submitBtn = document.getElementById('btnSubmit');
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Assigning...';

    let payload = { apartmentId: apartmentId };

    try {
        if (isResident) {
            const userId = document.getElementById('residentSelect').value;
            if (!userId) {
                showError('Please select a resident');
                return;
            }
            payload.userId = userId;
            payload.isExternalManager = false;
        } else {
            const name = document.getElementById('extName').value.trim();
            const phone = document.getElementById('extPhone').value.trim();
            const email = document.getElementById('extEmail').value.trim();

            if (!name) {
                showError('Name is required');
                return;
            }
            if (!phone || phone.length !== 10 || !/^\d+$/.test(phone)) {
                showError('Please enter valid 10-digit phone number');
                return;
            }

            payload.isExternalManager = true;
            payload.externalManagerName = name;
            payload.externalManagerPhone = phone;
            payload.externalManagerEmail = email || null;
        }

        const response = await fetch('/ApartmentBuilder/AssignManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {
            showSuccess('✅ Manager assigned successfully!');
            setTimeout(() => {
                bootstrap.Modal.getInstance(document.getElementById('assignManagerModal')).hide();
                location.reload();
            }, 1500);
        } else {
            showError(result.message || 'Failed to assign manager');
        }
    } catch (err) {
        console.error(err);
        showError('An error occurred. Please try again.');
    } finally {
        submitBtn.disabled = false;
        submitBtn.innerHTML = '<i class="fas fa-check"></i> Assign Manager';
    }
}

async function removeManager() {
    if (!confirm('⚠️ Remove current manager?')) return;

    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch('/ApartmentBuilder/RemoveManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ apartmentId: apartmentId })
        });

        const result = await response.json();

        if (result.success) {
            alert('✅ Manager removed!');
            location.reload();
        } else {
            alert('❌ ' + (result.message || 'Failed'));
        }
    } catch (err) {
        console.error(err);
        alert('❌ Error removing manager');
    }
}

function showError(msg) {
    const el = document.getElementById('errorMsg');
    const success = document.getElementById('successMsg');
    if (success) success.classList.add('d-none');
    if (el) {
        el.textContent = msg;
        el.classList.remove('d-none');
    }
}

function showSuccess(msg) {
    const el = document.getElementById('successMsg');
    const error = document.getElementById('errorMsg');
    if (error) error.classList.add('d-none');
    if (el) {
        el.textContent = msg;
        el.classList.remove('d-none');
    }
}

function hideMessages() {
    const error = document.getElementById('errorMsg');
    const success = document.getElementById('successMsg');
    if (error) error.classList.add('d-none');
    if (success) success.classList.add('d-none');
}

function getApartmentIdFromUrl() {
    const parts = window.location.pathname.split('/');
    return parts[parts.length - 1];
}

// Add CSS for tabs
const style = document.createElement('style');
style.textContent = `
    .btn-tab {
        flex: 1;
        border: none;
        border-radius: 8px;
        padding: 12px;
        background: transparent;
        color: #a0aec0;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.3s;
    }
    .btn-tab.active {
        background: linear-gradient(135deg, #667eea, #764ba2);
        color: white;
    }
    .btn-tab:hover {
        background: rgba(102, 126, 234, 0.2);
    }
`;
document.head.appendChild(style);






/*
function assignManager() {
    const apartmentId = getApartmentIdFromUrl();

    const modalHtml = `
        <div class="modal fade" id="assignManagerModal" tabindex="-1">
            <div class="modal-dialog modal-md">
                <div class="modal-content bg-dark text-white">

                    <!-- HEADER -->
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title">
                            <i class="fas fa-user-tie"></i> Assign Building Manager
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>

                    <!-- BODY -->
                    <div class="modal-body">

                        <!-- TAB SWITCHER -->
                        <div class="btn-group w-100 mb-4" role="group">
                            <button type="button" class="btn btn-outline-light active" id="btnTabResident"
                                    onclick="switchManagerTab('resident')">
                                <i class="fas fa-home"></i> Resident Manager
                            </button>
                            <button type="button" class="btn btn-outline-light" id="btnTabExternal"
                                    onclick="switchManagerTab('external')">
                                <i class="fas fa-user-plus"></i> External Manager
                            </button>
                        </div>

                        <!-- TAB A: RESIDENT MANAGER -->
                        <div id="panelResident">
                            <p class="text-muted small mb-3">
                                Select a resident owner from <strong>this apartment</strong> who already has the Manager role.
                            </p>
                            <div class="mb-3">
                                <label class="form-label">Select Resident</label>
                                <select class="form-select bg-secondary text-white border-secondary" id="residentManagerSelect">
                                    <option value="">-- Loading... --</option>
                                </select>
                            </div>
                        </div>

                        <!-- TAB B: EXTERNAL MANAGER -->
                        <div id="panelExternal" style="display:none;">
                            <p class="text-muted small mb-3">
                                Enter details for a manager who is <strong>not</strong> currently a resident of this apartment.
                            </p>
                            <div class="mb-3">
                                <label class="form-label">Manager Name <span class="text-danger">*</span></label>
                                <input type="text" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerName" placeholder="e.g. Ravi Kumar">
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Mobile Number <span class="text-danger">*</span></label>
                                <input type="tel" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerPhone" placeholder="e.g. 9876543210">
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Email <span class="text-muted">(optional)</span></label>
                                <input type="email" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerEmail" placeholder="e.g. ravi@email.com">
                            </div>
                            <div class="form-check mt-3">
                                <input class="form-check-input" type="checkbox" id="chkLivesInApartment">
                                <label class="form-check-label text-muted" for="chkLivesInApartment">
                                    This manager lives in this apartment (optional)
                                </label>
                            </div>
                        </div>

                        <!-- ERROR BOX -->
                        <div id="assignManagerError" class="alert alert-danger d-none mt-3"></div>
                    </div>

                    <!-- FOOTER -->
                    <div class="modal-footer border-secondary">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="submitManagerAssignment()">
                            <i class="fas fa-check"></i> Assign Manager
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Remove any old modal first
    const old = document.getElementById('assignManagerModal');
    if (old) old.remove();

    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Pre-load the resident dropdown
    loadResidentManagers(apartmentId);

    const modal = new bootstrap.Modal(document.getElementById('assignManagerModal'));
    modal.show();
}

// ─── TAB SWITCHER ─────────────────────────────────────────────────────────

function switchManagerTab(tab) {
    const panelResident = document.getElementById('panelResident');
    const panelExternal = document.getElementById('panelExternal');
    const btnResident = document.getElementById('btnTabResident');
    const btnExternal = document.getElementById('btnTabExternal');
    const errorDiv = document.getElementById('assignManagerError');

    // Hide error on switch
    if (errorDiv) { errorDiv.classList.add('d-none'); errorDiv.textContent = ''; }

    if (tab === 'resident') {
        panelResident.style.display = 'block';
        panelExternal.style.display = 'none';
        btnResident.classList.add('active');
        btnExternal.classList.remove('active');
    } else {
        panelResident.style.display = 'none';
        panelExternal.style.display = 'block';
        btnResident.classList.remove('active');
        btnExternal.classList.add('active');
    }
}

// ─── LOAD RESIDENT MANAGERS (dropdown) ────────────────────────────────────

async function loadResidentManagers(apartmentId) {
    const select = document.getElementById('residentManagerSelect');
    try {
        const response = await fetch(`/api/Manager/resident-managers/${apartmentId}`);
        const data = await response.json();

        if (data && data.success && data.data && data.data.length > 0) {
            select.innerHTML = '<option value="">-- Select Resident Manager --</option>';
            data.data.forEach(m => {
                const extra = m.isCurrentlyAssigned
                    ? ` (currently managing: ${m.currentApartmentName})`
                    : '';
                select.innerHTML += `<option value="${m.userId}">${m.fullName} – ${m.phone}${extra}</option>`;
            });
        } else {
            select.innerHTML = '<option value="" disabled>No resident managers available</option>';
        }
    } catch (err) {
        console.error('Error loading resident managers:', err);
        select.innerHTML = '<option value="" disabled>Error loading – try External tab</option>';
    }
}

// ─── SUBMIT MANAGER ASSIGNMENT ────────────────────────────────────────────

async function submitManagerAssignment() {
    const apartmentId = getApartmentIdFromUrl();
    const isResident = document.getElementById('btnTabResident').classList.contains('active');

    let payload = { apartmentId: apartmentId };

    if (isResident) {
        // ── Tab A: Resident Manager ──
        const userId = document.getElementById('residentManagerSelect').value;
        if (!userId) {
            showModalError('Please select a resident manager from the dropdown.');
            return;
        }
        payload.userId = userId;
        payload.isExternalManager = false;
        payload.livesInApartment = true;
    } else {
        // ── Tab B: External Manager ──
        const name = document.getElementById('extManagerName').value.trim();
        const phone = document.getElementById('extManagerPhone').value.trim();
        const email = document.getElementById('extManagerEmail').value.trim();
        const lives = document.getElementById('chkLivesInApartment').checked;

        if (!name) { showModalError('Manager Name is required.'); return; }
        if (!phone) { showModalError('Mobile Number is required.'); return; }

        payload.isExternalManager = true;
        payload.externalManagerName = name;
        payload.externalManagerPhone = phone;
        payload.externalManagerEmail = email || null;
        payload.livesInApartment = lives;
    }

    try {
        const response = await fetch('/ApartmentBuilder/AssignManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {
            bootstrap.Modal.getInstance(document.getElementById('assignManagerModal')).hide();
            alert('Manager assigned successfully!');
            location.reload();
        } else {
            showModalError(result.message || 'Failed to assign manager.');
        }
    } catch (err) {
        console.error('Error assigning manager:', err);
        showModalError('An error occurred. Please try again.');
    }
}

function showModalError(msg) {
    const errorDiv = document.getElementById('assignManagerError');
    errorDiv.textContent = msg;
    errorDiv.classList.remove('d-none');
}

// ─── REMOVE MANAGER ──────────────────────────────────────────────────────

async function removeManager() {
    if (!confirm('Are you sure you want to remove the current manager?')) return;

    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch('/ApartmentBuilder/RemoveManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ apartmentId: apartmentId })
        });

        const result = await response.json();
        if (result.success) {
            alert('Manager removed successfully!');
            location.reload();
        } else {
            alert(result.message || 'Failed to remove manager');
        }
    } catch (err) {
        console.error('Error removing manager:', err);
        alert('An error occurred while removing manager');
    }
}

// ─── COMMUNITY ROLE ASSIGNMENT ───────────────────────────────────────────

function assignRole(role) {
    const apartmentId = getApartmentIdFromUrl();
    window.location.href = `/Community/AssignRole?apartmentId=${apartmentId}&role=${role}`;
}

// ─── HELPER: Extract apartment ID from URL ───────────────────────────────
// URL pattern: /ApartmentBuilder/Details/{guid}

function getApartmentIdFromUrl() {
    const parts = window.location.pathname.split('/');
    return parts[parts.length - 1];
}

// ─── QUICK ACTIONS LINKS ──────────────────────────────────────────────────

function setupQuickActions() {
    const apartmentId = getApartmentIdFromUrl();
    const actions = [
        `/ResidentManagement/Index?apartmentId=${apartmentId}`,
        `/Staff/Index?apartmentId=${apartmentId}`,
        `/Finance/Index?apartmentId=${apartmentId}`,
        `/Event/Index?apartmentId=${apartmentId}`
    ];

    document.querySelectorAll('.action-card').forEach((card, i) => {
        if (actions[i]) card.href = actions[i];
    });
}

document.addEventListener('DOMContentLoaded', function () {
    setupQuickActions();
});


*/










/*
function assignManager() {
    const apartmentId = getApartmentIdFromUrl();

    const modalHtml = `
        <div class="modal fade" id="assignManagerModal" tabindex="-1">
            <div class="modal-dialog modal-md">
                <div class="modal-content bg-dark text-white">

                    <!-- HEADER -->
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title">
                            <i class="fas fa-user-tie"></i> Assign Building Manager
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>

                    <!-- BODY -->
                    <div class="modal-body">

                        <!-- TAB SWITCHER -->
                        <div class="btn-group w-100 mb-4" role="group">
                            <button type="button" class="btn btn-outline-light active" id="btnTabResident"
                                    onclick="switchManagerTab('resident')">
                                <i class="fas fa-home"></i> Resident Manager
                            </button>
                            <button type="button" class="btn btn-outline-light" id="btnTabExternal"
                                    onclick="switchManagerTab('external')">
                                <i class="fas fa-user-plus"></i> External Manager
                            </button>
                        </div>

                        <!-- ════════════════════════════════════════ -->
                        <!-- TAB A: RESIDENT MANAGER (from this apt) -->
                        <!-- ════════════════════════════════════════ -->
                        <div id="panelResident">
                            <p class="text-muted small mb-3">
                                Select a resident owner from <strong>this apartment</strong> who already has the Manager role.
                            </p>
                            <div class="mb-3">
                                <label class="form-label">Select Resident</label>
                                <select class="form-select bg-secondary text-white border-secondary" id="residentManagerSelect">
                                    <option value="">-- Loading... --</option>
                                </select>
                            </div>
                        </div>

                        <!-- ════════════════════════════════════════ -->
                        <!-- TAB B: EXTERNAL MANAGER (not a resident)-->
                        <!-- ════════════════════════════════════════ -->
                        <div id="panelExternal" style="display:none;">
                            <p class="text-muted small mb-3">
                                Enter details for a manager who is <strong>not</strong> currently a resident of this apartment.
                            </p>
                            <div class="mb-3">
                                <label class="form-label">Manager Name <span class="text-danger">*</span></label>
                                <input type="text" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerName" placeholder="e.g. Ravi Kumar">
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Mobile Number <span class="text-danger">*</span></label>
                                <input type="tel" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerPhone" placeholder="e.g. 9876543210">
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Email <span class="text-muted">(optional)</span></label>
                                <input type="email" class="form-control bg-secondary text-white border-secondary"
                                       id="extManagerEmail" placeholder="e.g. ravi@email.com">
                            </div>
                            <!-- ⭐ Optional checkbox: does the manager live here? -->
                            <div class="form-check mt-3">
                                <input class="form-check-input" type="checkbox" id="chkLivesInApartment">
                                <label class="form-check-label text-muted" for="chkLivesInApartment">
                                    This manager lives in this apartment (optional)
                                </label>
                            </div>
                        </div>

                        <!-- ERROR BOX -->
                        <div id="assignManagerError" class="alert alert-danger d-none mt-3"></div>
                    </div>

                    <!-- FOOTER -->
                    <div class="modal-footer border-secondary">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="submitManagerAssignment()">
                            <i class="fas fa-check"></i> Assign Manager
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;

    // Remove any old modal
    const old = document.getElementById('assignManagerModal');
    if (old) old.remove();

    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Pre-load the resident dropdown immediately
    loadResidentManagers(apartmentId);

    const modal = new bootstrap.Modal(document.getElementById('assignManagerModal'));
    modal.show();
}

// ─── TAB SWITCHER ─────────────────────────────────────────────────────────

function switchManagerTab(tab) {
    const panelResident = document.getElementById('panelResident');
    const panelExternal = document.getElementById('panelExternal');
    const btnResident = document.getElementById('btnTabResident');
    const btnExternal = document.getElementById('btnTabExternal');
    const errorDiv = document.getElementById('assignManagerError');

    // Hide error on switch
    if (errorDiv) { errorDiv.classList.add('d-none'); errorDiv.textContent = ''; }

    if (tab === 'resident') {
        panelResident.style.display = 'block';
        panelExternal.style.display = 'none';
        btnResident.classList.add('active');
        btnExternal.classList.remove('active');
    } else {
        panelResident.style.display = 'none';
        panelExternal.style.display = 'block';
        btnResident.classList.remove('active');
        btnExternal.classList.add('active');
    }
}

// ─── LOAD RESIDENT MANAGERS (dropdown) ────────────────────────────────────

async function loadResidentManagers(apartmentId) {
    const select = document.getElementById('residentManagerSelect');
    try {
        // ⭐ Correct URL: /api/Manager/resident-managers/{id}
        const response = await fetch(`/api/Manager/resident-managers/${apartmentId}`);
        const data = await response.json();

        if (data && data.success && data.data && data.data.length > 0) {
            select.innerHTML = '<option value="">-- Select Resident Manager --</option>';
            data.data.forEach(m => {
                const extra = m.isCurrentlyAssigned
                    ? ` (currently managing: ${m.currentApartmentName})`
                    : '';
                select.innerHTML += `<option value="${m.userId}">${m.fullName} – ${m.phone}${extra}</option>`;
            });
        } else {
            select.innerHTML = '<option value="" disabled>No resident managers available</option>';
        }
    } catch (err) {
        console.error('Error loading resident managers:', err);
        select.innerHTML = '<option value="" disabled>Error loading – try External tab</option>';
    }
}

// ─── SUBMIT ───────────────────────────────────────────────────────────────

async function submitManagerAssignment() {
    const apartmentId = getApartmentIdFromUrl();
    const errorDiv = document.getElementById('assignManagerError');
    const isResident = document.getElementById('btnTabResident').classList.contains('active');

    let payload = { apartmentId: apartmentId };

    if (isResident) {
        // ── Tab A: Resident Manager ──
        const userId = document.getElementById('residentManagerSelect').value;
        if (!userId) {
            showModalError('Please select a resident manager from the dropdown.');
            return;
        }
        payload.userId = userId;
        payload.isExternalManager = false;
        payload.livesInApartment = true; // resident already lives here
    } else {
        // ── Tab B: External Manager ──
        const name = document.getElementById('extManagerName').value.trim();
        const phone = document.getElementById('extManagerPhone').value.trim();
        const email = document.getElementById('extManagerEmail').value.trim();
        const lives = document.getElementById('chkLivesInApartment').checked;

        if (!name) { showModalError('Manager Name is required.'); return; }
        if (!phone) { showModalError('Mobile Number is required.'); return; }

        payload.isExternalManager = true;
        payload.externalManagerName = name;
        payload.externalManagerPhone = phone;
        payload.externalManagerEmail = email || null;
        payload.livesInApartment = lives;
    }

    try {
        const response = await fetch('/ApartmentBuilder/AssignManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {
            bootstrap.Modal.getInstance(document.getElementById('assignManagerModal')).hide();
            alert('Manager assigned successfully!');
            location.reload();
        } else {
            showModalError(result.message || 'Failed to assign manager.');
        }
    } catch (err) {
        console.error('Error assigning manager:', err);
        showModalError('An error occurred. Please try again.');
    }
}

function showModalError(msg) {
    const errorDiv = document.getElementById('assignManagerError');
    errorDiv.textContent = msg;
    errorDiv.classList.remove('d-none');
}

// ─── REMOVE MANAGER ──────────────────────────────────────────────────────

async function removeManager() {
    if (!confirm('Are you sure you want to remove the current manager?')) return;

    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch('/ApartmentBuilder/RemoveManager', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ apartmentId: apartmentId })
        });

        const result = await response.json();
        if (result.success) {
            alert('Manager removed successfully!');
            location.reload();
        } else {
            alert(result.message || 'Failed to remove manager');
        }
    } catch (err) {
        console.error('Error removing manager:', err);
        alert('An error occurred while removing manager');
    }
}

// ─── COMMUNITY ROLE ASSIGNMENT ───────────────────────────────────────────

function assignRole(role) {
    const apartmentId = getApartmentIdFromUrl();
    window.location.href = `/Community/AssignRole?apartmentId=${apartmentId}&role=${role}`;
}

// ─── HELPER: Extract apartment ID from URL ───────────────────────────────
// URL pattern: /ApartmentBuilder/Details/{guid}

function getApartmentIdFromUrl() {
    const parts = window.location.pathname.split('/');
    return parts[parts.length - 1];
}

// ─── QUICK ACTIONS LINKS ──────────────────────────────────────────────────

function setupQuickActions() {
    const apartmentId = getApartmentIdFromUrl();
    const actions = [
        `/ResidentManagement/Index?apartmentId=${apartmentId}`,
        `/Staff/Index?apartmentId=${apartmentId}`,
        `/Finance/Index?apartmentId=${apartmentId}`,
        `/Event/Index?apartmentId=${apartmentId}`
    ];

    document.querySelectorAll('.action-card').forEach((card, i) => {
        if (actions[i]) card.href = actions[i];
    });
}

document.addEventListener('DOMContentLoaded', function () {
    setupQuickActions();
});



*/