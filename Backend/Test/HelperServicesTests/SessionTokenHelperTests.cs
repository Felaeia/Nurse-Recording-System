using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using Xunit;

namespace NurseRecordingSystem.Tests.ServiceTests.HelperServicesTests
{
   public class SessionTokenHelperTests
   {
       private readonly IConfiguration _config;

       public SessionTokenHelperTests()
       {
           var inMemorySettings = new System.Collections.Generic.Dictionary<string, string> {
               {"ConnectionStrings:DefaultConnectionString", "Server=test;Database=db;User Id=invalid;Password=invalid;Connection Timeout=1;"}
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
               new sessionTokenHelper(badConfig)
           );

           Assert.Contains("Connection string 'Default Connecection' not Found", ex.Message);
       }

       [Fact]
       public void Constructor_ShouldSucceed_WhenConnectionStringPresent()
       {
           var helper = new sessionTokenHelper(_config);

           // Assert
           Assert.NotNull(helper);
       }
   }
}
