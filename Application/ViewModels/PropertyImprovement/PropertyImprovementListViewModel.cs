namespace Application.ViewModels.PropertyImprovement
{
    public class PropertyImprovementListViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }

        public string? PropertyCode { get; set; }
        public string? ImprovementName { get; set; }
    }
}
