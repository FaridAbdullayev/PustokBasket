using PustokHomework.Models;

namespace PustokHomework.ViewModel
{
    public class HomeViewModel
    {
        public List<Slider> Sliders {  get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> DiscountedBooks { get; set; }
        public List<Feature> Features { get; set; }
    }
}
