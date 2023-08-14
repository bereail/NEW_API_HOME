using Microsoft.AspNetCore.Mvc;
using toner_API.Model.DTO;
using toner_API.Models;

namespace toner_API.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("api/[controller]")] // Define la ruta base para las acciones del controlador (en este caso, "/api/Toner")
    public class TonerController : ControllerBase
    {
        private readonly tonerStoreContext _dbContext;

        // Constructor del controlador que recibe una instancia de DbContext
        public TonerController(tonerStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("toners")] // Define la ruta para la acción GetToners (GET /api/Toner/toners)
        public IActionResult GetToners()
        {
            try
            {
                // Consulta la base de datos para obtener todos los toners y proyecta los resultados en objetos TonerDTO
                var toners = _dbContext.Toner
                    .Select(t => new TonerDTO { Id = t.Id, Name = t.Name, Cant = t.Cant, Stock = t.Stock })
                    .ToList();

                if (toners.Any()) // Verifica si hay toners en la lista
                {
                    return Ok(toners); // Devuelve una respuesta exitosa con la lista de toners en formato JSON
                }

                return Ok("No toners found in the database"); // Si no hay toners, devuelve un mensaje
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la ejecución
            { 
            return BadRequest(ex.Message); // Devuelve una respuesta de error 400 con el mensaje de la excepción
            }
        }


        [HttpPost("toners")] // Define la ruta para la acción CreateToner (POST /api/Toner/toners)
        public IActionResult CreateToner([FromBody] TonerDTO tonerDTO)
        {
            try
            {
                // Verifica si el nombre del toner es nulo o vacío, o si la cantidad es menor o igual a cero
                if (string.IsNullOrEmpty(tonerDTO.Name) || tonerDTO.Cant <= 0)
                {
                    return BadRequest("Invalid toner data."); // Devuelve un error 400 si los datos del toner son inválidos
                }

                // Crea una nueva instancia del modelo Toner usando los datos del DTO
                var toner = new Toner
                {
                    Name = tonerDTO.Name,
                    Cant = tonerDTO.Cant,
                };

                _dbContext.Toner.Add(toner); // Agrega el nuevo toner a la base de datos
                _dbContext.SaveChanges(); // Guarda los cambios en la base de datos

                return Ok("Toner created successfully"); // Devuelve una respuesta exitosa con un mensaje de éxito
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la ejecución
            {
                return BadRequest(ex.Message); // Devuelve una respuesta de error 400 con el mensaje de la excepción
            }
        }

        [HttpGet("toners/{id}")]
        public IActionResult GetTonerById(int id)
        {
            try
            {
                var toner = _dbContext.Toner.Find(id);
                if (toner == null) 
                {
                    return NotFound("Toner not found");
                }
                var tonerDTO = new TonerDTO
                {
                    Id = toner.Id,
                    Name = toner.Name,
                    Stock = toner.Stock,
                };
                return Ok(tonerDTO);
        }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("toners/{id}")] // Define la ruta para la acción UpdateToner (PUT /api/Toner/toners/{id})
        public IActionResult UpdateToner(int id, [FromBody] TonerDTO updatedTonerDto)
        {
            try
            {
                // Busca un toner en la base de datos por su ID
                var toner = _dbContext.Toner.Find(id);

                if (toner == null)
                {
                    return NotFound("Toner not found"); // Devuelve un error 404 si no se encuentra el toner
                }

                // Verifica si el nombre del toner actualizado es nulo o vacío, o si el stock es menor o igual a cero
                if (string.IsNullOrEmpty(updatedTonerDto.Name) || updatedTonerDto.Stock <= 0)
                {
                    return BadRequest("Invalid toner data."); // Devuelve un error 400 si los datos del toner son inválidos
                }

                // Actualiza el nombre y el stock del toner con los datos del DTO actualizado
                toner.Name = updatedTonerDto.Name;
                toner.Stock = updatedTonerDto.Stock;

                _dbContext.SaveChanges(); // Guarda los cambios en la base de datos

                return Ok("Toner updated successfully."); // Devuelve una respuesta exitosa con un mensaje de éxito
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la ejecución
            {
                return BadRequest(ex.Message); // Devuelve una respuesta de error 400 con el mensaje de la excepción
            }
        }



        [HttpDelete("toners/{id}")] // Define la ruta para la acción DeleteToner (DELETE /api/Toner/toners/{id})
        public IActionResult DeleteToner(int id)
        {
            try
            {
                // Busca un toner en la base de datos por su ID
                var toner = _dbContext.Toner.Find(id);

                if (toner == null)
                {
                    return NotFound("Toner not found"); // Devuelve un error 404 si no se encuentra el toner
                }

                // Remueve el toner de la base de datos
                _dbContext.Toner.Remove(toner);
                _dbContext.SaveChanges(); // Guarda los cambios en la base de datos

                return Ok("Toner deleted successfully"); // Devuelve una respuesta exitosa con un mensaje de éxito
            }
            catch (Exception ex) // Captura cualquier excepción que ocurra durante la ejecución
            {
                return BadRequest($"An error occurred while deleting the toner: {ex.Message}"); // Devuelve una respuesta de error 400 con el mensaje de la excepción
            }
        }

    }
}
