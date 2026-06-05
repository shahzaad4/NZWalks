using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Should be atleast 3 characters")]
        [MaxLength(3,ErrorMessage ="Shouuld be 3 characters at most")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
