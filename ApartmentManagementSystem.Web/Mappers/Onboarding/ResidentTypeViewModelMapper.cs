using ApartmentManagementSystem.Web.Services.DTOs.Onboarding;
using ApartmentManagementSystem.Web.ViewModels.Onboarding;

namespace ApartmentManagementSystem.Web.Mappers.Onboarding;

public static class ResidentTypeViewModelMapper
{
    public static List<ResidentTypeOption> From(List<ResidentTypeDto> dto)
    {
        return dto.Select(x => new ResidentTypeOption
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}









/*public static class ResidentTypeViewModelMapper
{
    // public static List<ResidentTypeOption> From(IEnumerable<ResidentTypeDto> dto)
    public static List<EligibleResidentViewModel> From(IEnumerable<ResidentListDto> dto)

    {
        return dto.Select(r => new ResidentTypeOption
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();
    }
}*/
