using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Common
{
    public class SystemStatusDto
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(32)]
        public required string Code { get; set; }

        [Required]
        [MaxLength(32)]
        public required string StatusName { get; set; }

        [MaxLength(64)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }
    }
}
