using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperClinicStatusDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IHelperServices.IHelperClinicStatus
{
    public interface IViewClinicStatus
    {
        Task<List<ViewClinicStatusResponseDTO>> ViewAllAsync();
    }
}
