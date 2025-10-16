using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.NurseCreation;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.NurseCreation
{
    public interface ICreateNurse
    {
        // Returns the newly created NurseId
        Task<int> CreateNurseAsync(CreateNurseRequestDTO request);
    }
}