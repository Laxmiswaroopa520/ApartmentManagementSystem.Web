using ApartmentManagementSystem.Web.Services.DTOs.Apartment;
using ApartmentManagementSystem.Web.ViewModels.Apartment;

namespace ApartmentManagementSystem.Web.Mappers.Apartment
{
    public static class ApartmentDiagramMapper
    {
        public static ApartmentDiagramViewModel From(ApartmentDiagramDto dto)
        {
            return new ApartmentDiagramViewModel
            {
                ApartmentId = dto.ApartmentId,
                Name = dto.Name,
                Address=dto.Address,                    //Added this as part of visualize 3d view 
                TotalFloors = dto.TotalFloors,
                Floors = dto.Floors.Select(MapFloor).ToList()
            };
        }

        private static FloorDiagramViewModel MapFloor(FloorDiagramDto dto)
        {
            return new FloorDiagramViewModel
            {
                FloorId = dto.FloorId,
                FloorNumber = dto.FloorNumber,
                Name = dto.Name,
                Flats = dto.Flats.Select(MapFlat).ToList()
            };
        }

        private static FlatDiagramViewModel MapFlat(FlatDiagramDto dto)
        {
            return new FlatDiagramViewModel
            {
                FlatId = dto.FlatId,
                FlatNumber = dto.FlatNumber,
                IsOccupied = dto.IsOccupied,
                OccupantName = dto.OccupantName,
                OccupantType = dto.OccupantType,
                Status = dto.Status
            };
        }
    }
}