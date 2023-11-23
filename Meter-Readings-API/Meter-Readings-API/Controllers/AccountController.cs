using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Meter_Readings_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// Gets or sets the account service.
        /// </summary>
        private IAccountService accountService { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="AccountController"/>
        /// </summary>
        /// <param name="accountService">An instance of <see cref="IAccountService"/>.</param>
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Create a single account in the database.
        /// </summary>
        /// <param name="account">The account to be inserted into the database.</param>
        /// <response code="204">Indicates that the entity was successfully inserted into the database.</response>
        /// <response code="400">Indicates that a validation error happened while inserting the entity into the database.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with no body.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Account account)
        {
            try
            {
                bool created = await accountService.Create(account);
                if (created)
                {
                    return Created();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Gets all account entities in the database.
        /// </summary>
        /// <response code="200">Indicates that the requested accounts were found.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with <see cref="IEnumerable{Account}"/>.</returns>
        [HttpGet]
        [Produces<IEnumerable<Account>>]
        public IActionResult Get()
        {
            try
            {
                List<Account> accounts = accountService.Get();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get a single account entities in the database.
        /// </summary>
        /// <response code="200">Indicates that the requested account was found.</response>
        /// <response code="404">Indicates that the requested account does not exist.</response>
        /// <response code="500">Indicates that request an error occurred on the server while processing the request.</response>
        /// <returns>A response with <see cref="Account"/>.</returns>
        [HttpGet("{id}")]
        [Produces<Account>]
        public IActionResult Get(int id)
        {
            try
            {
                Account? account = accountService.Get(id);
                if (account != null)
                {
                    return Ok(account);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates a single entity in the database.
        /// </summary>
        /// <param name="account">The account entity to be updated.</param>
        /// <returns>A response with no body.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(Account account)
        {
            try
            {
                bool updated = await accountService.Update(account);
                if (updated)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
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
                bool deleted = await accountService.Delete(id);
                if (deleted)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
