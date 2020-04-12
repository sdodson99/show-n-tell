using AutoMapper;

namespace ShowNTell.API.Models.MappingProfiles
{
    public class MapperFactory
    {
        public IMapper CreateMapper()
        {
            MapperConfiguration config = new MapperConfiguration((o) =>
            {
                o.AddProfile<ResponseDomainMappingProfile>();
            });

            return config.CreateMapper();
        }
    }
}