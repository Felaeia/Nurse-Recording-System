using NurseRecordingSystem.Class.Services.UserServices.UserForms;
using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.UserServicesTests.UserFormsTests
{
    public class UpdateUserFormTest
    {
        [Fact]
        public void UpdateUserForm_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new UpdateUserForm(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateUserFormAsync_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new UpdateUserForm(config);
            UpdateUserFormRequestDTO request = null;
            string updater = "TestUser";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateUserFormAsync(request, updater));
            Assert.Equal("userFormRequest", exception.ParamName);
        }

        [Fact]
        public async Task UpdateUserFormAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new UpdateUserForm(config);
            var request = new UpdateUserFormRequestDTO
            {
                formId = 1,
                issueType = "Test Issue",
                issueDescryption = "Test Description",
                status = "Open",
                patientName = "Test Patient"
            };
            string updater = "TestUser";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.UpdateUserFormAsync(request, updater));
        }
    }
}
