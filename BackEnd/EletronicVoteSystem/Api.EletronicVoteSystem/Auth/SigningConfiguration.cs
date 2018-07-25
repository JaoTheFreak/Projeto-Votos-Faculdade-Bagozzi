using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Api.EletronicVoteSystem.Auth
{
    public class SigningConfiguration
    {
        public SecurityKey Key { get; }

        public SigningCredentials SigningCredentials { get; }

        public SigningConfiguration()
        {
            using (var providerRsa = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(providerRsa.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                Key,
                SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
