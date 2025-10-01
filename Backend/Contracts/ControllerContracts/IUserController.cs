using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Model.DTO.AuthDTOs;

namespace NurseRecordingSystem.Contracts.ControllerContracts
{
    public interface IUserController
    {
        [HttpPost("create-user")]
        Task<IActionResult> CreateAuthentication([FromBody] CreateUserWithAuthenticationDTO request);
    }
}
