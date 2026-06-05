using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Should be atleast 3 characters")]
        [MaxLength(3, ErrorMessage = "Shouuld be 3 characters at most")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
