using NurseRecordingSystem.Class.Services.UserServices.UserForms;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.UserServicesTests.UserFormsTests
{
    public class DeleteUserFormTest
    {
        [Fact]
        public void DeleteUserForm_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new DeleteUserForm(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteUserFormAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new DeleteUserForm(config);
            int formId = 1;
            string deletedBy = "Nurse1";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.DeleteUserFormAsync(formId, deletedBy));
        }

        // Error

        [Fact]
        public async Task DeleteUserFormAsync_NullDeletedBy_ThrowsArgumentNullException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;" })
                .Build();
            var service = new DeleteUserForm(config);
            int formId = 1;
            string deletedBy = null!;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteUserFormAsync(formId, deletedBy));
            Assert.Equal("deletedBy", exception.ParamName);
        }
    }
}
