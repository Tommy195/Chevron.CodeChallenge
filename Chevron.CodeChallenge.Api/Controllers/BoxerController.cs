using Microsoft.AspNetCore.Mvc;
using Chevron.CodeChallenge.Models;
using Chevron.CodeChallenge.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Chevron.CodeChallege.Api.Controllers
{
    //I decided to comment this for testing purposes so we could test the CRUD
    //[Authorize]
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class BoxerController : ControllerBase
    {
        private readonly ILogger<BoxerController> _logger;
        private readonly IBoxerService _boxerService;

        public BoxerController(ILogger<BoxerController> logger,IBoxerService boxerService)
        {
            _logger = logger;
            _boxerService = boxerService;
        }

        /// <summary>
        /// Gets al boxers 
        /// </summary>
        /// <remarks>
        /// Possible errors:
        /// 
        ///     BadRequest: Empty BoxerDTO
        ///     InternalServerError: Error when persisting 
        /// 
        /// </remarks>
        /// <returns>returns Ok result and the boxers list</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<BoxerDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<BoxerDTO>>> Get()
        {
            try
            {
                _logger.LogInformation($"Getting all boxers.");

                var boxers = await _boxerService.GetAll();

                return Ok(boxers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting boxers. {ex.Message}");

                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not retrieve boxers");
            }
        }

        /// <summary>
        /// Gets boxer entity by id
        /// </summary>
        /// <remarks>
        /// Possible errors:
        ///     
        ///     BadRequest: Empty BoxerDTO
        ///     InternalServerError: Error when persisting 
        ///    
        /// </remarks>
        /// <param name="id">Boxer Id</param>
        /// <returns>returns OK result and found Boxer's dto</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BoxerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<BoxerDTO>> Get(Guid id)
        {
            try
            {
                _logger.LogInformation($"Getting boxer by id.");

                var product = await _boxerService.Get(id);

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting boxer by id. {ex.Message}");

                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not retrieve boxer");
            }
        }

        /// <summary>
        ///  Creates new Boxer
        /// </summary>
        /// <remarks>
        /// Possible errors:
        /// 
        ///     BadRequest: Empty EmployerDto
        ///     InternalServerError: Error when persisting 
        /// 
        /// </remarks>
        /// <param name="employerDto">EmployerDto object</param>
        /// <returns>returns Ok result and the created Employer's id</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BoxerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<BoxerDTO>> Post([FromQuery] BoxerDTO value)
        {
            try
            {
                if (value == null)
                {
                    _logger.LogInformation($"Invalid Input. Input: {value}");

                    return BadRequest("Invalid input.");
                }

                _logger.LogInformation($"Creating boxer.");

                var response = await _boxerService.Post(value);

                _logger.LogInformation($"Created boxer. Id: {response.Id}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating boxer. {ex.Message}");

                return StatusCode((int) HttpStatusCode.InternalServerError, "Could not create boxer");
            }
        }

        /// <summary>
        ///  Updates existing Boxer
        /// </summary>
        /// <remarks>
        /// Possible errors:
        /// 
        ///     BadRequest: Empty EmployerDto
        ///     InternalServerError: Error when persisting 
        /// 
        /// </remarks>
        /// <param name="id">EmployerDto object</param>
        /// <param name="value">EmployerDto object</param>
        /// <returns>returns Ok result and the updated Boxers's dto</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BoxerDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<BoxerDTO>> Put(Guid id, [FromQuery] BoxerDTO value)
        {
            try
            {
                if (value == null)
                {
                    _logger.LogInformation($"Invalid Input. Input: {value}");

                    return BadRequest("Invalid input.");
                }

                _logger.LogInformation($"Updating boxer. Boxer: {value.Id}");

                var response = await _boxerService.Put(id, value);

                _logger.LogInformation($"Updated boxer. Id: {response.Id}");

                return Ok(await _boxerService.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating boxer. {ex.Message}");

                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not update boxer");
            }
        }

        /// <summary>
        ///  Deletes existing Boxer
        /// </summary>
        /// <remarks>
        /// Possible errors:
        /// 
        ///     BadRequest: Empty EmployerDto
        ///     InternalServerError: Error when persisting 
        /// 
        /// </remarks>
        /// <param name="id">EmployerDto object</param>
        /// <returns>returns Ok result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null)
                {
                    _logger.LogInformation($"Invalid Input. Input: {id}");

                    return BadRequest("Invalid input.");
                }

                _logger.LogInformation($"Deleting boxer. Boxer Id: {id}");

                _boxerService.Delete(id);

                _logger.LogInformation($"Deleted boxer. Boxer Id: {id}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting boxer. {ex.Message}");

                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not delete boxer");
            }

        }
    }
}
