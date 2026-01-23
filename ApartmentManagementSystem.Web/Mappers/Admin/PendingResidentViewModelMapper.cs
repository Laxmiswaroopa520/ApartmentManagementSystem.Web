
using ApartmentManagementSystem.Web.Services.DTOs.Admin;
using ApartmentManagementSystem.Web.ViewModels.Admin;

namespace ApartmentManagementSystem.Web.Mappers.Admin
{
    public static class PendingResidentViewModelMapper
    {
        public static List<PendingResidentViewModel> From(List<PendingResidentDto> dto)
        {
            return dto.Select(r => new PendingResidentViewModel
            {
                UserId = r.UserId,
                FullName = r.FullName,
                PrimaryPhone = r.PrimaryPhone,
                Email = r.Email,
                ResidentType = r.ResidentType,
                RegisteredOn = r.RegisteredOn,
                Status = r.Status
            }).ToList();
        }
    }
}

