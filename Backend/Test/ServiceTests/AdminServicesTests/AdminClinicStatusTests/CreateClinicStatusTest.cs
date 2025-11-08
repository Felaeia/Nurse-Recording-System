using Moq;
using NurseRecordingSystem.Class.Services.ClinicStatusServices;
using NurseRecordingSystem.DTO.AdminServiceDTOs.AdminClinicStatusDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminClinicStatusTests
{
    public class CreateClinicStatusTest
    {
        [Fact]
        public async Task CreateAsync_InvalidConnectionString_ThrowsException()
        {
            // Arrange
            var mockConnectionStrings = new Mock<IConfigurationSection>();
            mockConnectionStrings.Setup(IConfigurationSection => IConfigurationSection["DefaultConnection"]).Returns((string)null);
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(IConfiguration => IConfiguration.GetSection("ConnectionStrings")).Returns(mockConnectionStrings.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => new CreateClinicStatus(mockConfiguration.Object).CreateAsync(new CreateClinicStatusRequestDTO(), "test"));
        }
    }
}
