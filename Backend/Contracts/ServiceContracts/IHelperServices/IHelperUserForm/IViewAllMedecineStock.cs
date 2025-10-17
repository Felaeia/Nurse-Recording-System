using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm
{
    public interface IViewAllMedecineStocks
    {
        // Returns a list of all active stock DTOs
        Task<List<ViewAllMedecineStockResponseDTO>> ViewAllAsync();
    }
}
