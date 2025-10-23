namespace NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs
{
    public class ViewAllMedecineStockResponseDTO
    {
        public int MedicineId { get; set; }
        public string MedecineName { get; set; } = string.Empty;
        public string MedecineDescription { get; set; } = string.Empty;
        public int NumberOfStock { get; set; }
    }
}
