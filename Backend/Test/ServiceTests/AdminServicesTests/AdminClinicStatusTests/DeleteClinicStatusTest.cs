using Moq;
using NurseRecordingSystem.Class.Services.ClinicStatusServices;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminClinicStatusTests
{
    public class DeleteClinicStatusTest
    {
        [Fact]
        public async Task DeleteAsync_InvalidConnectionString_ThrowsException()
        {
            // Arrange
            var mockConnectionStrings = new Mock<IConfigurationSection>();
            mockConnectionStrings.Setup(IConfigurationSection => IConfigurationSection["DefaultConnection"]).Returns((string)null);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(IConfiguration => IConfiguration.GetSection("ConnectionStrings")).Returns(mockConnectionStrings.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => new DeleteClinicStatus(mockConfiguration.Object).DeleteAsync(1, "test"));
        }
    }
}
