using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs;

namespace NurseRecordingSystem.Contracts.ControllerContracts
{
    public interface IUserController
    {
        [HttpPost("create-user")]
        Task<IActionResult> CreateAuthentication([FromBody] CreateAuthenticationRequestDTO request);
    }
}
