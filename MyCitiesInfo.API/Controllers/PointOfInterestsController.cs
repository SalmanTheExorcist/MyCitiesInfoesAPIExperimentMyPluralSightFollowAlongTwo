using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyCitiesInfo.API.Entities;
using MyCitiesInfo.API.Models;
using MyCitiesInfo.API.Services;

namespace MyCitiesInfo.API.Controllers
{
  
    [ApiController]
    [Authorize(Policy = "MustBeFromManamaBH")]
    [Route("api/v{version:apiVersion}/mycitiesinfoes/{mycitiesinfoesId:int}/pointofinterests")]
    [ApiVersion(2)]
    public class PointOfInterestsController : ControllerBase
    {
        private readonly ILogger<PointOfInterestsController> _logger;
        private readonly IMyMailService _myMailService;
        private readonly IMyCitiesInfoesRepository _myCitiesInfoesRepository;
        private readonly IMapper _mapper;

        public PointOfInterestsController(ILogger<PointOfInterestsController> logger,
                                          IMyMailService myMailService,
                                        IMyCitiesInfoesRepository myCitiesInfoesRepository,
                                        IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _myMailService = myMailService ?? throw new ArgumentNullException(nameof(myMailService));

            _myCitiesInfoesRepository = myCitiesInfoesRepository ??
                                     throw new ArgumentNullException(nameof(myCitiesInfoesRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }




        //-------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDTO>>> GetMyCityPointOfInterestsForCity(int mycitiesinfoesId)
        {

            //--look for the City-Claim:
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if (!await _myCitiesInfoesRepository.CityNameMatchesCityId(cityName, mycitiesinfoesId))
            {
                return Forbid();
            }




            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }
            var pointsOfInterestsFromDb = await _myCitiesInfoesRepository.GetPointsOfInterestsForACityAsync(mycitiesinfoesId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDTO>>(pointsOfInterestsFromDb));

        }
        //-------------------------------------------------------

        [HttpGet("{pointofinterestid:int}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDTO>> GetSinglePointOfInterest(int mycitiesinfoesId,
                                                                        int pointofinterestid)
        {

            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }

            var singlePointOfInterestFromDb = await _myCitiesInfoesRepository
                                            .GetSinglePointOfInterestForACityAsync(mycitiesinfoesId, pointofinterestid);

            if (singlePointOfInterestFromDb == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDTO>(singlePointOfInterestFromDb));


        }




        //-------------------------------------------------------
        //--Creating a resource below---
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDTO>> CreatePointOfInterest(int mycitiesinfoesId,
                                  [FromBody] PointOfInterestForCreationDTO pointOfInterestForCreationDTO)
        {

            //--Find the required city by id
            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }


            var newPointOfInterestForDb = _mapper.Map<Entities.PointOfInterest>(pointOfInterestForCreationDTO);


            await _myCitiesInfoesRepository.AddPointOfInterestToACityAsync(mycitiesinfoesId, newPointOfInterestForDb);

            await _myCitiesInfoesRepository.SaveChangesAsync();


            var newPointOfInterestDTO = _mapper.Map<PointOfInterestDTO>(newPointOfInterestForDb);

            return CreatedAtRoute("GetPointOfInterest",
                                new
                                {
                                    mycitiesinfoesId = mycitiesinfoesId,
                                    pointofinterestid = newPointOfInterestDTO.Id
                                },
                                newPointOfInterestDTO);



        }//--End--HttpPost (Created-Resource)




        //-------------------------------------------------------
        //--Updating a resource below (Full-Update with HTTP-PUT Method)---

        [HttpPut("{pointofinterestid:int}")]
        public async Task<ActionResult> UpdatePointOfInterest(int mycitiesinfoesId,
                                                  int pointofinterestid,
                                [FromBody] PointOfInterestForUpdatingDTO pointOfInterestForUpdatingDTO)
        {

            //--Find the required city 
            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }



            //--next, find pointOfInterst by id
            var pointOfInterestFromDb = await _myCitiesInfoesRepository
                                        .GetSinglePointOfInterestForACityAsync(mycitiesinfoesId,
                                                                                pointofinterestid);
            if (pointOfInterestFromDb == null)
            {
                return NotFound();
            }


            _mapper.Map(pointOfInterestForUpdatingDTO, pointOfInterestFromDb);

            await _myCitiesInfoesRepository.SaveChangesAsync();

            //--returning Status-Code:204 (Success _+ No Content to return)
            return NoContent();

        }//--End-HTTP-PUT





        //-------------------------------------------------------
        //--Partial Update using JsonPatch document--
        [HttpPatch("{pointofinterestid:int}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int mycitiesinfoesId,
                                              int pointofinterestid,
                   [FromBody] JsonPatchDocument<PointOfInterestForUpdatingDTO> jsonPatchDocument)
        {
            //--Find the required city by id
            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }


            //--next, find pointOfInterst by id
            var pointOfInterestFromDb = await _myCitiesInfoesRepository
                                       .GetSinglePointOfInterestForACityAsync(mycitiesinfoesId,
                                                                               pointofinterestid);
            if (pointOfInterestFromDb == null)
            {
                return NotFound();
            }



            //--
            var pointOfInterestForUpdatingDTOForPatch =
                        _mapper.Map<Models.PointOfInterestForUpdatingDTO>(pointOfInterestFromDb);



            //--
            jsonPatchDocument.ApplyTo(pointOfInterestForUpdatingDTOForPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestForUpdatingDTOForPatch))
            {
                return BadRequest(ModelState);
            }


            _mapper.Map(pointOfInterestForUpdatingDTOForPatch, pointOfInterestFromDb);

            await _myCitiesInfoesRepository.SaveChangesAsync();

            return NoContent();

        }//--End-HTTP-PATCH




        //-------------------------------------------------------
        //--Deleting a resource

        [HttpDelete("{pointofinterestid:int}")]
        public async Task<ActionResult> DeletePointOfInterest(int mycitiesinfoesId,
                                              int pointofinterestid)
        {
            //--Find the required city by id
            if (!await _myCitiesInfoesRepository.CityExistsAsync(mycitiesinfoesId))
            {
                _logger.LogInformation($"MyCity with Id {mycitiesinfoesId} was not found when accessing Points-Of-Interests.");
                return NotFound();

            }


            //--next, find pointOfInterst by id
            var pointOfInterestFromDb = await _myCitiesInfoesRepository
                                       .GetSinglePointOfInterestForACityAsync(mycitiesinfoesId,
                                                                               pointofinterestid);
            if (pointOfInterestFromDb == null)
            {
                return NotFound();
            }


            _myCitiesInfoesRepository.DeletePointOfInterest(pointOfInterestFromDb);
            await _myCitiesInfoesRepository.SaveChangesAsync();

            _myMailService.SendMail("Point-Of-Interest Deleted.",
                                    $"Point-Of-Interest {pointOfInterestFromDb.Name} with Id {pointOfInterestFromDb.Id} was deleted.");

            return NoContent();

        }//--End-HttpDelete




        //-------------------------------------------------------




    }//--End-Class
}//--End-Namespace
