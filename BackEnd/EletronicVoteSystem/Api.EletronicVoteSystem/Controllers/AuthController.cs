using Api.EletronicVoteSystem.Auth;
using Infra.EletronicVoteSystem;
using Infra.EletronicVoteSystem.Entities;
using Infra.EletronicVoteSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Api.EletronicVoteSystem.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly SigningConfiguration _signingConfiguration;

        public AuthController(IUserRepository userRepository,
            TokenConfiguration tokenConfiguration,
            SigningConfiguration signingConfiguration)
        {
            _userRepository = userRepository;
            _tokenConfiguration = tokenConfiguration;
            _signingConfiguration = signingConfiguration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] User userToAuth)
        {
            userToAuth.Password = Util.GenerateHash(userToAuth.Password);

            var user = _userRepository.ValidateUser(userToAuth);

            if (user == null)
                return NotFound();

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Id.ToString(), "Login"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
                }
            );

            var dataCriacao = DateTime.Now;

            var dataExpiracao = dataCriacao.Add(TimeSpan.FromSeconds(_tokenConfiguration.Seconds));

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            var token = handler.WriteToken(securityToken);

            return Ok(new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token
            });

        }
    }
}