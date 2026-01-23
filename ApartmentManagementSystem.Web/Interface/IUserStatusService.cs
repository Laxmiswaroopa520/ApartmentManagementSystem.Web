namespace ApartmentManagementSystem.Web.Interface
{
    public interface IUserStatusService
    {
        Task<bool> IsUserActiveAsync(Guid userId);
    }
}