using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Apartment;

namespace ApartmentManagementSystem.Web.Mappers.Apartment
{
    public static class ApartmentDetailMapper
    {
        public static ApartmentDetailViewModel From(ApartmentDetailDto dto)
        {
            return new ApartmentDetailViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                PinCode = dto.PinCode,
                TotalFloors = dto.TotalFloors,
                FlatsPerFloor = dto.FlatsPerFloor,
                TotalFlats = dto.TotalFlats,
                OccupiedFlats = dto.OccupiedFlats,
                VacantFlats = dto.VacantFlats,
                Status = dto.Status,
                IsActive = dto.IsActive,
                Manager = dto.Manager != null ? MapManagerInfo(dto.Manager) : null,
                President = dto.President != null ? MapCommunityLeader(dto.President) : null,
                Secretary = dto.Secretary != null ? MapCommunityLeader(dto.Secretary) : null,
                Treasurer = dto.Treasurer != null ? MapCommunityLeader(dto.Treasurer) : null,
                CreatedAt = dto.CreatedAt
            };
        }

        private static ManagerInfoViewModel MapManagerInfo(ManagerInfoDto dto)
        {
            return new ManagerInfoViewModel
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                AssignedAt = dto.AssignedAt
            };
        }

        private static CommunityLeaderViewModel MapCommunityLeader(CommunityLeaderDto dto)
        {
            return new CommunityLeaderViewModel
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                FlatNumber = dto.FlatNumber,
                AssignedAt = dto.AssignedAt
            };
        }
    }
}