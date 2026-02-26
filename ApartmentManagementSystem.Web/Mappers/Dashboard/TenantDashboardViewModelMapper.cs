using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Dashboard
{
        public static class TenantDashboardViewModelMapper
        {
            public static TenantDashboardViewModel From(dynamic dto)
            {
                return new TenantDashboardViewModel
                {
                    FullName = dto.FullName,
                    PendingComplaints = dto.PendingComplaints,
                    PendingRent = dto.PendingRent,

                    MyFlat = dto.MyFlat == null ? null : new FlatSummaryViewModel
                    {
                        FlatNumber = dto.MyFlat.FlatNumber,
                        ApartmentName = dto.MyFlat.ApartmentName,
                        OwnerName = dto.MyFlat.OwnerName
                    }
                };
            }
        }
    }
