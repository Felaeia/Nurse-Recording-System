
using NurseRecordingSystem.Class.Services.UserServices.Users;
using Xunit;

namespace NurseRecordingSystem.Tests.ServiceTests.UserServicesTests
{

    public class CreateUsersServicesTests
    {
        private readonly IConfiguration _config;

        public CreateUsersServicesTests()
        {

            var inMemorySettings = new Dictionary<string, string> {
                {"ConnectionStrings:DefaultConnection", "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NurseRecordingSystem;Integrated Security=True"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenConnectionStringMissing()
        {
            // Arrange
            var badConfig = new ConfigurationBuilder().Build();

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
                new CreateUser(badConfig)
            );

            Assert.Contains("Connection string 'DefaultConnection' not found", ex.Message);
        }

        [Fact]
        public async Task CreateUserAuthenticateAsync_ShouldThrow_WhenAuthRequestIsNull()
        {
            // Arrange
            var service = new CreateUser(_config);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.CreateUserAuthenticateAsync(null!, new DTO.UserServiceDTOs.UsersDTOs.CreateUserRequestDTO())
            );
        }

        //[Fact]
        //public async Task CreateUserAuthenticateAsync_ShouldInsertAuthAndUser()
        //{
            
        //    var service = new CreateUsersServices(_config); 
        //    var authRequest = new CreateAuthenticationRequestDTO
        //    {
        //        UserName = "testuser",
        //        Password = "Password123!",
        //        Email = "test@example.com"
        //    };
        //    var user = new CreateUserRequestDTO
        //    {
        //        FirstName = "asd",
        //        MiddleName = "Q",
        //        LastName = "sad",
        //        ContactNumber = "1234567890",
        //        Address = "Test Address",
              
        //    };

            
        //    var newAuthId = await service.CreateUserAuthenticateAsync(authRequest, user);

            
        //    Assert.True(newAuthId > 0);

        //}

    }
}
