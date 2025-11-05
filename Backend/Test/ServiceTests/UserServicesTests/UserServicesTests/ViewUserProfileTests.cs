using NurseRecordingSystem.Class.Services.UserServices.Users;
using NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.UserServicesTests.UserServicesTests
{
    public class ViewUserProfileTests
    {
        [Fact]
        public void ViewUserProfile_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new ViewUserProfile(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task GetUserProfileAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new ViewUserProfile(config);
            int userId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetUserProfileAsync(userId));
        }
    }
}
