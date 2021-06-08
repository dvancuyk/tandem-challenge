using AutoMapper;
using TandemChallenge.Api.Models;
using TandemChallenge.Domain;

namespace TandemChallenge.Api.Mapping
{
    public class AddUserViewModelToCreateUserCommandProfile : Profile
    {
        public AddUserViewModelToCreateUserCommandProfile()
        {
            CreateMap<AddUserViewModel, CreateUserCommand>();
        }
    }
}
