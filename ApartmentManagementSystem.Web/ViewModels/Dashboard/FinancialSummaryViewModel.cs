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
       // public decimal TotalOutstanding { get; set; }
        public decimal CollectionThisMonth { get; set; }
        public decimal CollectionLastMonth { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public int PendingPayments { get; set; }
        public int OverduePayments { get; set; }
        public decimal AverageMonthlyCollection { get; set; }
        public List<MonthlyCollectionViewModel> MonthlyCollections { get; set; } = new();
    }
}