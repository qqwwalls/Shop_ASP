using AutoMapper;
using Shop.Application.DTOs.UserDTOs;
using Shop.Domain.Models;

namespace Shop.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserReadDTO>();
        }
    }
}
