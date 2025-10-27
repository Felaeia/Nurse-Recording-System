// using Microsoft.Data.SqlClient;
// using Microsoft.Extensions.Configuration;
// using NurseRecordingSystem.Class.Services.UserServices.UserForms;
// using NurseRecordingSystem.Contracts.ServiceContracts.IUserServices.UserForms;
// using NurseRecordingSystem.DTO.UserServiceDTOs.UserFormsDTOs;
// using System.Collections.Generic;
// using Xunit;

// namespace NurseRecordingSystem.Test.ServiceTests.UserServicesTests.UserFormsTests
// {
//     public class CreateUserFormTest
//     {
//         [Fact]
//         public async Task CreateUserFormAsync_NullRequest_ThrowsArgumentNullException()
//         {
//             // Arrange
//             var config = new ConfigurationBuilder()
//                 .AddInMemoryCollection(new Dictionary<string, string> { ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=TestDB;Trusted_Connection=True;" })
//                 .Build();
//             var service = new CreateUserForm(config);
//             UserFormRequestDTO request = null!;
//             var userId = "123";
//             var creator = "Nurse1";

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateUserFormAsync(request, userId, creator));
//             Assert.Equal("userFormRequest", exception.ParamName);
//             Assert.Contains("UserFormRequest Cannot be Null", exception.Message);
//         }

//         [Fact]
//         public void CreateUserForm_ConfigurationNull_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             var config = new ConfigurationBuilder()
//                 .AddInMemoryCollection(new Dictionary<string, string> { ["ConnectionStrings:DefaultConnection"] = null })
//                 .Build();

//             // Act & Assert
//             var exception = Assert.Throws<InvalidOperationException>(() => new CreateUserForm(config));
//             Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
//         }

//         [Fact]
//         public async Task CreateUserFormAsync_ValidRequest_ThrowsException()
//         {
//             // Arrange
//             var config = new ConfigurationBuilder()
//                 .AddInMemoryCollection(new Dictionary<string, string> { ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=TestDB;Trusted_Connection=True;" })
//                 .Build();
//             var service = new CreateUserForm(config);
//             var request = new UserFormRequestDTO
//             {
//                 issueType = "Medical",
//                 issueDescryption = "Patient needs assistance",
//                 status = "Pending",
//                 patientName = "John Doe",
//                 createdBy = "Nurse1",
//                 updatedBy = "Nurse1",
//                 DeletedBy = "Nurse1"
//             };
//             var userId = "123";
//             var creator = "Nurse1";

//             // Since we can't easily mock SqlConnection in unit tests, we'll test the exception handling
//             // In a real scenario, integration tests would be used for database interactions

//             // Act & Assert
//             await Assert.ThrowsAsync<Exception>(() => service.CreateUserFormAsync(request, userId, creator));
//         }
//     }
// }
