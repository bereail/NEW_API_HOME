using Microsoft.AspNetCore.Mvc;
using toner_API.Model.DTO;
using toner_API.Models;

namespace toner_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(tonerStoreContext dbContext) : base(dbContext)
        {
        }

        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            try
            {
                var listUsers = _dbContext.Users
                    .Select(t => new UsersDTO { Id = t.Id, Name = t.Name, IdRol = t.IdRol })
                    .ToList();

                if (listUsers != null && listUsers.Any())
                {
                    return Ok(listUsers);
                }

                return NotFound("No users found in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("users")]
        public IActionResult CreateUser([FromBody] UsersDTO usersDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(usersDTO.Name))
                {
                    return BadRequest("Invalid user name");
                }

                var user = new Users
                {
                    Name = usersDTO.Name,
                    Pass = usersDTO.Pass
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return CreatedAtAction(nameof(GetUsers), null, "User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _dbContext.Users.Find(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UsersDTO updatedUserDTO)
        {
            try
            {
                var user = _dbContext.Users.Find(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Validar que los datos recibidos sean correctos, por ejemplo:
                if (string.IsNullOrEmpty(updatedUserDTO.Name))
                {
                    return BadRequest("Invalid user name");
                }

                // Actualizar los datos del usuario
                user.Name = updatedUserDTO.Name;
                user.Pass = updatedUserDTO.Pass;

                _dbContext.SaveChanges();

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
