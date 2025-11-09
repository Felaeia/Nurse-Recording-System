using NurseRecordingSystem.Authorization;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AuthenticationServices
{
    public class RoleRequirementTest
    {
        [Fact]
        public void Constructor_SingleRole_SetsAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Admin");

            // Assert
            Assert.Single(requirement.AllowedRoles);
            Assert.Contains("Admin", requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_MultipleRoles_SetsAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Admin", "Nurse", "User");

            // Assert
            Assert.Equal(3, requirement.AllowedRoles.Count());
            Assert.Contains("Admin", requirement.AllowedRoles);
            Assert.Contains("Nurse", requirement.AllowedRoles);
            Assert.Contains("User", requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_NoRoles_SetsEmptyAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement();

            // Assert
            Assert.Empty(requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_DuplicateRoles_IncludesDuplicates()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Admin", "Admin");

            // Assert
            Assert.Equal(2, requirement.AllowedRoles.Count());
            Assert.All(requirement.AllowedRoles, role => Assert.Equal("Admin", role));
        }
    }
}
