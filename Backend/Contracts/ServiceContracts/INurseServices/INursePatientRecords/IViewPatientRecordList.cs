using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords
{
    public interface IViewPatientRecordList
    {
        Task<List<PatientRecordListItemDTO>> GetPatientRecordListAsync(int? nurseId = null);
    }
}
