using Microsoft.AspNetCore.Authorization;

namespace NurseRecordingSystem.Authorization
{
    // This class just holds the "rule" (e.g., "Role must be 'Nurse'")
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}