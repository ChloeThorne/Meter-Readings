using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Meter_Readings_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : Controller
    {
        /// <summary>
        /// Gets or sets the meter reading service.
        /// </summary>
        private IMeterReadingService meterReadingService { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="MeterReadingController"/>
        /// </summary>
        /// <param name="meterReadingService">An instance of <see cref="IMeterReadingService"/>.</param>
        public MeterReadingController(IMeterReadingService meterReadingService) 
        { 
            this.meterReadingService = meterReadingService;
        }

        /// <summary>
        /// Upload a meter reading file to be processed and added to the database.
        /// </summary>
        /// <param name="formFile">The form file containing the file to be uploaded.</param>
        /// <response code="200">Indicates that request has been processed successfully.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with <see cref="UploadMeterReadingsViewModel"/>.</returns>
        [HttpPost]
        [Route("meter-reading-uploads")]
        [Produces<UploadMeterReadingsViewModel>]
        public async Task<IActionResult> MeterReadingUploads(IFormFile formFile)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StreamReader streamReader = new StreamReader(formFile.OpenReadStream()))
                {
                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(streamReader.ReadLine());
                    }
                }

                UploadMeterReadingsViewModel vm = await meterReadingService.UploadFromCsv(stringBuilder.ToString());
                return Ok(vm);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Create a single meter reading in the database.
        /// </summary>
        /// <param name="meterReading">The meter reading to be inserted into the database.</param>
        /// <response code="204">Indicates that the entity was successfully inserted into the database.</response>
        /// <response code="400">Indicates that a validation error happened while inserting the entity into the database.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with no body.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateMeterReadingViewModel meterReadingViewModel)
        {
            try
            {
                MeterReading meterReading = new MeterReading(meterReadingViewModel.AccountId, meterReadingViewModel.MeterReadingDateTime, meterReadingViewModel.MeterReadValue);
                bool created = await meterReadingService.Create(meterReading);
                if(created)
                {
                    return Created();
                }

                return BadRequest();
            } 
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Gets all meter reading entities in the database.
        /// </summary>
        /// <response code="200">Indicates that the requested meter readings were found.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with <see cref="IEnumerable{MeterReading}"/>.</returns>
        [HttpGet]
        [Produces<IEnumerable<MeterReading>>]
        public IActionResult Get() 
        {
            try
            {
                List<MeterReading> meterReadings = meterReadingService.Get();
                return Ok(meterReadings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a single meter reading entities in the database.
        /// </summary>
        /// <response code="200">Indicates that the requested meter reading was found.</response>
        /// <response code="404">Indicates that the requested meter reading does not exist.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with <see cref="MeterReading"/>.</returns>
        [HttpGet("{id}")]
        [Produces<MeterReading>]
        public IActionResult Get(int id)
        {
            try
            {
                MeterReading? meterReading = meterReadingService.Get(id);
                if(meterReading != null)
                {
                    return Ok(meterReading);
                }

                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates a single entity in the database.
        /// </summary>
        /// <param name="meterReading">The meter reading entity to be updated.</param>
        /// <returns>A response with no body.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(MeterReading meterReading)
        {
            try
            {
                bool updated = await meterReadingService.Update(meterReading);
                if(updated)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an entity by ID from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A response with no body.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await meterReadingService.Delete(id);
                if(deleted)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}
