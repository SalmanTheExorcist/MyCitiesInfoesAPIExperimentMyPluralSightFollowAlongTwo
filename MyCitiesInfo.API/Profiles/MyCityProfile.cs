using AutoMapper;

namespace MyCitiesInfo.API.Profiles
{
    public class MyCityProfile: Profile
    {
        public MyCityProfile()
        {

            CreateMap<Entities.MyCity, Models.MyCitiesInfoWithoutPointsOfInterestsDTO>();
            CreateMap<Entities.MyCity, Models.MyCitiesInfoDTO>();
        }

    }//End-Class
}//End-Namespace
