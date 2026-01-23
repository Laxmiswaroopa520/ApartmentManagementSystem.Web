using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Admin;

namespace ApartmentManagementSystem.Web.Mappers.Admin
{
    public static class FloorDropdownMapper
    {
        public static List<FloorDropdownViewModel> From(List<FloorDto> dto)
        {
            return dto.Select(f => new FloorDropdownViewModel
            {
                Id = f.Id,
                FloorNumber = f.FloorNumber
            }).ToList();
        }
    }
    }
