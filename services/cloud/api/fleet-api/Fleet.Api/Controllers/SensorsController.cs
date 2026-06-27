using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Sensor;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Controllers
{
    /// <summary>
    /// API endpoints for managing sensors.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SensorsController(ISensorService sensorService) : ControllerBase
    {
        /// <summary>
        /// Gets all sensors.
        /// </summary>
        /// <returns>A list of all sensors.</returns>
        /// <response code="200">Returns the list of sensors.</response>
        // GET: api/v1/Sensors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorDto>>> GetSensors()
        {
            var sensors = await sensorService.GetSensorsAsync();
            return Ok(sensors);
        }

        /// <summary>
        /// Gets a specific sensor by ID.
        /// </summary>
        /// <param name="id">The sensor ID.</param>
        /// <returns>The requested sensor.</returns>
        /// <response code="200">Returns the sensor.</response>
        /// <response code="404">If the sensor is not found.</response>
        // GET: api/v1/Sensors/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SensorDto>> GetSensor(Guid id)
        {
            var sensor = await sensorService.GetSensorAsync(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        /// <summary>
        /// Creates a new sensor.
        /// </summary>
        /// <param name="createSensorDto">The sensor data to create.</param>
        /// <returns>The created sensor.</returns>
        /// <response code="201">Returns the newly created sensor.</response>
        /// <response code="400">If the request data is invalid or referenced entities are not found.</response>
        // POST: api/v1/Sensors
        [HttpPost]
        public async Task<ActionResult<SensorDto>> PostSensor(CreateSensorDto createSensorDto)
        {
            try
            {
                var sensor = await sensorService.CreateSensorAsync(createSensorDto);
                return CreatedAtAction(nameof(GetSensor), new { id = sensor.Id }, sensor);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing sensor.
        /// </summary>
        /// <param name="id">The sensor ID to update.</param>
        /// <param name="sensorDto">The updated sensor data.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the sensor was successfully updated.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="404">If the sensor is not found.</response>
        // PUT: api/v1/Sensors/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutSensor(Guid id, SensorDto sensorDto)
        {
            if (id != sensorDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await sensorService.UpdateSensorAsync(id, sensorDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a sensor.
        /// </summary>
        /// <param name="id">The sensor ID to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the sensor was successfully deleted.</response>
        /// <response code="404">If the sensor is not found.</response>
        // DELETE: api/v1/Sensors/{id}
        [HttpDelete("{id:guid}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteSensor(Guid id)
        {
            return Ok();
        }
    }
}
