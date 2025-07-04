using System;
using Microsoft.Extensions.Configuration;

namespace Pandora.Infrastructure.Security
{
    public static class AESEnvironmentHelper
    {
        public static string GetAesKey(IConfiguration configuration)
        {
            // Öncelik: appsettings.json'daki TestAesKey
            var key = configuration["TestAesKey"];
            if (!string.IsNullOrEmpty(key))
                return key;

            // Gelişmiş: environment variable veya dosya
            var keyFile = Environment.GetEnvironmentVariable("PANDORA_AES_KEY_FILE");
            if (!string.IsNullOrEmpty(keyFile) && System.IO.File.Exists(keyFile))
                return System.IO.File.ReadAllText(keyFile).Trim();

            var envKey = Environment.GetEnvironmentVariable("PANDORA_AES_KEY");
            if (!string.IsNullOrEmpty(envKey))
                return envKey;

            throw new InvalidOperationException("AES key is not set. Please set TestAesKey in appsettings.json or use PANDORA_AES_KEY(_FILE).");
        }
    }
}