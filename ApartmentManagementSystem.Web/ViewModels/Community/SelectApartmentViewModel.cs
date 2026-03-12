//added this view model for selecting apartments while assigning roles for community memebers....
namespace ApartmentManagementSystem.Web.ViewModels.Community
{
    public class SelectApartmentViewModel
    {
        public List<ApartmentDropdownItem> Apartments { get; set; } = new();
    }
}
