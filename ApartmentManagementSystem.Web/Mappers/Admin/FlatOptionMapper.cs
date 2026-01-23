using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Admin;

namespace ApartmentManagementSystem.Web.Mappers.Admin
{
    public static class FlatOptionMapper
    {
        public static List<FlatOption> From(List<FlatDto> dto)
        {
            return dto.Select(f => new FlatOption
            {
                Id = f.Id,
                FlatNumber = f.FlatNumber
            }).ToList();
        }
    }
}
