using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using SCCB.Core.Settings;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SCCB.Core.Helpers
{
    public class PasswordProcessor
    {
        private readonly HashGenerationSetting _hashGenerationSetting;

        public PasswordProcessor(HashGenerationSetting hashGenerationSetting)
        {
            _hashGenerationSetting = hashGenerationSetting ?? throw new ArgumentException(nameof(hashGenerationSetting));
        }

        /// <summary>
        /// Get password hash.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>Password hash</returns>
        public string GetPasswordHash(string password)
        {
            var saltBytes = Convert.FromBase64String(_hashGenerationSetting.Salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: _hashGenerationSetting.IterationCount,
                numBytesRequested: _hashGenerationSetting.BytesNumber));

            return hashed;
        }
    }
}
