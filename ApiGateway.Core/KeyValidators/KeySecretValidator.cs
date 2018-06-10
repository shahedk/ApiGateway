using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Core.KeyValidators
{
    public class KeySecretValidator : IKeyValidator
    {
        private readonly IKeyData _keyData;
        private readonly IStringLocalizer<KeySecretValidator> _localizer;
        private readonly ILogger<KeySecretValidator> _logger;

        public KeySecretValidator(IKeyData keyData, IStringLocalizer<KeySecretValidator> localizer, ILogger<KeySecretValidator> logger)
        {
            _keyData = keyData;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<KeyValidationResult> IsValid(string ownerKeyId, string pubKey, string secret)
        {
            var result = new KeyValidationResult();

            if (string.IsNullOrWhiteSpace(pubKey) || string.IsNullOrWhiteSpace(secret))
            {
                result.IsValid = false;
                result.Message = _localizer["Client key or secret cannot be blank"];
            }
            else
            {
                var key = await _keyData.GetByPublicKey(pubKey);

                if (key == null || key.Properties[ApiKeyPropertyNames.ClientSecret] != secret)
                {
                    result.IsValid = false;
                    result.Message = _localizer["Key and secrect does not match"];
                }
                else
                {
                    var log = _localizer["Login successful for: "] + pubKey;
                    _logger.LogInformation(LogEvents.LoginSuccess, log);

                    result.IsValid = true;
                }
            }

            return result;
        }
    }
}