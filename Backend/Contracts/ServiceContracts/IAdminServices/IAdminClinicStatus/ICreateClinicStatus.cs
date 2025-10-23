using NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.IAdminServices.IAdminClinicStatus
{
    public interface ICreateClinicStatus
    {
        Task<int> CreateAsync(CreateClinicStatusRequestDTO request, string createdBy);
    }
}
