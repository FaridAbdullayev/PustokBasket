using System.ComponentModel.DataAnnotations;

namespace PustokHomework.Models
{
    public class BookImage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        public bool? PosterStatus { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
