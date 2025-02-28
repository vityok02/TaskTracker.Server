using Api.Controllers.State.Responses;
using Application.Modules.States;
using AutoMapper;

namespace Api.Mapping;

public class StateProfile : Profile
{
    public StateProfile()
    {
        CreateMap<StateDto, StateResponse>();
    }
}
