// apartment-details.js - Add this to your Scripts section

// Manager Assignment Modal
function assignManager() {
    // Create modal HTML
    const modalHtml = `
        <div class="modal fade" id="assignManagerModal" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-secondary">
                        <h5 class="modal-title">
                            <i class="fas fa-user-tie"></i> Assign Building Manager
                        </h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <form id="assignManagerForm">
                            <div class="mb-3">
                                <label class="form-label">Select Manager</label>
                                <select class="form-select" id="managerUserId" required>
                                    <option value="">-- Loading Managers --</option>
                                </select>
                                <small class="text-muted">Only users with Manager role can be assigned</small>
                            </div>
                            <div id="assignManagerError" class="alert alert-danger d-none"></div>
                        </form>
                    </div>
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

    // Remove existing modal if any
    const existingModal = document.getElementById('assignManagerModal');
    if (existingModal) {
        existingModal.remove();
    }

    // Add modal to page
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Load available managers
    loadAvailableManagers();

    // Show modal using Bootstrap
    const modal = new bootstrap.Modal(document.getElementById('assignManagerModal'));
    modal.show();
}

async function loadAvailableManagers() {
    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch(`/api/Manager/available-managers/${apartmentId}`);
        const data = await response.json();

        const select = document.getElementById('managerUserId');

        if (data.success && data.data && data.data.length > 0) {
            select.innerHTML = '<option value="">-- Select Manager --</option>';
            data.data.forEach(manager => {
                select.innerHTML += `<option value="${manager.userId}">${manager.fullName} - ${manager.email}</option>`;
            });
        } else {
            select.innerHTML = '<option value="">No available managers found</option>';
        }
    } catch (error) {
        console.error('Error loading managers:', error);
        document.getElementById('managerUserId').innerHTML =
            '<option value="">Error loading managers</option>';
    }
}

async function submitManagerAssignment() {
    const userId = document.getElementById('managerUserId').value;
    const errorDiv = document.getElementById('assignManagerError');

    if (!userId) {
        errorDiv.textContent = 'Please select a manager';
        errorDiv.classList.remove('d-none');
        return;
    }

    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch('/ApartmentBuilder/AssignManager', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                apartmentId: apartmentId,
                userId: userId
            })
        });

        const result = await response.json();

        if (result.success) {
            // Close modal
            bootstrap.Modal.getInstance(document.getElementById('assignManagerModal')).hide();

            // Show success message and reload page
            alert('Manager assigned successfully!');
            location.reload();
        } else {
            errorDiv.textContent = result.message || 'Failed to assign manager';
            errorDiv.classList.remove('d-none');
        }
    } catch (error) {
        console.error('Error assigning manager:', error);
        errorDiv.textContent = 'An error occurred while assigning manager';
        errorDiv.classList.remove('d-none');
    }
}

async function removeManager() {
    if (!confirm('Are you sure you want to remove the current manager?')) {
        return;
    }

    try {
        const apartmentId = getApartmentIdFromUrl();
        const response = await fetch('/ApartmentBuilder/RemoveManager', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                apartmentId: apartmentId
            })
        });

        const result = await response.json();

        if (result.success) {
            alert('Manager removed successfully!');
            location.reload();
        } else {
            alert(result.message || 'Failed to remove manager');
        }
    } catch (error) {
        console.error('Error removing manager:', error);
        alert('An error occurred while removing manager');
    }
}

// Community Role Assignment
function assignRole(role) {
    const apartmentId = getApartmentIdFromUrl();
    // Redirect to community role assignment page with apartment context
    window.location.href = `/Community/AssignRole?apartmentId=${apartmentId}&role=${role}`;
}

// Helper function to get apartment ID from URL
function getApartmentIdFromUrl() {
    const pathParts = window.location.pathname.split('/');
    return pathParts[pathParts.length - 1];
}

// Quick Actions Navigation
function setupQuickActions() {
    const apartmentId = getApartmentIdFromUrl();

    document.querySelectorAll('.action-card').forEach((card, index) => {
        const actions = [
            `/Resident/Index?apartmentId=${apartmentId}`,
            `/Staff/Index?apartmentId=${apartmentId}`,
            `/Finance/Index?apartmentId=${apartmentId}`,
            `/Event/Index?apartmentId=${apartmentId}`
        ];

        if (actions[index]) {
            card.href = actions[index];
        }
    });
}

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function () {
    setupQuickActions();
});