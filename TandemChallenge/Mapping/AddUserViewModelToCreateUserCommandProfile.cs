using AutoMapper;
using TandemChallenge.Api.Models;
using TandemChallenge.Domain;

namespace TandemChallenge.Api.Mapping
{
    internal class AddUserViewModelToCreateUserCommandProfile : Profile
    {
        public AddUserViewModelToCreateUserCommandProfile()
        {
            CreateMap<AddUserViewModel, CreateUserCommand>();
        }
    }
}
