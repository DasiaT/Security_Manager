using System.Security.Cryptography;
using System.Text;

namespace Manager_Security_BackEnd.Models.Generals
{
    public class General_Generate_Password
    {
        public async Task<string> Generate_SHA512_Password(string password)
        {
            return await Task.Run(() =>
            {
                byte[] salt = GenerateSalt();

                byte[] saltedPassword = CombineByteArrays(Encoding.UTF8.GetBytes(password), salt);

                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashBytes = sha512.ComputeHash(saltedPassword);

                    string hashedPassword = Convert.ToBase64String(hashBytes);

                    string saltString = Convert.ToBase64String(salt);

                    return hashedPassword + ":" + saltString;
                }
            });
        }
        public async Task<string> ValidatePasswordAsync(string password, string hashedPasswordWithSalt)
        {
            string[] parts = hashedPasswordWithSalt.Split(':');
            string storedHashedPassword = parts[0];
            string storedSalt = parts[1];

            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] saltedPassword = CombineByteArrays(Encoding.UTF8.GetBytes(password), salt);

            return await Task.Run(() =>
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashBytes = sha512.ComputeHash(saltedPassword);
                    string hashedPassword = Convert.ToBase64String(hashBytes);
                    return $"{hashedPassword}:{storedSalt}";
                }
            });
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private byte[] CombineByteArrays(byte[] array1, byte[] array2)
        {
            byte[] combined = new byte[array1.Length + array2.Length];
            Array.Copy(array1, 0, combined, 0, array1.Length);
            Array.Copy(array2, 0, combined, array1.Length, array2.Length);
            return combined;
        }
    }
}
