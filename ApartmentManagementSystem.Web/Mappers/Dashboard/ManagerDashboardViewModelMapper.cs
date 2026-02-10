using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Dashboard
{
    public static class ManagerDashboardViewModelMapper
    {
        public static ManagerDashboardViewModel From(ManagerDashboardDto dto)
        {
            return new ManagerDashboardViewModel
            {
                FullName = dto.FullName,
                Role = dto.Role,
                ApartmentId = dto.ApartmentId,
                ApartmentName = dto.ApartmentName,
                Stats = new ApartmentDashboardStatsViewModel
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
                    TodaysVisitors = dto.Stats.TodaysVisitors
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
                NoticeBoard = dto.NoticeBoard
                    .Select(n => new NoticeBoardMessageViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Priority = n.Priority,
                        Category = n.Category,
                        PostedBy = n.PostedBy,
                        FlatNumber = n.FlatNumber,
                        PostedAt = n.PostedAt,
                        IsResolved = n.IsResolved
                    })
                    .ToList(),
                PendingResidents = dto.PendingResidents
                    .Select(p => new PendingResidentViewModel
                    {
                        UserId = p.UserId,
                        FullName = p.FullName,
                        PrimaryPhone = p.PrimaryPhone,
                        Email = p.Email,
                        ResidentType = p.ResidentType,
                        RegisteredOn = p.RegisteredOn,
                        Status = p.Status
                    })
                    .ToList()
            };
        }
    }
}