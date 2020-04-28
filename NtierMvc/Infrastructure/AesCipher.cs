﻿using BotDetect.C5;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NtierMvc.Infrastructure
{
    public class AesCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;
        //16CD9AA16D2294C3EA21B9AAF49F4

        private const string initVector = "dvet9uzpgzl88wxg";
        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        //public static string EncryptString(string plainText, string passPhrase)
        //{
        //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        //    byte[] keyBytes = password.GetBytes(Keysize / 8);
        //    RijndaelManaged symmetricKey = new RijndaelManaged();
        //    symmetricKey.Mode = CipherMode.CBC;
        //    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        //    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        //    cryptoStream.FlushFinalBlock();
        //    byte[] cipherTextBytes = memoryStream.ToArray();
        //    memoryStream.Close();
        //    cryptoStream.Close();
        //    return Convert.ToBase64String(cipherTextBytes);
        //}
        //Decrypt
        //public static string DecryptString(string cipherText, string passPhrase)
        //{
        //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        //    byte[] keyBytes = password.GetBytes(Keysize / 8);
        //    RijndaelManaged symmetricKey = new RijndaelManaged();
        //    symmetricKey.Mode = CipherMode.CBC;
        //    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        //    MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        //    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        //    memoryStream.Close();
        //    cryptoStream.Close();
        //    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        //}

        public static string EncryptString(string plainText, string passPhrase)
        {
            try
            {
                plainText = plainText + passPhrase;
                string strRet = null;
                string strSub = null;
                ArrayList arrOffsets = new ArrayList();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;

                arrOffsets.Insert(0, 94);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 44);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 82);

                strRet = "";

                for (intCounter = 0; intCounter <= plainText.Length - 1;
                intCounter++)
                {
                    strSub = plainText.Substring(intCounter, 1);
                    intVal =
                    (int)System.Text.Encoding.ASCII.GetBytes(strSub)[0];
                    intMod = intCounter % arrOffsets.Count;
                    intNewVal = intVal +
                    Convert.ToInt32(arrOffsets[intMod]);
                    intNewVal = intNewVal % 256;
                    strRet = strRet + intNewVal.ToString("X2");
                }
                return strRet;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DecryptString(string Source, string passPhrase)
        {
            try
            {
               
                ArrayList arrOffsets = new ArrayList();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;
                string strOut = null;
                string strSub = null;
                string strSub1 = null;
                string strDecimal = null;

                arrOffsets.Insert(0, 94);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 44);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 82);

                strOut = "";
                for (intCounter = 0; intCounter <= Source.Length - 1;
                intCounter += 2)
                {
                    strSub = Source.Substring(intCounter, 1);
                    strSub1 = Source.Substring((intCounter + 1), 1);
                    intVal = int.Parse(strSub,
                    System.Globalization.NumberStyles.HexNumber) * 16 + int.Parse(strSub1,
                    System.Globalization.NumberStyles.HexNumber);
                    intMod = (intCounter / 2) % arrOffsets.Count;
                    intNewVal = intVal -
                    Convert.ToInt32(arrOffsets[intMod]) + 256;
                    intNewVal = intNewVal % 256;
                    strDecimal = ((char)intNewVal).ToString();
                    strOut = strOut + strDecimal;
                }
                return strOut.Replace(passPhrase,"");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}