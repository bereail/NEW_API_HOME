using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using toner_API.Model.DTO;
using toner_API.Models;

namespace toner_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargaController : ControllerBase
    {
        private readonly tonerStoreContext _dbContext;

        public CargaController(tonerStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("cargas")]
        public IActionResult CreateCarga([FromBody] CargaDTO cargaDTO)
        {
            try
            {
                // Verifica si la cargaDTO es nula
                if (cargaDTO == null)
                {
                    return BadRequest("Invalid payload. Carga data is required.");
                }

                // Busca el toner en la base de datos por su ID
                var toner = _dbContext.Toner.Find(cargaDTO.IdToner);
                if (toner == null)
                {
                    return BadRequest("Invalid Toner ID.");
                }

                // Verifica si el stock del toner es insuficiente para la carga solicitada
                if (toner.Stock < cargaDTO.Cant)
                {
                    return BadRequest("Insufficient stock.");
                }

                // Crea una nueva instancia de Carga
                var carga = new Carga
                {
                    IdUser = cargaDTO.IdUser,
                    IdToner = cargaDTO.IdToner,
                    IdService = cargaDTO.IdService,
                    CargaAt = DateTime.UtcNow, // Establece la fecha y hora actuales del servidor
                    Cant = cargaDTO.Cant
                };

                // Actualiza el stock del toner
                toner.Stock -= cargaDTO.Cant;

                // Agrega la carga a la base de datos
                _dbContext.Carga.Add(carga);

                // Guarda los cambios en la base de datos
                _dbContext.SaveChanges();

                // Devuelve una respuesta exitosa con los detalles de la carga creada
                return CreatedAtAction(nameof(CreateCarga), new { id = carga.Id }, carga);
            }
            catch (Exception ex)
            {
                // Captura y maneja cualquier excepción que ocurra durante la ejecución
                string innerErrorMessage = ex.InnerException?.Message;
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message, innerError = innerErrorMessage });
            }
        }

        // Puedes agregar aquí otras acciones para obtener, actualizar y eliminar cargas si es necesario
    }
}
