using ApartmentManagementSystem.Web.Services.DTOs.Community;
using ApartmentManagementSystem.Web.ViewModels.Community;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Community;

public static class EligibleResidentViewModelMapper
{
    public static List<EligibleResidentViewModel> From(IEnumerable<ResidentListDto> dto)
    {
        return dto.Select(r => new EligibleResidentViewModel
        {
            UserId = r.UserId,
            DisplayText = $"{r.FullName} - Flat {r.FlatNumber}"
        }).ToList();
    }
}
