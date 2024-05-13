using Pustok.Models;
using System.ComponentModel.DataAnnotations;

namespace PustokHomework.Models
{
    public class Author:AuditEntity
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [MinLength(3)]
        [Required]
        public string FullName { get; set; }
        public List<Book>? Books { get; set; }
    }
}
