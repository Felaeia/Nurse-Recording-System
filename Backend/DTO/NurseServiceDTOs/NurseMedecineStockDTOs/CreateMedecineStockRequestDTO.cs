using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs
{
    public class CreateMedecineStockRequestDTO
    {
        [Required]
        public string MedecineName { get; set; } = string.Empty;

        [Required]
        public string MedecineDescription { get; set; } = string.Empty;

        [Required]
        public int NumberOfStock { get; set; }

        [Required]
        public int NurseId { get; set; } // The ID of the nurse creating the stock
    }
}
