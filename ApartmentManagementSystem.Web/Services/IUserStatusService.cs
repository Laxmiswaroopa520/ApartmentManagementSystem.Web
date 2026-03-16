namespace ApartmentManagementSystem.Web.Services
{
    public interface IUserStatusService
    {
        Task<bool> IsUserActiveAsync(Guid userId);
    }
}