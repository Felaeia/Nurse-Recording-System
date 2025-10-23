using NurseRecordingSystem.DTO.NurseServiceDTOs.NurseClinicStatusDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseClinicStatus
{
    public interface IUpdateClinicStatus
    {
        Task<bool> UpdateAsync(int logId, UpdateClinicStatusRequestDTO request, string updatedBy);
    }
}
