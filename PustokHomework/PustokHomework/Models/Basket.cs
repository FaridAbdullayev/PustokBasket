namespace PustokHomework.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set;}
        public int Count {  get; set;}
        List<Book> Books { get; set;}
    }
}
