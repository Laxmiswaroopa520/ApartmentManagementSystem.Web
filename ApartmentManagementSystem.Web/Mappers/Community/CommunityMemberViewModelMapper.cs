namespace ApartmentManagementSystem.Web.Mappers.Community
{
    using ApartmentManagementSystem.Web.Services.DTOs.Community;
    using ApartmentManagementSystem.Web.ViewModels.Community;
    public static class CommunityMemberViewModelMapper
    {
        public static List<CommunityMemberViewModel> From(IEnumerable<CommunityMemberDto> dto)
        {
            return dto.Select(m => new CommunityMemberViewModel
            {
                UserId = m.UserId,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone,
                FlatNumber = m.FlatNumber,
                Role = m.Role,
                AssignedOn = m.AssignedOn,
                IsActive = m.IsActive
            }).ToList();
        }
    }
}