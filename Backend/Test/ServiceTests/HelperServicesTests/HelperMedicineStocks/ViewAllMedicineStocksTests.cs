using NurseRecordingSystem.Class.Services.MedecineStockServices;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.HelperServicesTests.HelperMedicineStocks
{
    public class ViewAllMedicineStocksTests
    {
        [Fact]
        public void ViewAllMedecineStocks_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new ViewAllMedecineStocks(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task ViewAllAsync_InvalidConnection_ThrowsSqlException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new ViewAllMedecineStocks(config);

            // Act & Assert
            await Assert.ThrowsAsync<Microsoft.Data.SqlClient.SqlException>(() => service.ViewAllAsync());
        }
    }
}
