// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// ============================================
// ApartmentManagementSystem.Web/wwwroot/js/site.js
// ============================================

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    // Auto-dismiss success and info alerts
    const alerts = document.querySelectorAll('.alert-success, .alert-info');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000); // 5 seconds
    });
});

// Phone number formatting
function formatPhoneNumber(input) {
    // Remove all non-numeric characters
    let value = input.value.replace(/\D/g, '');

    // Limit to 10 digits
    if (value.length > 10) {
        value = value.slice(0, 10);
    }

    input.value = value;
}

// OTP input formatting
function formatOtpInput(input) {
    // Remove all non-numeric characters
    let value = input.value.replace(/\D/g, '');

    // Limit to 6 digits
    if (value.length > 6) {
        value = value.slice(0, 6);
    }

    input.value = value;
}

// Username validation (letters, numbers, underscores only)
function validateUsername(input) {
    let value = input.value.replace(/[^a-zA-Z0-9_]/g, '');
    input.value = value;
}

// Real-time password strength indicator (for future enhancement)
function checkPasswordStrength(password) {
    let strength = 0;

    if (password.length >= 8) strength++;
    if (password.match(/[a-z]+/)) strength++;
    if (password.match(/[A-Z]+/)) strength++;
    if (password.match(/[0-9]+/)) strength++;
    if (password.match(/[$@#&!]+/)) strength++;

    return strength;
}

// Form validation helper
function showValidationError(inputElement, message) {
    const feedbackElement = inputElement.nextElementSibling;
    if (feedbackElement && feedbackElement.classList.contains('text-danger')) {
        feedbackElement.textContent = message;
    }
}

// Confirm navigation away from unsaved form (for future use)
function confirmFormExit(formElement) {
    let formChanged = false;

    formElement.addEventListener('change', function () {
        formChanged = true;
    });

    window.addEventListener('beforeunload', function (e) {
        if (formChanged) {
            e.preventDefault();
            e.returnValue = '';
        }
    });

    formElement.addEventListener('submit', function () {
        formChanged = false;
    });
}

// Loading spinner utility (for AJAX calls in future)
function showLoadingSpinner(button) {
    const originalText = button.innerHTML;
    button.disabled = true;
    button.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...';

    return function hideSpinner() {
        button.disabled = false;
        button.innerHTML = originalText;
    };
}

// Copy to clipboard utility (for OTP sharing in future)
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        // Show success message
        alert('Copied to clipboard!');
    }, function (err) {
        console.error('Could not copy text: ', err);
    });
}

// Debounce utility (for search/filter in future)
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Export functions for use in other scripts
window.ApartmentApp = {
    formatPhoneNumber: formatPhoneNumber,
    formatOtpInput: formatOtpInput,
    validateUsername: validateUsername,
    checkPasswordStrength: checkPasswordStrength,
    showLoadingSpinner: showLoadingSpinner,
    copyToClipboard: copyToClipboard,
    debounce: debounce
};

// Console greeting (optional, can remove in production)
console.log('%c🏢 Apartment Management System', 'color: #0d6efd; font-size: 20px; font-weight: bold;');
console.log('%cPhase 0 & 1 - Authentication & Onboarding', 'color: #28a745; font-size: 14px;');
console.log('%cDeveloped with Clean Architecture & API-First Design', 'color: #666; font-size: 12px;');