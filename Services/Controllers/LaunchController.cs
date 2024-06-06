using System.ComponentModel.DataAnnotations;
using Application.Wrappers;
using Cross.Cutting.Helper;
using Domain.Commands.Launch.Requests;
using Domain.Commands.Launch.Responses;
using Domain.Queries.Launch.Requests;
using Domain.Queries.Launch.Responses;
using Domain.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.Controllers
{
    [ApiController]
    [Route("api/launch")]
    public class LaunchController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LaunchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("search")]
        [SwaggerOperation(Summary = "Method for fuzzy search of mission, location, pad, rocket and launch.")]
        public async Task<IActionResult>SearchByParams([FromQuery]SearchLaunchRequest request)
        {
            try
            {
                _ = request ?? throw new ArgumentNullException(ErrorMessages.NullArgument);

                var wrapper = new MediatrRequestWrapper<SearchLaunchRequest, SeachByParamResponse>(request, null);
                var data = await _mediator.Send(wrapper);

                return Ok(data);
            }
            catch(ValidationException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpGet]
        [Route("{launchId:Guid}")]
        [SwaggerOperation(Summary = "Method for search a launch by his UUID. This UUID is proveniente from the database.")]
        public async Task<IActionResult> GetById([FromRoute]GetByIdRequest request)
        {
            try
            {
                var wrapper = new MediatrRequestWrapper<GetByIdRequest, GetOneLaunchResponse>(request, null);
                var data = await _mediator.Send(wrapper);

                return Ok(data);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpGet]
        [Route("paged")]
        [SwaggerOperation(Summary = "Method for return launches paged")]
        public async Task<IActionResult> GetAllPaged([FromQuery]PageRequest request)
        {
            try
            {
                var wrapper = new MediatrRequestWrapper<PageRequest, GetLaunchesPagedResponse>(request, null);
                var data = await _mediator.Send(wrapper);

                return Ok(data);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{launchId:Guid}")]
        [SwaggerOperation(Summary = "Method for delete a launch by his UUID.")]
        public async Task<IActionResult> Delete([FromRoute]SoftDeleteLaunchRequest request)
        {
            try
            {
                var wrapper = new MediatrRequestWrapper<SoftDeleteLaunchRequest, SoftDeleteLaunchResponse>(request, null);
                await _mediator.Send(wrapper);

                return Ok(SuccessMessages.DeletedEntity);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpPut]
        [Route("{launchId:Guid}")]
        [SwaggerOperation(Summary = "Method to update a launch by synchronize his data with ll.thespacedevs API.")]
        public async Task<IActionResult> Update([FromRoute]UpdateOneLaunchRequest request)
        {
            try
            {
                var wrapper = new MediatrRequestWrapper<UpdateOneLaunchRequest, UpdateOneLaunchResponse>(request, null);
                var updatedLaunch = await _mediator.Send(wrapper);

                return Ok(updatedLaunch);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, ex.Message);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpPost]
        [Route("launchers")]
        [SwaggerOperation(Summary = "Method to synchronize data with ll.thespacedevs API.", Description = "The offset query string is launch count starting point. It will bring up 100 to 100 max of 1500 new launches per request.")]
        public async Task<IActionResult> UpdateDataSet([FromQuery]UpdateLaunchSetRequest request)
        {
            try
            {
                var wrapper = new MediatrRequestWrapper<UpdateLaunchSetRequest, UpdateDataSetResponse>(request, null);
                var updated = await _mediator.Send(wrapper);

                if(updated.Success == false)
                    return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}");
                    
                return Ok(updated);
            }
            catch(ValidationException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status429TooManyRequests, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }
    }
}
