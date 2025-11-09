using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using NurseRecordingSystem.Authorization;
using System.Data;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AuthenticationServices
{
    public class SessionTokenAuthorizationHandlerTest
    {
        [Fact]
        public void Constructor_ValidConnectionString_CreatesHandler()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var inMemorySettings = new Dictionary<string, string> { ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=TestDB;Trusted_Connection=True;" };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            // Act
            var handler = new SessionTokenAuthorizationHandler(mockHttpContextAccessor.Object, config);

            // Assert
            Assert.NotNull(handler);
        }

        [Fact]
        public void Constructor_InvalidConnectionString_ThrowsException()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var inMemorySettings = new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new SessionTokenAuthorizationHandler(mockHttpContextAccessor.Object, config));
        }

    }
}
