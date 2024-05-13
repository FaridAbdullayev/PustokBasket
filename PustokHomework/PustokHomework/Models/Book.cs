using Pustok.Attributes.ValidationAttributes;
using Pustok.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokHomework.Models
{
    public class Book:AuditEntity
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [MinLength(10)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercent { get; set; }
        public bool StockStatus { get; set; }
        public List<BookImage> Images { get; set; } = new List<BookImage>();
        public List<BookTag> BookTags { get; set; } = new List<BookTag>();
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        [NotMapped]
        [MaxSize(2 * 1024 * 1024)]
        [AllowedFileTypes("image/png", "image/jpeg")]
        public IFormFile PosterFile { get; set; }
        [NotMapped]
        [MaxSize(2 * 1024 * 1024)]
        [AllowedFileTypes("image/png", "image/jpeg")]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        [MaxSize(2 * 1024 * 1024)]
        [AllowedFileTypes("image/png", "image/jpeg")]
        public List<IFormFile>? ImageFiles { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<int>? TagIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int>? BookImageIds { get; set; } = new List<int>();
    }
}
