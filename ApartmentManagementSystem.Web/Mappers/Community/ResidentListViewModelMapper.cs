
using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;

namespace ApartmentManagementSystem.Web.Mappers.Community
{
    public static class ResidentListViewModelMapper
    {
        public static List<ResidentListViewModel> From(
            IEnumerable<ResidentListDto> dto)
        {
            return dto.Select(r => new ResidentListViewModel
            {
                UserId = r.UserId,
                FullName = r.FullName,
                Email = r.Email,
                Phone = r.Phone,
                ResidentType = r.ResidentType,
                FlatNumber = r.FlatNumber,
                Status = r.Status,
                RegisteredOn = r.RegisteredOn
            }).ToList();
        }
    }
}
