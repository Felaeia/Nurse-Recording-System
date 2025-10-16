using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords
{
    public interface IViewPatientRecord
    {
        Task<ViewPatientRecordResponseDTO> GetPatientRecordAsync(int patientRecordId);
    }
}
