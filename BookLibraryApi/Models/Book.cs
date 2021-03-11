using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookLibraryApi.Models
{
    public class Book
    {
        [Key] public int Id { get; set; }
        public IdentityUser User { get; set; }
        [Column(TypeName = "nvarchar(64)")] public string Author { get; set; }
        [Column(TypeName = "nvarchar(64)")] public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int NumberOfPages { get; set; }
    }
}