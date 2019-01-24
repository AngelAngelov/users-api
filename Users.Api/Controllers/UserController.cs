using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Core.Interfaces;
using Users.Core.Models;

namespace Users.Api.Controllers
{
    [DisableCors]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        /*
         * Controller does not allow CORS
         * Response is in JSON format
         * Route - /api/User/
         */

        private readonly IUserRepository _repo;
        public UserController(IUserRepository userRepository)
        {
            _repo = userRepository;
        }

        /*
         * GET
         * /User/{id} - get speciffic user by id
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            if (id != default(Guid))
            {
                User user = await _repo.Get(id);
                return Ok(user);
            }

            return BadRequest("Bad parameter");
        }

        /*
         * GET
         * /User/All - get all users
         */
        [HttpGet]
        [Route("[action]")]
        public IActionResult All()
        {
            List<User> users = _repo.All().ToList();
            return Ok(users);
        }

        /*
         * POST
         * /Users/ - create new user
         */
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserCreate user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User createdUser = await _repo.Add(user);
                    return Ok(createdUser);
                }
                catch (Exception)
                {
                    // Log exception ...

                    return BadRequest("Error creating new user");
                }
            }

            return BadRequest("Fill all required fields");
        }

        /*
         * PUT
         * /User/ - delete speciffic user by id
         */
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User createdUser = await _repo.Edit(user);
                    return Ok(createdUser);
                }
                catch (Exception)
                {
                    // Log exception ...

                    return BadRequest("Error creating new user");
                }
            }

            return BadRequest("Fill all required fields");
        }

        /*
         * DELETE
         * /User/{id} - edit existing user
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute]Guid id)
        {
            if (id != null)
            {
                try
                {
                    await _repo.Remove(id);
                    return Ok();
                }
                catch (Exception)
                {
                    // Log exception ...

                    return BadRequest("Error deleting user");
                }
            }

            return BadRequest("No user id");
        }
    }
}