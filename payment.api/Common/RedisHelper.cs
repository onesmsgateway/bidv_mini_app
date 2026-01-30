using Microsoft.EntityFrameworkCore.Storage;
using payment.api.AppSettings;
using StackExchange.Redis;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace PaymentPackageTelco.api.Common
{
    public class RedisHelper
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ConnectionMultiplexer Connection => LazyConnection.Value;

        private static IDatabase dbRedis => Connection.GetDatabase();

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            configurationOptions.EndPoints.Add(AppConst.REDIS_HOST, AppConst.REDIS_PORT);
            configurationOptions.Password = AppConst.REDIS_PASS;
            configurationOptions.AllowAdmin = true;
            configurationOptions.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        private static bool CheckConnectionRedis()
        {
            bool isConnected = Connection.IsConnected;
            if (!isConnected) logger.Error("CheckConnectionRedis" + "Not connection!" + AppConst.REDIS_HOST + AppConst.REDIS_PORT + AppConst.REDIS_PASS);
            return isConnected;
        }

        public static bool Set(string key, string value)
        {
            if (CheckConnectionRedis()) return dbRedis.StringSet(key, value);
            return false;
        }

        public static bool Set(string key, string value, int expireMinutes = 60)
        {
            var _timeSpace = TimeSpan.FromMinutes(expireMinutes);
            if (CheckConnectionRedis()) return dbRedis.StringSet(key, value, _timeSpace);
            return false;
        }

        public static bool Set<T>(string key, T value, int expireMinutes = 60)
        {
            string _jsonValue = JsonSerializer.Serialize(value);
            var _timeSpace = TimeSpan.FromMinutes(expireMinutes);
            if (CheckConnectionRedis()) return dbRedis.StringSet(key, _jsonValue, _timeSpace);
            return false;
        }

        public static string Get(string key)
        {
            if (CheckConnectionRedis())
            {
                string value = dbRedis.StringGet(key);
                return String.IsNullOrEmpty(value) ? String.Empty : value;
            }
            return String.Empty;
        }

        public static T? Get<T>(string key)
        {
            if (CheckConnectionRedis())
            {
                string _value = dbRedis.StringGet(key);
                return string.IsNullOrEmpty(_value) ? default(T) : JsonSerializer.Deserialize<T>(_value);
            }
            return default(T);
        }

        public static bool KeyExpire(string key, DateTime timeSpan)
        {
            if (CheckConnectionRedis())
            {
                return dbRedis.KeyExpire(key, timeSpan);
            }
            return false;
        }
    }
}
