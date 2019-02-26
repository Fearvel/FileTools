using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace de.fearvel.io.File
{

    /// <summary>
    /// Class for Encrypting or Decrypting files
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public static class Encryption
    {

        /// <summary>
        /// static function to Encrypt files
        /// </summary>
        /// <param name="inputFile">File to be Encrypted</param>
        /// <param name="outputFile">Output for the Encrypted file</param>
        /// <param name="password">Password</param>
        public static void EncryptFile(string inputFile, string outputFile, string password)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);
                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);
                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// static function to Decrypt files
        /// </summary>
        /// <param name="inputFile">File to be Decrypted</param>
        /// <param name="outputFile">Output for the Decrypted file</param>
        /// <param name="password">Password</param>
        public static void DecryptFile(string inputFile, string outputFile, string password)
        {
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);
                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);
                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        /// <summary>
        /// Decrypts a file and provides a memory string
        /// </summary>
        /// <param name="inputFile">File to be Decrypted</param>
        /// <param name="password">Password</param>
        /// <returns>MemoryStream with the Decrypted file content</returns>
        public static MemoryStream DecryptFileToMemory(string inputFile, string password)
        {
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);
                MemoryStream mStream = new MemoryStream();
                int data;

                while ((data = cs.ReadByte()) != -1)
                    mStream.WriteByte((byte)data);
                cs.Close();
                fsCrypt.Close();
                return mStream;
            }
        }
    }
}