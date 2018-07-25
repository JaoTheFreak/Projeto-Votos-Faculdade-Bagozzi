using Api.EletronicVoteSystem.Auth;
using Infra.EletronicVoteSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

// USAR CONTROLLER COMO EXEMPLO

namespace Api.EletronicVoteSystem.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly EVSContext _dbContext;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly SigningConfiguration _signingConfiguration;

        public ValuesController(EVSContext dbContext, 
            TokenConfiguration tokenConfiguration, 
            SigningConfiguration signingConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
            _signingConfiguration = signingConfiguration;
        }


        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(string email)
        {
            if (email != null)
            {
                var usuarioBase = _dbContext.Users.FirstOrDefault(user => user.Email.Equals(email));

                var credenciaisValidas = (usuarioBase != null);

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuarioBase.Id.ToString(), "Login"),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, usuarioBase.Id.ToString())
                        }
                    );

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao.Add(TimeSpan.FromSeconds(_tokenConfiguration.Seconds));

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

            return NotFound();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize("Bearer")]
        public IActionResult Get(int id)
        {
            return Ok(new
            {
                message = $"Bem Vindo Usuário {id}"
            });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
