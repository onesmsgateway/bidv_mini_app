using Jose;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;

namespace payment.api.Common
{
    public class Header
    {
        [JsonProperty("alg")]
        public string Alg { get; set; }

        [JsonProperty("enc")]
        public string Enc { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Recipient
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("encrypted_key")]
        public string EncryptedKey { get; set; }
    }

    public class Jwe
    {
        [JsonProperty("recipients")]
        public Recipient[] Recipients { get; set; }

        [JsonProperty("protected")]
        public string Protected { get; set; }

        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; }

        [JsonProperty("iv")]
        public string Iv { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }

    public class BidvJweRequestHelper
    {
        private static readonly string _path = $"{AppContext.BaseDirectory}\\Resource\\openssl\\";
        private static byte[] HexStrToBytes(string HexStr)
        {
            int length = HexStr.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexStr.Substring(i, 2), 16);
            }
            return bytes;
        }

        private static RSA LoadPrivateKeyFromPemFile(string filePath)
        {
            try
            {
                using (var reader = File.OpenText(filePath))
                {
                    var pemReader = new PemReader(reader);
                    var keyObject = pemReader.ReadObject();

                    RsaPrivateCrtKeyParameters rsaPrivateCrtKey;

                    if (keyObject is AsymmetricCipherKeyPair keyPair)
                    {
                        rsaPrivateCrtKey = (RsaPrivateCrtKeyParameters)keyPair.Private;
                    }
                    else if (keyObject is RsaPrivateCrtKeyParameters rsaParams)
                    {
                        rsaPrivateCrtKey = rsaParams;
                    }
                    else
                    {
                        throw new InvalidDataException("Tệp PEM không chứa khóa riêng tư hợp lệ (PKCS#1/PKCS#8).");
                    }

                    return DotNetUtilities.ToRSA(rsaPrivateCrtKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading private key: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        private static string GetSymmatricKey()
        {
            try
            {
                return File.ReadAllText(Path.Combine(_path, "symmatrickey.txt")).Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading symmetric key.");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///JWS:JsonWebEncryption
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>JWE String</returns>
        public static string GenJWE(string payload)
        {
            var symatricKey = GetSymmatricKey();
            var keyBytes = HexStrToBytes(symatricKey);
            // JWE Compact Serialization (5 phần: H.K.IV.C.T)
            var _parts = JWT.Encode(payload, keyBytes, JweAlgorithm.A256KW, // Key Management Algorithm (A256KW)
                JweEncryption.A128CBC_HS256,     // Content Encryption Algorithm (A128CBC-HS256)
                (JweCompression?)SerializationMode.Compact).Split('.');

            if (_parts.Length != 5)
            {
                throw new FormatException("JWE Compact Serialization wrong format.");
            }

            var _recipient = new Recipient
            {
                Header = new Header(),  // Tương ứng với "header"
                EncryptedKey = _parts[1] // Encrypted Key
            };

            return  new Jwe
            {
                Protected = _parts[0],          // Protected Header (phần 0)
                Recipients = new[] { _recipient }, // Mảng Recipients đã tạo
                Ciphertext = _parts[3],         // Ciphertext (phần 3)
                Iv = _parts[2],                 // Initialization Vector (phần 2)
                Tag = _parts[4]                 // Authentication Tag (phần 4)
            }.ToString();
        }

        /// <summary>
        /// JWS:JsonWebEncryption
        /// </summary>
        /// <returns>DetachedJwsString</returns>
        public static string SignJWS(string jweString)
        {
            try
            {
                var _privateKeyPath = Path.Combine(_path, "key.pem");
                RSA rSA = LoadPrivateKeyFromPemFile(_privateKeyPath);
                var jwsFull = JWT.Encode(jweString, rSA, JwsAlgorithm.RS256);

                var jwsArr = jwsFull.Split('.');
                if (jwsArr.Length != 3) throw new Exception("JWS Compact Serialization format is invalid.");

                // Lấy phần Header (0) và Signature (2)
                return (string)jwsArr[0].Trim().Concat("..").Concat(jwsArr[2].Trim());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating Detached JWS: {e.Message}");
               return string.Empty;
            }
        }

        public static string SignJWSfrom(string payload)
        {
            return SignJWS(GenJWE(payload));
        } 
    }
}
