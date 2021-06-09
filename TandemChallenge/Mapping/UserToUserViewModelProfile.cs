using AutoMapper;
using TandemChallenge.Api.Models;
using TandemChallenge.Domain;

namespace TandemChallenge.Api.Mapping
{
    internal class UserToUserViewModelProfile : Profile
    {
        public UserToUserViewModelProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.Name, u => u.MapFrom(user => user.FullName));
        }
    }
}
