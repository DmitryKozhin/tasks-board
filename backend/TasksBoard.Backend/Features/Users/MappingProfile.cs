using AutoMapper;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, PublicUser>(MemberList.None);
        }
    }
}