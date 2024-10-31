using Microsoft.EntityFrameworkCore;
using MyCitiesInfo.API.DbContexts;
using MyCitiesInfo.API.Entities;

namespace MyCitiesInfo.API.Services
{
    public class MyCitiesInfoesRepository : IMyCitiesInfoesRepository
    {
        private readonly MyCitiesInfoContext _myCitiesInfoContext;

        public MyCitiesInfoesRepository(MyCitiesInfoContext myCitiesInfoContext)
        {
            _myCitiesInfoContext = myCitiesInfoContext ?? 
                                    throw new ArgumentNullException(nameof(myCitiesInfoContext));
        }

        //------------------------------------------------------------
      
        /*----
        public async Task<IEnumerable<MyCity>> GetCitiesAsync()
        {

            return await _myCitiesInfoContext.MyCities
                                             .OrderBy(c => c.Name)
                                            .ToListAsync();


        }
        -----*/

        //------------------------------------------------------------
        //--Overloaded GetCitiesAsync()
        public async Task<(IEnumerable<MyCity>, PaginationMetadata)> GetCitiesAsync(string? nameFilter, 
                                                              string? searchQuery,
                                                              int pageNumber,
                                                              int pageSize)
        {

         
            //start with a collection we can query:
            var myQuerableMyCitiesCollection = 
                _myCitiesInfoContext.MyCities as IQueryable<Entities.MyCity>;


            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                nameFilter = nameFilter.Trim();
                myQuerableMyCitiesCollection = myQuerableMyCitiesCollection
                                                .Where(c => c.Name == nameFilter);
            }


            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                myQuerableMyCitiesCollection = myQuerableMyCitiesCollection
                                             .Where(c => c.Name.Contains(searchQuery)
                                       || (c.Description != null && c.Description.Contains(searchQuery)));

            }


            var totalItemCount = await myQuerableMyCitiesCollection.CountAsync();

            var myPaginationMetadata = 
                            new PaginationMetadata(
                                totalItemCount,
                                pageSize,
                                pageNumber);


            var finalCollectionToReturn =  await myQuerableMyCitiesCollection
                            .OrderBy(c => c.Name)
                            .Skip(pageSize * (pageNumber - 1))
                            .Take(pageSize)
                            .ToListAsync();

            return (finalCollectionToReturn, myPaginationMetadata);

        }

        //------------------------------------------------------------
        public async Task<MyCity?> GetSingleCityAsync(int myCityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _myCitiesInfoContext.MyCities.Include(c => c.MyCityPointOfInterests)
                                                .Where(c => c.Id == myCityId)
                                                .FirstOrDefaultAsync();
            }

            return await _myCitiesInfoContext.MyCities
                                              .Where(c => c.Id == myCityId)
                                              .FirstOrDefaultAsync();



        }



        //------------------------------------------------------------

        public async Task<bool> CityExistsAsync(int myCityId)
        {
            return await _myCitiesInfoContext.MyCities.AnyAsync(c => c.Id == myCityId);
        }


        //------------------------------------------------------------
        public async Task<PointOfInterest?> GetSinglePointOfInterestForACityAsync(int myCityId,
                                                                       int pointOfInterestId)
        {
            return await _myCitiesInfoContext.PointOfInterests
                                 .Where(p => p.MyCityId == myCityId && p.Id == pointOfInterestId)
                                 .FirstOrDefaultAsync();

        }

        //------------------------------------------------------------
        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForACityAsync(int myCityId)
        {
            return await _myCitiesInfoContext.PointOfInterests
                                             .Where(p => p.MyCityId == myCityId)
                                             .ToListAsync();
        }


        //------------------------------------------------------------
        public async Task AddPointOfInterestToACityAsync(int myCityId, PointOfInterest newPointOfInterest)
        {
            var myCityFromDb = await this.GetSingleCityAsync(myCityId, false);
            if(myCityFromDb != null)
            {
               //--Note: This doesn't presist to the database not an I/O operation
               //- and so "async" not required
                myCityFromDb.MyCityPointOfInterests.Add(newPointOfInterest);
            }

        }

        //------------------------------------------------------------
        public async Task<bool> SaveChangesAsync()
        {
            //--return "true" if zero, one or more entities were saved successfully.
            return (await _myCitiesInfoContext.SaveChangesAsync() >= 0);
        }

        //------------------------------------------------------------
        public void DeletePointOfInterest(PointOfInterest pointOfInterestToDelete)
        {
            _myCitiesInfoContext.PointOfInterests.Remove(pointOfInterestToDelete);
        }

        //------------------------------------------------------------
        public async Task<bool> CityNameMatchesCityId(string? cityName, int myCityId)
        {
            return await _myCitiesInfoContext.MyCities
                                              .AnyAsync(c => c.Id == myCityId && c.Name == cityName);

        }



        //------------------------------------------------------------


    }//End-Class
}//--End-Namespace
