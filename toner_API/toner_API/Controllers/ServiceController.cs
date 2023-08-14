using Microsoft.AspNetCore.Mvc;
using toner_API.Model.DTO;
using toner_API.Models;
using System;
using System.Linq;

namespace toner_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : BaseController // Hereda de BaseController
    {
        public ServiceController(tonerStoreContext dbContext) : base(dbContext)
        {
        }

        [HttpGet("service")]
        public IActionResult GetServices()
        {
            try
            {
                var listServices = _dbContext.Service
                    .Select(t => new ServiceDTO { Id = (int)t.Id, Name = t.Name })
                    .ToList();

                if (listServices != null && listServices.Any())
                {
                    return Ok(listServices);
                }

                return Ok("No Service in the database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("service")]
        public IActionResult CreateService([FromBody] ServiceDTO serviceDto)
        {
            try
            {
                // Validar que los datos recibidos sean correctos, por ejemplo:
                if (string.IsNullOrEmpty(serviceDto.Name))
                {
                    return BadRequest("Invalid service name.");
                }

                // Crear un objeto Service a partir de los datos del ServiceDTO
                var service = new Service
                {
                    Name = serviceDto.Name
                };

                // Agregar el nuevo servicio a la base de datos y guardar los cambios
                _dbContext.Service.Add(service);
                _dbContext.SaveChanges();

                // Devolver una respuesta indicando que la operación se realizó con éxito
                return Ok("Service created successfully.");
            }
            catch (Exception ex)
            {
                // En caso de error, devolver un mensaje con el detalle del error.
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("service/{id}")]
        public IActionResult GetServiceById(int id)
        {
            try
            {
                var service = _dbContext.Service.Find(id);

                if (service == null)
                {
                    return NotFound("Service not found");
                }

                var serviceDto = new ServiceDTO
                {
                    Id = (int)service.Id,
                    Name = service.Name
                };

                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("service/{id}")]
        public IActionResult UpdateService(int id, [FromBody] ServiceDTO updatedServiceDto)
        {
            try
            {
                var service = _dbContext.Service.Find(id);

                if (service == null)
                {
                    return NotFound("Service not found");
                }

                // Validar que los datos recibidos sean correctos, por ejemplo:
                if (string.IsNullOrEmpty(updatedServiceDto.Name))
                {
                    return BadRequest("Invalid service name.");
                }

                // Actualizar los datos del servicio
                service.Name = updatedServiceDto.Name;

                _dbContext.SaveChanges();

                return Ok("Service updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("service/{id}")]
        public IActionResult DeleteService(int id)
        {
            try
            {
                var service = _dbContext.Service.Find(id);

                if (service == null)
                {
                    return NotFound("Service not found");
                }

                _dbContext.Service.Remove(service);
                _dbContext.SaveChanges();

                return Ok("Service deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
