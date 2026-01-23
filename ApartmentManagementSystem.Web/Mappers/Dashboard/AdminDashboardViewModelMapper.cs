using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Dashboard
{
    public static class AdminDashboardViewModelMapper
    {
        public static EnhancedDashboardViewModel From(EnhancedAdminDashboardDto dto)
        {
            return new EnhancedDashboardViewModel
            {
                FullName = dto.FullName,
                Role = dto.Role,
                AllRoles = dto.AllRoles,

                Stats = new AdvancedDashboardStatsViewModel
                {
                    TotalResidents = dto.Stats.TotalResidents,
                    TotalFlats = dto.Stats.TotalFlats,
                    OccupiedFlats = dto.Stats.OccupiedFlats,
                    VacantFlats = dto.Stats.VacantFlats,
                    PendingRegistrations = dto.Stats.PendingRegistrations,
                    TotalStaffMembers = dto.Stats.TotalStaffMembers,
                    ActiveStaffMembers = dto.Stats.ActiveStaffMembers,
                    CommunityMembers = dto.Stats.CommunityMembers,
                    PendingComplaints = dto.Stats.PendingComplaints,
                    ResolvedComplaintsThisMonth = dto.Stats.ResolvedComplaintsThisMonth,
                    TotalOutstandingBills = dto.Stats.TotalOutstandingBills,
                    CollectionThisMonth = dto.Stats.CollectionThisMonth,
                    TodaysVisitors = dto.Stats.TodaysVisitors,
                    ActiveSecurityPersonnel = dto.Stats.ActiveSecurityPersonnel
                },

                RecentActivities = dto.RecentActivities
                    .Select(a => new RecentActivityViewModel
                    {
                        Activity = a.Activity,
                        Type = a.Type,
                        Timestamp = a.Timestamp
                    })
                    .ToList(),

                QuickActions = dto.QuickActions
                    .Select(q => new QuickActionViewModel
                    {
                        Title = q.Title,
                        Icon = q.Icon,
                        Url = q.Url,
                        Color = q.Color,
                        RequiresPermission = q.RequiresPermission
                    })
                    .ToList(),

                FinancialSummary = dto.FinancialSummary == null ? null :
                    new FinancialSummaryViewModel
                    {
                        TotalOutstanding = dto.FinancialSummary.TotalOutstanding,
                        CollectedThisMonth = dto.FinancialSummary.CollectedThisMonth,
                        CollectedLastMonth = dto.FinancialSummary.CollectedLastMonth,
                        PendingMaintenanceFees = dto.FinancialSummary.PendingMaintenanceFees,
                        PendingUtilityBills = dto.FinancialSummary.PendingUtilityBills,
                        Last6MonthsCollection = dto.FinancialSummary.Last6MonthsCollection
                            .Select(m => new MonthlyCollectionViewModel
                            {
                                Month = m.Month,
                                Amount = m.Amount
                            })
                            .ToList()
                    }
            };
        }
    }

}
