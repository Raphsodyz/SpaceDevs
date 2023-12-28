using Business.DTO;
using AutoMapper;
using Business.Interface;
using Cross.Cutting.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModel;

namespace Services.Controllers
{
    [ApiController]
    [Route("api")]
    public class LaunchController : ControllerBase
    {
        private readonly ILaunchApiBusiness _launchApiBusiness;
        private readonly IMapper _mapper;
        public LaunchController(ILaunchApiBusiness launchApiBusiness,IMapper mapper)
        {
            _launchApiBusiness = launchApiBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("launchers")]
        public async Task<IActionResult> SearchByParams([FromQuery]SearchLaunchViewModel search)
        {
            try
            {
                _ = search ?? throw new ArgumentNullException(ErrorMessages.NullArgument);
                var registers = await _launchApiBusiness.SearchByParam(_mapper.Map<SearchLaunchDTO>(search));
                var data = _mapper.Map<List<LaunchViewModel>>(registers);

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
        [Route("launchers/{launchId}")]
        public async Task<IActionResult> GetById(Guid? launchId)
        {
            try
            {
                var launchDTO = await _launchApiBusiness.GetOneLaunch(launchId);
                var viewModel = _mapper.Map<LaunchViewModel>(launchDTO);

                return Ok(viewModel);
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
        [Route("launchers/paged")]
        public async Task<IActionResult> GetAllPaged(int page)
        {
            try
            {
                Pagination<LaunchDTO> pagedLaunchList = await _launchApiBusiness.GetAllLaunchPaged(page);
                Pagination<LaunchViewModel> pagedLaunchListViewModel = _mapper.Map<Pagination<LaunchViewModel>>(pagedLaunchList);

                return Ok(new { CurrentlyPage = pagedLaunchListViewModel.CurrentPage, TotalRegisters = pagedLaunchListViewModel.NumberOfEntities, Pages = pagedLaunchListViewModel.NumberOfPages, Data = pagedLaunchListViewModel.Entities });
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
        [Route("launchers/{launchId}")]
        public async Task<IActionResult> Delete(Guid? launchId)
        {
            try
            {
                await _launchApiBusiness.SoftDeleteLaunch(launchId);
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
        [Route("launchers/{launchId}")]
        public async Task<IActionResult> Edit(Guid? launchId)
        {
            try
            {
                LaunchDTO updatedLaunch = await _launchApiBusiness.UpdateLaunch(launchId);
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
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }

        [HttpPost]
        [Route("launchers")]
        public async Task<IActionResult> UpdateData(int? skip)
        {
            try
            {
                bool updated = await _launchApiBusiness.UpdateDataSet(skip);
                if (updated)
                    return Ok(SuccessMessages.ImportedDataSuccess);
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ErrorMessages.InternalServerError}\n{ex.Message}");
            }
        }
    }
}
