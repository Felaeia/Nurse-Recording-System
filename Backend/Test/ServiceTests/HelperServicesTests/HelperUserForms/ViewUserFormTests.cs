using NurseRecordingSystem.Class.Services.UserServices.UserForms;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.HelperServicesTests.HelperUserForms
{
    public class ViewUserFormTests
    {
        [Fact]
        public void ViewUserForm_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new ViewUserForm(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task GetUserFormAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new ViewUserForm(config);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetUserFormAsync(1));
        }
    }
}
