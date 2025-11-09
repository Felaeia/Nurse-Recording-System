using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.HelperServicesTests.HelperAuthentication
{
    public class PasswordHelperTest
    {
        [Fact]
        public void CreatePasswordHash_ValidPassword_GeneratesHashAndSalt()
        {
            // Arrange
            string password = "TestPassword123";

            // Act
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.Equal(64, passwordHash.Length); // HMACSHA512 produces 64-byte hash
            Assert.Equal(128, passwordSalt.Length); // HMACSHA512 key is 128 bytes (1024 bits)
        }

        [Fact]
        public void VerifyPasswordHash_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "TestPassword123";
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            bool result = PasswordHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPasswordHash_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string password = "TestPassword123";
            string wrongPassword = "WrongPassword456";
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            bool result = PasswordHelper.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyPasswordHash_DifferentSalt_ReturnsFalse()
        {
            // Arrange
            string password = "TestPassword123";
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            PasswordHelper.CreatePasswordHash(password, out byte[] _, out byte[] differentSalt);

            // Act
            bool result = PasswordHelper.VerifyPasswordHash(password, passwordHash, differentSalt);

            // Assert
            Assert.False(result);
        }
    }
}
