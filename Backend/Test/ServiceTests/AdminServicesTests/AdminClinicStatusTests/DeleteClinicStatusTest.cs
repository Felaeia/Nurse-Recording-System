using Moq;
using NurseRecordingSystem.Class.Services.ClinicStatusServices;
using Xunit;
using Microsoft.Data.SqlClient;
using System.Data;

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

        [Fact]
        public async Task DeleteAsync_InvalidLogId_ThrowsSqlException()
        {
            // Arrange
            var mockConnectionStrings = new Mock<IConfigurationSection>();
            mockConnectionStrings.Setup(IConfigurationSection => IConfigurationSection["DefaultConnection"]).Returns("Server=localhost;Database=TestDB;Trusted_Connection=True;Connection Timeout=1;");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(IConfiguration => IConfiguration.GetSection("ConnectionStrings")).Returns(mockConnectionStrings.Object);
            var service = new DeleteClinicStatus(mockConfiguration.Object);

            // Act & Assert
            await Assert.ThrowsAsync<SqlException>(() => service.DeleteAsync(0, "test"));
        }

        [Fact]
        public async Task DeleteAsync_InvalidDeletedBy_ThrowsSqlException()
        {
            // Arrange
            var mockConnectionStrings = new Mock<IConfigurationSection>();
            mockConnectionStrings.Setup(IConfigurationSection => IConfigurationSection["DefaultConnection"]).Returns("Server=localhost;Database=TestDB;Trusted_Connection=True;Connection Timeout=1;");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(IConfiguration => IConfiguration.GetSection("ConnectionStrings")).Returns(mockConnectionStrings.Object);
            var service = new DeleteClinicStatus(mockConfiguration.Object);

            // Act & Assert
            await Assert.ThrowsAsync<SqlException>(() => service.DeleteAsync(1, ""));
        }
    }
}
