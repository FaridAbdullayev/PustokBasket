namespace PustokHomework.ViewModel
{
    public class BasketDetailViewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Image { get; set; }
        public bool StockStatus { get; set; }
        public int Count {  get; set; }

    }
}
