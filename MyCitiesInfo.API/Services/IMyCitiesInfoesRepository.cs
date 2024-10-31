using MyCitiesInfo.API.Entities;

namespace MyCitiesInfo.API.Services
{
    public interface IMyCitiesInfoesRepository
    {

        //-----------------------------------------------
        /*----    
        Task<IEnumerable<MyCity>> GetCitiesAsync(); 
        -----*/
        //-----------------------------------------------
        Task<(IEnumerable<MyCity>, PaginationMetadata)> GetCitiesAsync(string? nameFilter, 
                                                string? searchQuery, 
                                                int pageNumber, 
                                                int pageSize);



        //-----------------------------------------------
        Task<bool> CityExistsAsync(int myCityId);

        //-----------------------------------------------
        //--This method may return null
        Task<MyCity?> GetSingleCityAsync(int myCityId, bool includePointsOfInterest);

        //-----------------------------------------------
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForACityAsync(int myCityId);


        //-----------------------------------------------
        //--This method may return null
        Task<PointOfInterest?> GetSinglePointOfInterestForACityAsync(int myCityId,
                                                                int pointOfInterestId);


        //-----------------------------------------------
        Task AddPointOfInterestToACityAsync(int myCityId, PointOfInterest newPointOfInterest);

        //-----------------------------------------------

        void DeletePointOfInterest(PointOfInterest pointOfInterestToDelete);

        //-----------------------------------------------
        Task<bool> CityNameMatchesCityId(string? cityName, int myCityId);




        //-----------------------------------------------
        Task<bool> SaveChangesAsync();

        //-----------------------------------------------
    }//End-interface
}//--End-Namespace
