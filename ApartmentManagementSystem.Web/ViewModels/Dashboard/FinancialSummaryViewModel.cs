namespace ApartmentManagementSystem.Web.ViewModels.Dashboard
{
    public class FinancialSummaryViewModel
    {
        public decimal TotalOutstanding { get; set; }
        public decimal CollectedThisMonth { get; set; }
        public decimal CollectedLastMonth { get; set; }
        public decimal PendingMaintenanceFees { get; set; }
        public decimal PendingUtilityBills { get; set; }
        public List<MonthlyCollectionViewModel> Last6MonthsCollection { get; set; } = new();
    }
}