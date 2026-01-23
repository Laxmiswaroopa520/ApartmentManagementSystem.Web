using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;

namespace ApartmentManagementSystem.Web.Mappers.Community
{
    public static class ResidentDetailViewModelMapper
    {
        public static ResidentDetailViewModel From(ResidentDetailDto dto)
        {
            return new ResidentDetailViewModel
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                PrimaryPhone = dto.PrimaryPhone,
                SecondaryPhone = dto.SecondaryPhone,
                ResidentType = dto.ResidentType,
                FlatNumber = dto.FlatNumber,
                ApartmentName = dto.ApartmentName,
                RegisteredOn = dto.RegisteredOn,
                Status = dto.Status,
                Roles = dto.Roles,
                TotalComplaints = dto.TotalComplaints,
                OutstandingBills = dto.OutstandingBills
            };
        }
    }
}
