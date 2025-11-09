using Microsoft.Extensions.Configuration;
using Xunit;
using NurseRecordingSystem.Class.Services.NurseServices.PatientRecords;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminPatientRecord
{
    public class DeletePatientRecordTest
    {
        [Fact]
        public void Constructor_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            IConfiguration config = null;

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new DeletePatientRecord(config));
            Assert.Contains("Configuration cannot be null.", exception.Message);
        }

        [Fact]
        public void Constructor_ConnectionStringNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new DeletePatientRecord(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task SoftDeletePatientRecordAsync_InvalidConnection_ThrowsException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)//MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=1;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new DeletePatientRecord(config);
            int patientRecordId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.SoftDeletePatientRecordAsync(patientRecordId));
        }
    }
}
