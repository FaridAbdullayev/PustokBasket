using Pustok.Models;
using System.ComponentModel.DataAnnotations;

namespace PustokHomework.Models
{
    public class Genre: AuditEntity
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
