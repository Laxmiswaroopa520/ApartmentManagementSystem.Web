using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Dashboard
{
    //mapper class for ownerdashboard
    public static class OwnerDashboardViewModelMapper
    {
        public static OwnerDashboardViewModel From(OwnerDashboardResponse dto)
        {
            return new OwnerDashboardViewModel
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                PendingComplaints = dto.PendingComplaints,
                PendingBills = dto.PendingBills,
                TotalOutstanding = dto.TotalOutstanding,

                MyFlats = dto.MyFlats.Select(f => new FlatSummaryViewModel
                {
                    FlatId = f.FlatId,
                    FlatNumber = f.FlatNumber,
                    ApartmentName = f.ApartmentName,
                    OwnerName = f.OwnerName,
                    TenantName = f.TenantName
                }).ToList()
            };
        }
    }
}