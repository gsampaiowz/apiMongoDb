namespace minimalAPIMongo.ViewModels
{
    public class OrderViewModel
    {
        public DateOnly Date {  get; set; }
        public int Status { get; set; }
        public List<string>? ProductsIds { get; set; }
        public string? ClientId { get; set; }

    }
}
