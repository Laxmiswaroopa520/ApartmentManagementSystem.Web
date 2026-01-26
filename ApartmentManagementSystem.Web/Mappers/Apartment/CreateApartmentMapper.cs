using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Apartment;

namespace ApartmentManagementSystem.Web.Mappers.Apartment
{
    public static class CreateApartmentMapper
    {
        public static CreateApartmentDto ToDto(CreateApartmentViewModel viewModel)
        {
            return new CreateApartmentDto
            {
                Name = viewModel.Name,
                Address = viewModel.Address,
                City = viewModel.City,
                State = viewModel.State,
                PinCode = viewModel.PinCode,
                TotalFloors = viewModel.TotalFloors,
                FlatsPerFloor = viewModel.FlatsPerFloor
            };
        }
    }
}