using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UserFormsDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUserForms
{
    // Change class to interface to match naming and intent
    public interface IViewUserFormList
    {
        Task<List<UserFormListItemDTO>> GetUserFormListAsync(int? userId, string? status);
    }
}
