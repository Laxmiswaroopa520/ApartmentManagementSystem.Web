//added this for apartment drop down in staff members feature
namespace ApartmentManagementSystem.Web.ViewModels.Community
{
    /// <summary>Typed item for apartment dropdowns — avoids anonymous type issues in Razor views.</summary>
    public class ApartmentDropdownItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
