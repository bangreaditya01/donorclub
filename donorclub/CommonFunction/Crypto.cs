using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using System.Web.Mvc;

namespace LudoFoundation_app.CommanFunction
{


    public class Crypto
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("Your32ByteSecureKeyHere!!!123456");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("16ByteInitVector!"); // 16 bytes IV for AES

        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC; // CBC is secure with a good IV
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream msEncrypt = new MemoryStream())
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                    csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
        public static string Encrypt(string stringvalue, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(stringvalue);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public static string Decrypt(string hexvalue, System.Text.Encoding encoding)
        {
            int CharsLength = hexvalue.Length;
            byte[] bytesarray = new byte[CharsLength / 2];
            for (int i = 0; i < CharsLength; i += 2)
            {
                bytesarray[i / 2] = Convert.ToByte(hexvalue.Substring(i, 2), 16);
            }
            return encoding.GetString(bytesarray);
        }



        //public const string Password = "CADCONFERNAGPUR";

        //public static string Decrypt(string TextToBeDecrypted)
        //{
        //    RijndaelManaged RijndaelCipher = new RijndaelManaged();

        //    string DecryptedData;

        //    try
        //    {
        //        //byte[] EncryptedData = Convert.FromBase64String(TextToBeDecrypted);

        //        //byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
        //        ////Making of the key for decryption
        //        //PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        //        ////Creates a symmetric Rijndael decryptor object.
        //        //ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

        //        //MemoryStream memoryStream = new MemoryStream(EncryptedData);
        //        ////Defines the cryptographics stream for decryption.THe stream contains decrpted data
        //        //CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

        //        //byte[] PlainText = new byte[EncryptedData.Length];
        //        //int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
        //        //memoryStream.Close();
        //        //cryptoStream.Close();

        //        ////Converting to string
        //        //DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

        //        string EncryptionKey = "MAKV2SPBNI99212";
        //        TextToBeDecrypted = TextToBeDecrypted.Replace(" ", "+");
        //        byte[] cipherBytes = Convert.FromBase64String(TextToBeDecrypted);
        //        using (Aes encryptor = Aes.Create())
        //        {
        //            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //            encryptor.Key = pdb.GetBytes(32);
        //            encryptor.IV = pdb.GetBytes(16);
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                    cs.Close();
        //                }
        //                TextToBeDecrypted = Encoding.Unicode.GetString(ms.ToArray());
        //            }
        //        }
        //        DecryptedData = TextToBeDecrypted;
        //    }
        //    catch
        //    {
        //        DecryptedData = TextToBeDecrypted;
        //    }
        //    return DecryptedData;
        //}

        //public static string Encrypt(string TextToBeEncrypted)
        //{
        //    //RijndaelManaged RijndaelCipher = new RijndaelManaged();
        //    //byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(TextToBeEncrypted);
        //    //byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
        //    //PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        //    ////Creates a symmetric encryptor object. 
        //    //ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        //    //MemoryStream memoryStream = new MemoryStream();
        //    ////Defines a stream that links data streams to cryptographic transformations
        //    //CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
        //    //cryptoStream.Write(PlainText, 0, PlainText.Length);
        //    ////Writes the final state and clears the buffer
        //    //cryptoStream.FlushFinalBlock();
        //    //byte[] CipherBytes = memoryStream.ToArray();
        //    //memoryStream.Close();
        //    //cryptoStream.Close();
        //    string EncryptedData = "";
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(TextToBeEncrypted);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            EncryptedData = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return EncryptedData;
        //}
    }
}