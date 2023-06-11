using Azure.Core.GeoJson;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationService.Helpers
{
    public class Security
    {
        public static string Encrypt(string plainText, string toEncrypt/*, bool useHashing = true*/)
        {
            /*byte[] resultArray = null;*/
            byte[] keyArray = null;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            try
            {
                

                /*if (useHashing)
                {*/
                    using (var sha512 = SHA512.Create())
                    {
                        byte[] key = sha512.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
                        byte[] truncatedKey = new byte[32]; // 256-bit key size

                        Buffer.BlockCopy(key, 0, truncatedKey, 0, truncatedKey.Length);

                        using (var aes = Aes.Create())
                        {
                            aes.Key = truncatedKey;
                            aes.GenerateIV();

                            using (var encryptor = aes.CreateEncryptor())
                            {
                                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                                byte[] encryptedBytes = new byte[aes.IV.Length + cipherBytes.Length];
                                Buffer.BlockCopy(aes.IV, 0, encryptedBytes, 0, aes.IV.Length);
                                Buffer.BlockCopy(cipherBytes, 0, encryptedBytes, aes.IV.Length, cipherBytes.Length);

                                keyArray = encryptedBytes;
                            }
                        }
                    }
                    /*using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
                    {
                        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    }*/
                /*}
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
*/

                /*using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateEncryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }*/

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            return Convert.ToBase64String(keyArray, 0, keyArray.Length);
        }
    }
}
