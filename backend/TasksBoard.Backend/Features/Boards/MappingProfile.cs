using AutoMapper;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Boards
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Board, SmallBoardModel>(MemberList.None);
        }
    }
}