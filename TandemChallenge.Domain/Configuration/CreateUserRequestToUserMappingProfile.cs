using AutoMapper;

namespace TandemChallenge.Domain.Configuration
{
    public class CreateUserRequestToUserMappingProfile : Profile
    {
        public CreateUserRequestToUserMappingProfile()
        {
            CreateMap<CreateUserCommand, User>();
        }
    }
}
