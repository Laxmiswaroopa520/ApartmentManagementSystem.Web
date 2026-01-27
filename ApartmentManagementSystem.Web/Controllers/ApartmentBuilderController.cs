/*
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services; // ⭐ Add this
using System.Text.Json;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ApartmentBuilderController : Controller
    {
        private readonly ApiClient _apiClient; // ⭐ Changed to ApiClient
        private readonly IConfiguration _configuration;

        public ApartmentBuilderController(
            ApiClient apiClient, // ⭐ Changed from IHttpClientFactory
            IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> ManageApartments()
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<List<ApartmentListViewModel>>>(
                    "api/ApartmentManagement/all"
                );

                return View(response?.Data ?? new List<ApartmentListViewModel>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading apartments: {ex.Message}");
                return View(new List<ApartmentListViewModel>());
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<ApartmentDetailViewModel>>(
                    $"api/ApartmentManagement/{id}"
                );

                if (response?.Data != null)
                {
                    return View(response.Data);
                }

                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading details: {ex.Message}");
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<ApartmentDiagramViewModel>>(
                    $"api/ApartmentManagement/{id}/diagram"
                );

                if (response?.Data != null)
                {
                    return View(response.Data);
                }

                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading diagram: {ex.Message}");
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                Console.WriteLine("=== CreateApartment Called ===");
                Console.WriteLine($"Model: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                // ⭐ Use ApiClient which is already configured
                var response = await _apiClient.PostAsync<CreateApartmentViewModel, ApiResponse<object>>(
                    "api/ApartmentManagement/create",
                    model
                );

                Console.WriteLine($"Response received: Success={response?.Success}");

                if (response?.Success == true)
                {
                    return Json(new { success = true, message = "Apartment created successfully!" });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to create apartment"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR in CreateApartment ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }
    }
}
*/

using ApartmentManagementSystem.Web.Mappers.Apartment;
using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Web.Services.DTOs.Manager;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ApartmentBuilderController : Controller
    {
        private readonly ApartmentApiService _apartmentApiService;
        private readonly ManagerApiService _managerApiService;

        public ApartmentBuilderController(ApartmentApiService apartmentApiService, ManagerApiService managerApiService)
        {
            _apartmentApiService = apartmentApiService;
            _managerApiService = managerApiService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageApartments()
        {
            try
            {
                var response = await _apartmentApiService.GetAllApartmentsAsync();

                var viewModel = response?.Success == true && response.Data != null
                    ? ApartmentListMapper.From(response.Data)
                    : new List<ApartmentListViewModel>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading apartments: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartments";
                return View(new List<ApartmentListViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await _apartmentApiService.GetApartmentDetailAsync(id);

                if (response?.Success == true && response.Data != null)
                {
                    var viewModel = ApartmentDetailMapper.From(response.Data);
                    return View(viewModel);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Apartment not found";
                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading details: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartment details";
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await _apartmentApiService.GetApartmentDiagramAsync(id);

                if (response?.Success == true && response.Data != null)
                {
                    var viewModel = ApartmentDiagramMapper.From(response.Data);
                    return View(viewModel);
                }

                TempData["ErrorMessage"] = response?.Message ?? "Diagram not found";
                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading diagram: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to load apartment diagram";
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid data provided"
                    });
                }

                Console.WriteLine("=== CreateApartment Called ===");
                Console.WriteLine($"Model: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                // Map ViewModel to DTO
                var dto = CreateApartmentMapper.ToDto(model);

                // Call API through service
                var response = await _apartmentApiService.CreateApartmentAsync(dto);

                Console.WriteLine($"Response received: Success={response?.Success}");

                if (response?.Success == true)
                {
                    return Json(new
                    {
                        success = true,
                        message = response.Message ?? "Apartment created successfully!",
                        data = response.Data
                    });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to create apartment"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR in CreateApartment ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided" });
                }

                // Call your API service to assign manager
                var response = await _managerApiService.AssignManagerToApartmentAsync(request);

                if (response?.Success == true)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Manager assigned successfully!"
                    });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to assign manager"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning manager: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }
        // this is for remove manager

        [HttpPost]
        public async Task<IActionResult> RemoveManager([FromBody] RemoveManagerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided" });
                }

                var response = await _managerApiService.RemoveManagerFromApartmentAsync(request);

                if (response?.Success == true)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Manager removed successfully!"
                    });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to remove manager"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing manager: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }
    }
}
















/*

using ApartmentManagementSystem.Web.Services;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ApartmentBuilderController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly IConfiguration _configuration;

        public ApartmentBuilderController(
            ApiClient apiClient,
            IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> ManageApartments()
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<List<ApartmentListViewModel>>>(
                    "api/ApartmentManagement/all"
                );

                return View(response?.Data ?? new List<ApartmentListViewModel>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading apartments: {ex.Message}");
                return View(new List<ApartmentListViewModel>());
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<ApartmentDetailViewModel>>(
                    $"api/ApartmentManagement/{id}"
                );

                if (response?.Data != null)
                {
                    return View(response.Data);
                }

                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading details: {ex.Message}");
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        public async Task<IActionResult> Visualize(Guid id)
        {
            try
            {
                var response = await _apiClient.GetAsync<ApiResponse<ApartmentDiagramViewModel>>(
                    $"api/ApartmentManagement/{id}/diagram"
                );

                if (response?.Data != null)
                {
                    return View(response.Data);
                }

                return RedirectToAction(nameof(ManageApartments));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading diagram: {ex.Message}");
                return RedirectToAction(nameof(ManageApartments));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                Console.WriteLine("=== CreateApartment Called ===");
                Console.WriteLine($"Model: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                // Map ViewModel to DTO
                var dto = new CreateApartmentDto
                {
                    Name = model.Name,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    PinCode = model.PinCode,
                    TotalFloors = model.TotalFloors,
                    FlatsPerFloor = model.FlatsPerFloor
                };

                // ⭐ Use the correct response type
                var response = await _apiClient.PostAsync<CreateApartmentDto, ApiResponse<CreateApartmentResponseDto>>(
                    "api/ApartmentManagement/create",
                    dto
                );

                Console.WriteLine($"Response received: Success={response?.Success}");

                if (response?.Success == true)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Apartment created successfully!",
                        data = response.Data
                    });
                }

                return Json(new
                {
                    success = false,
                    message = response?.Message ?? "Failed to create apartment"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR in CreateApartment ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }
    }
}

*/
















/*
using Microsoft.AspNetCore.Mvc;
// Web/Controllers/ApartmentBuilderController.cs
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
using ApartmentManagementSystem.Web.Services.DTOs;

namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ApartmentBuilderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApartmentBuilderController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> ManageApartments()
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync("api/ApartmentManagement/all");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apartments = JsonSerializer.Deserialize<ApiResponse<List<ApartmentListViewModel>>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(apartments?.Data ?? new List<ApartmentListViewModel>());
            }

            return View(new List<ApartmentListViewModel>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/ApartmentManagement/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<ApartmentDetailViewModel>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(result?.Data);
            }

            return RedirectToAction(nameof(ManageApartments));
        }

        public async Task<IActionResult> Visualize(Guid id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/ApartmentManagement/{id}/diagram");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<ApartmentDiagramViewModel>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(result?.Data);
            }

            return RedirectToAction(nameof(ManageApartments));
        }
        /// updated code for knowing the exact error
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            try
            {
                Console.WriteLine("=== CreateApartment Called ===");
                Console.WriteLine($"Model: Name={model.Name}, Floors={model.TotalFloors}, Flats={model.FlatsPerFloor}");

                var client = CreateHttpClient();

                Console.WriteLine($"API Base URL: {client.BaseAddress}");

                var json = JsonSerializer.Serialize(model);
                Console.WriteLine($"Sending JSON: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/ApartmentManagement/create", content);

                Console.WriteLine($"API Response Status: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, data = responseContent });
                }

                return Json(new { success = false, message = responseContent });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERROR in CreateApartment ===");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return Json(new
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                });
            }
        }

        /*  [HttpPost]
          public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
          {
              var client = CreateHttpClient();
              var json = JsonSerializer.Serialize(model);
              var content = new StringContent(json, Encoding.UTF8, "application/json");

              var response = await client.PostAsync("api/ApartmentManagement/create", content);
              var responseContent = await response.Content.ReadAsStringAsync();

              if (response.IsSuccessStatusCode)
              {
                  return Json(new { success = true, data = responseContent });
              }

              return Json(new { success = false, message = responseContent });
          }
          ---------------

// Web/Controllers/ApartmentBuilderController.cs

private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();

            // ⭐ FIX: Use correct API URL
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7093/";        //not in 7001
            client.BaseAddress = new Uri(apiBaseUrl);

            var token = HttpContext.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
      /*  private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7093";     //7001
            client.BaseAddress = new Uri(apiBaseUrl);

            var token = HttpContext.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }------------
    }
}



*/














/*

    // Web/Controllers/ApartmentBuilderController.cs
 using Microsoft.AspNetCore.Authorization;
 using System.Net.Http.Headers;
 using System.Text;
 using System.Text.Json;
using ApartmentManagementSystem.Web.Services.DTOs;
using ApartmentManagementSystem.Web.ViewModels.Apartment;
namespace ApartmentManagementSystem.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ApartmentBuilderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApartmentBuilderController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: ApartmentBuilder/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: ApartmentBuilder/ManageApartments
        public async Task<IActionResult> ManageApartments()
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync("api/ApartmentManagement/all");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apartments = JsonSerializer.Deserialize<ApiResponse<List<ApartmentListViewModel>>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(apartments?.Data ?? new List<ApartmentListViewModel>());
            }

            return View(new List<ApartmentListViewModel>());
        }

        // GET: ApartmentBuilder/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/ApartmentManagement/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<ApartmentDetailViewModel>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(result?.Data);
            }

            return RedirectToAction(nameof(ManageApartments));
        }

        // GET: ApartmentBuilder/Visualize/{id}
        public async Task<IActionResult> Visualize(Guid id)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync($"api/ApartmentManagement/{id}/diagram");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<ApartmentDiagramViewModel>>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                return View(result?.Data);
            }

            return RedirectToAction(nameof(ManageApartments));
        }

        // POST: Create Apartment (called from JavaScript)
        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentViewModel model)
        {
            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/ApartmentManagement/create", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, data = responseContent });
            }

            return Json(new { success = false, message = responseContent });
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001";
            client.BaseAddress = new Uri(apiBaseUrl);

            var token = HttpContext.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}*/