using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Contracts.ControllerContracts
{
    public interface IAuthController
    {
        [HttpPost("Login")]
        Task<IActionResult> LoginUser([FromBody] LoginRequestDTO loginUser);
    }
}
