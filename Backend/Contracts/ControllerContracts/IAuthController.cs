using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Model.DTO.AuthDTOs;

namespace NurseRecordingSystem.Contracts.ControllerContracts
{
    public interface IAuthController
    {
        [HttpPost("Login")]
        Task<IActionResult> LoginUser([FromBody] LoginRequestDTO loginUser);
    }
}
