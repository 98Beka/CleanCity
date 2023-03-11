using CleanCity.DTO;
using CleanCity.Models;
using AutoMapper;

namespace CleanCity.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PointOnTheMap, PointOnTheMapDTO>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        dest.Position = new double[]
                        {
                                    src.Latitude,
                                    src.Longitude
                        };
                        return true;
                    }
                ));
        }
    }
}
