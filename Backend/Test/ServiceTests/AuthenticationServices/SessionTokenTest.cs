using NurseRecordingSystem.Class.Services.Authentication;
using NurseRecordingSystem.DTO.AuthServiceDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AuthenticationServices
{
    public class SessionTokenTest
    {
        [Fact]
        public void SessionTokenService_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new SessionTokenService(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task CreateSessionAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new SessionTokenService(config);
            int authId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.CreateSessionAsync(authId));
        }

        [Fact]
        public async Task RefreshSessionTokenAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new SessionTokenService(config);
            int authId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.RefreshSessionTokenAsync(authId));
        }

        [Fact]
        public async Task EndSessionAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new SessionTokenService(config);
            byte[] tokenBytes = new byte[64];

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.EndSessionAsync(tokenBytes));
        }

        [Fact]
        public async Task ValidateTokenAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new SessionTokenService(config);
            int authId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.ValidateTokenAsync(authId));
        }
    }
}
