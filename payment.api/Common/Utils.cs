using Microsoft.IdentityModel.Tokens;
using payment.api.AppSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace payment.api.Common
{
    public class Utils
    {
        public static string GenerateSha256(string timestamp, string userId, string service, string data, string language)
        {
            var _key = AppConst.bidvSignSecretKey;
            var _payload = $"{_key}|{timestamp}|{userId}|{service}|{data}|{language}";
            using (var _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key)))
            {
                var _hashBytes = _hmac.ComputeHash(Encoding.UTF8.GetBytes(_payload));
                var sb = new StringBuilder(_hashBytes.Length * 2);
                foreach (var b in _hashBytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string GenerateSha256(string billnumber, string serviceId)
        {
            var _key = AppConst.bidvSignSecretKey;
            var _payload = $"{_key}|{serviceId}|{billnumber}";
            using (var _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key)))
            {
                var _hashBytes = _hmac.ComputeHash(Encoding.UTF8.GetBytes(_payload));
                var sb = new StringBuilder(_hashBytes.Length * 2);
                foreach (var b in _hashBytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string EncryptAES(string jsonPayload, string base64Key, string base64IV)
        {
            byte[] keyBytes = Convert.FromBase64String(base64Key);
            byte[] ivBytes = Convert.FromBase64String(base64IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(jsonBytes, 0, jsonBytes.Length);
                byte[] combined = new byte[ivBytes.Length + encryptedBytes.Length];
                Buffer.BlockCopy(ivBytes, 0, combined, 0, ivBytes.Length);
                Buffer.BlockCopy(encryptedBytes, 0, combined, ivBytes.Length, encryptedBytes.Length);

                return Convert.ToBase64String(combined);
            }
        }

        public static string GenerateRandomIV()
        {
            byte[] iv = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return Convert.ToBase64String(iv);
        }

        public static string SHA256withRSA(string a, string d, string n, string p)
        {
            string data = string.Format("{0}|{1}|{2}|{3}", a, d, n, p);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] keyBytes = Convert.FromBase64String(AppConst.partnerPrivateKeyRsa);

            using (RSA rsa = RSA.Create())
            {
                try
                {
                    rsa.ImportPkcs8PrivateKey(keyBytes, out _);
                }
                catch (CryptographicException ex)
                {
                    throw new Exception("Lỗi khi nhập Private Key PKCS#8. Hãy đảm bảo khóa là PKCS#8 hợp lệ.", ex);
                }

                byte[] signedBytes = rsa.SignData(
                    dataBytes,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                return Convert.ToBase64String(signedBytes);
            }
        }

        public static string dEnscryptedData(string jsonPayload)
        {
            var _generateRandomIV = GenerateRandomIV();
            byte[] ivBytes = Convert.FromBase64String(_generateRandomIV);
            byte[] keyBytes = Convert.FromBase64String(EncryptAES(jsonPayload, AppConst.bidvSecretKeyAres256, _generateRandomIV));

            byte[] combined = new byte[ivBytes.Length + keyBytes.Length];
            Buffer.BlockCopy(ivBytes, 0, combined, 0, ivBytes.Length);
            Buffer.BlockCopy(keyBytes, 0, combined, ivBytes.Length, keyBytes.Length);

            return Convert.ToBase64String(combined);
        }

        public static string GenerateRandomDigitWith(int length)
        {
            string _tickBase = DateTime.UtcNow.Ticks.ToString();
            string _timePart;
            string _randomPart;
            var _rng = new Random();

            if (_tickBase.Length >= 8)
            {
                _timePart = _tickBase.Substring(_tickBase.Length - 8);
            }
            else
            {
                _timePart = _tickBase.PadLeft(8, '0');
            }
            lock (_rng)
            {
                _randomPart = _rng.Next(1000, 10000).ToString("D4");
            }
            return $"{_timePart}{_randomPart}";
        }

        public static string Base64UrlEncode(byte[] input)
        {
            string output = Convert.ToBase64String(input);
            output = output.Replace("=", "");
            output = output.Replace("+", "-");
            output = output.Replace("/", "_");
            return output;
        }
    }
}
