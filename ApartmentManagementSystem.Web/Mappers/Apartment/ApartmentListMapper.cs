using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Apartment;

namespace ApartmentManagementSystem.Web.Mappers.Apartment
{
    public static class ApartmentListMapper
    {
        public static List<ApartmentListViewModel> From(List<ApartmentListDto> dtos)
        {
            return dtos.Select(dto => new ApartmentListViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                TotalFloors = dto.TotalFloors,
                TotalFlats = dto.TotalFlats,
                OccupiedFlats = dto.OccupiedFlats,
                Status = dto.Status,
                IsActive = dto.IsActive
            }).ToList();
        }
    }
}