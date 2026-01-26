using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Admin;

namespace ApartmentManagementSystem.Web.Mappers.Admin
{
    public static class ApartmentDropdownMapper
    {
        public static List<ApartmentDropdownViewModel> From(List<ApartmentDropdownDto> dtos)
        {
            return dtos.Select(dto => new ApartmentDropdownViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            }).ToList();
        }
    }
}