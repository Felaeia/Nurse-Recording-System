namespace NurseRecordingSystem.Class.Services.HelperServices
{
    public class PasswordHelper 
    {
        //Create password hash function
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Verify password hash function
        public static bool VerifyPasswordHash(string password, byte[] storedPasswordHash, byte[] storedPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedPasswordHash);
            }
        }
    }
}
