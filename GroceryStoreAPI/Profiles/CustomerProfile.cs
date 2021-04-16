using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Entities.Customer, Models.CustomerForDisplayDTO>();
            CreateMap<Models.CustomerForCreationDTO, Entities.Customer>();
            CreateMap<Models.CustomerForUpdateDTO, Entities.Customer>();
        }
    }
}
