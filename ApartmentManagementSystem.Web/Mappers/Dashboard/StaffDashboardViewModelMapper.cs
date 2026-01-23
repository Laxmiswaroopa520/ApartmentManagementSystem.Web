using ApartmentManagementSystem.Web.Services.DTOs.Dashboard;
using ApartmentManagementSystem.Web.ViewModels.Dashboard;

namespace ApartmentManagementSystem.Web.Mappers.Dashboard
{
    public static class StaffDashboardViewModelMapper
    {
        public static StaffDashboardViewModel From(StaffDashboardDto dto)
        {
            return new StaffDashboardViewModel
            {
                FullName = dto.FullName,
                StaffType = dto.StaffType,
                ShiftStart = dto.ShiftStart,
                ShiftEnd = dto.ShiftEnd,
                TodaysTasks = dto.TodaysTasks,
                CompletedTasks = dto.CompletedTasks,
                PendingTasks = dto.PendingTasks,
                MyTasks = dto.MyTasks.Select(t => new TaskViewModel
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    Status = t.Status
                }).ToList()
            };
        }
    }

}