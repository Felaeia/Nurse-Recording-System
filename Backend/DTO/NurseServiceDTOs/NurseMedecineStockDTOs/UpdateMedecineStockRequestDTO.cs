namespace NurseRecordingSystem.DTO.NurseServiceDTOs.NurseMedecineStockDTOs
{
    public class UpdateMedecineStockRequestDTO
    {
        // Note: MedicineId is often passed in the route, but included here for clarity if needed in the body
        public string MedecineName { get; set; } = string.Empty;
        public string MedecineDescription { get; set; } = string.Empty;
        public int NumberOfStock { get; set; }
    }
}
