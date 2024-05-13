
using Pustok.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokHomework.Models
{
    public class Slider
    {
        public int Id { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string BtnText { get; set; }
        public string BtnUrl { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [MaxSize(1024 * 1024 * 2)]
        [AllowedFileTypes("image/png", "image/jpeg")]
        public IFormFile? ImageFile { get; set; }
        public int Order { get; set; }    
    }
}
