using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace Meter_Readings_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterReadingController : Controller
    {
        private IMeterReadingService _meterReadingService { get; set; }
        public MeterReadingController(IMeterReadingService meterReadingService) 
        { 
            _meterReadingService = meterReadingService;
        }

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

                UploadMeterReadingsViewModel vm = await _meterReadingService.UploadFromCsv(stringBuilder.ToString());
                return Ok(vm);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(MeterReading meterReading)
        {
            try
            {
                bool created = await _meterReadingService.Create(meterReading);
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

        [HttpGet]
        public IActionResult Get() 
        {
            try
            {
                List<MeterReading> meterReadings = _meterReadingService.Get();
                return Ok(meterReadings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                MeterReading? meterReading = _meterReadingService.Get(id);
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

        [HttpPut]
        public async Task<IActionResult> Update(MeterReading meterReading)
        {
            try
            {
                bool updated = await _meterReadingService.Update(meterReading);
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

        [HttpDelete("/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _meterReadingService.Delete(id);
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
