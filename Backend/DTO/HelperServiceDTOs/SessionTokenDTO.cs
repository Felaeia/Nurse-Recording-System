using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class SessionTokenDTO
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        public byte[] Token { get; set; } = null!;

        [Required]
        public int AuthId { get; set; }

        [Required]
        public DateTime ExpiresOn { get; set; }

        [Required]
        public int IsActive { get; set; }
    }
}