using Infra.EletronicVoteSystem;
using Infra.EletronicVoteSystem.Entities;
using Infra.EletronicVoteSystem.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Api.EletronicVoteSystem.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/Registry")]
    public class RegistryController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegistryController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] User newUser)
        {
            try
            {
                newUser.Password = Util.GenerateHash(newUser.Password);

                var created = _userRepository.Create(newUser);

                if (created)
                {

                    return StatusCode(201, new
                    {
                        newUser.Id,
                        email = newUser.Email
                    });
                }

                return StatusCode(500);

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}