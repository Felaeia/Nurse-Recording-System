
using Microsoft.Extensions.Configuration;
using Xunit;
using NurseRecordingSystem.Class.Services.UserServices.Users;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminUsersTest
{
    public class DeleteUserTest
    {
        [Fact]
        public void DeleteUser_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new DeleteUser(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new DeleteUser(config);
            int userId = 1;
            string deletedBy = "Admin1";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.SoftDeleteUserAsync(userId, deletedBy));
        }

        [Fact]
        public async Task SoftDeleteUserAsync_NullDeletedBy_ThrowsArgumentNullException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new DeleteUser(config);
            int userId = 1;
            string deletedBy = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SoftDeleteUserAsync(userId, deletedBy));
        }
    }
}
