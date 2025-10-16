using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords
{
    public interface ICreatePatientRecord
    {
        Task<int> CreatePatientRecordAsync(CreatePatientRecordRequestDTO request, string createdBy);
    }
}
