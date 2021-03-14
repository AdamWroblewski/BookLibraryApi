using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookLibraryApi.CustomAttributes;
using Microsoft.AspNetCore.Identity;

namespace BookLibraryApi.Models
{
    public class Book
    {
        [Key] public int Id { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public string Author { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public string Title { get; set; }

        [Required] public int ReleaseYear { get; set; }
        [Required] [MinValue(0)] public int NumberOfPages { get; set; }
    }
}