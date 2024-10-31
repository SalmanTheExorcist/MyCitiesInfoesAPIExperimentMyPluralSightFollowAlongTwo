using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCitiesInfo.API.Models;
using MyCitiesInfo.API.Services;
using System.Text.Json;

namespace MyCitiesInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/mycitiesinfoes")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class MyCitiesInfoesController : ControllerBase
    {
       
        private readonly IMyCitiesInfoesRepository _myCitiesInfoesRepository;
        private readonly IMapper _mapper;

        private const int MAX_MYCITIES_PAGE_SIZE = 20;

        public MyCitiesInfoesController(IMyCitiesInfoesRepository myCitiesInfoesRepository,
                                        IMapper mapper)
        {
            _myCitiesInfoesRepository = myCitiesInfoesRepository ??
                                        throw new ArgumentNullException(nameof(myCitiesInfoesRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        //-----------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyCitiesInfoWithoutPointsOfInterestsDTO>>> 
                          GetMyCitiesInfoes([FromQuery(Name ="filteronname")]string? nameFilter,
                                            [FromQuery(Name ="searchquery")]string? searchQuery,
                                            [FromQuery(Name = "pagenumber")] int pageNumber = 1,
                                            [FromQuery(Name = "pagesize")] int pageSize= 10)
        {

            if(pageSize > MAX_MYCITIES_PAGE_SIZE)
            {
                pageSize = MAX_MYCITIES_PAGE_SIZE;
            }


            var (myCitiesFromDb, myPaginationMetadata) = await _myCitiesInfoesRepository
                                                                .GetCitiesAsync(nameFilter, 
                                                                                searchQuery,
                                                                                pageNumber,
                                                                                pageSize);

            Response.Headers.Append("X-Pagination",
                                JsonSerializer.Serialize(myPaginationMetadata));

            return Ok(_mapper.Map<IEnumerable<MyCitiesInfoWithoutPointsOfInterestsDTO>>(myCitiesFromDb));


        }

        //-----------------------------------------------

        //[HttpGet("{id:int}")]
        //public JsonResult GetSingleMyCitiesInfoes(int id)
        //{
        //    return new JsonResult(
        //        MyCitiesInfoesDataStore.Current.MyCitiesInfoes
        //                .FirstOrDefault(c => c.Id == id)
        //        );
        //}
        //-----------------------------------------------
     
       /// <summary>
       /// Get a single MyCity by Id
       /// </summary>
       /// <param name="id">The Id of the MyCity we are getting.</param>
       /// <param name="includePointsOfInterests">Whether or not we include Points-Of-Interests for the MyCity we are getting.</param>
       /// <returns>A MyCity with or without its Points-of-Interests</returns>
       /// <response code="200">Returns the requested MyCity</response>

        [HttpGet("{mycitiesinfoesId:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async  Task<IActionResult> GetSingleMyCitiesInfoes(int mycitiesinfoesId,
                                                             bool includePointsOfInterests = false)
        {

            var mySingleCityFromDb = await _myCitiesInfoesRepository.GetSingleCityAsync(mycitiesinfoesId, includePointsOfInterests);

            if(mySingleCityFromDb == null)
            {
                return NotFound();
            }

            if (includePointsOfInterests)
            {
                return Ok(_mapper.Map<MyCitiesInfoDTO>(mySingleCityFromDb));
            }


            return Ok(_mapper.Map<MyCitiesInfoWithoutPointsOfInterestsDTO>(mySingleCityFromDb));

        }

      

        //-----------------------------------------------

    }//---End-Class
}//--End-Namespace
