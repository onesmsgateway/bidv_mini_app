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
        public static string GenerateSha256(string payload)   
        {
            var _key = AppConst.bidvSignSecretKey;
            var _payload = $"{_key}|{payload}";
            using (var _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key)))
            {
                var _hashBytes = _hmac.ComputeHash(Encoding.UTF8.GetBytes(_payload));
                var sb = new StringBuilder(_hashBytes.Length * 2);
                foreach (var b in _hashBytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static IList<string> SplitAndTrim(string str, char separator = ',')
        {
            if (string.IsNullOrWhiteSpace(str))
                return new List<string>();

            return str.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim())
                         .Where(s => !string.IsNullOrWhiteSpace(s))
                         .ToList();
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

    public static class StringExtensions
    {
        public static bool IsEqualIgnoreCase(this string? sourceStr, string? targetStr)
        {
            if (ReferenceEquals(sourceStr, targetStr))
            {
                return true;
            }
            if (sourceStr == null || targetStr == null)
            {
                return false;
            }
            return sourceStr.Equals(targetStr, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetValueOrDefault(this string? sourceStr)
        {
            return sourceStr ?? "";
        }


        public static int ParseToIntOrDefault(this string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return 0;
            }
            if (int.TryParse(source, out int result))
            {
                return result;
            }
            return 0;
        }

        public static bool ContainsIgnoreCase(this string? source, string value)
        {
            if (source == null || value == null)
            {
                return false;
            }
            if (value.Length == 0)
            {
                return true;
            }
            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    public static class ByteConverterExtensions
    {
        public static decimal ToGb(this int megabytes)
        {
            return Math.Round((decimal)(megabytes / 1024.0), 2, MidpointRounding.AwayFromZero);  ;
        }

        public static decimal ToGbPerDay(this int megabytes, decimal days)
        {
            return Math.Round(Math.Round((decimal)(megabytes / 1024.0), 2, MidpointRounding.AwayFromZero) / days, 2, MidpointRounding.AwayFromZero); 
        }
    }

    public class JwtUtils
    {
        public static JwtSecurityToken GetTokenInfo(string token)
        {
            SecurityToken validatedToken;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppConst.jwtKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(AppConst.partnerJwtExpired)
                }, out validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static async Task<string> GenerateToken(string customerId, string customerName, int hour_expired)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConst.jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[] {
                new Claim("id", customerId),
                new Claim("username", customerName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(null, null, claims, expires: DateTime.UtcNow.AddHours(hour_expired), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
