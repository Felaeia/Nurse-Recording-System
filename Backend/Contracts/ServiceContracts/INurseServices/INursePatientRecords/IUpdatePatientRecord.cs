using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords
{
    public interface IUpdatePatientRecord
    {
        Task<UpdatePatientRecordResponseDTO> UpdatePatientRecordAsync(UpdatePatientRecordRequestDTO request, string updatedBy);
    }
}
