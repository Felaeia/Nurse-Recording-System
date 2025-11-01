using Microsoft.AspNetCore.Authorization;

namespace NurseRecordingSystem.Authorization
{
    // This class just holds the "rule" (e.g., "Role must be 'Nurse'")
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> AllowedRoles { get; }

        // The "params" keyword lets you pass one or more roles easily:
        // new RoleRequirement("Nurse")
        // new RoleRequirement("Nurse", "User")
        public RoleRequirement(params string[] roles)
        {
            AllowedRoles = roles;
        }
    }
}