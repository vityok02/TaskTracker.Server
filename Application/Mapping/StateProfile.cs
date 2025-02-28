using Application.Modules.States;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping;

public class StateProfile : Profile
{
    public StateProfile()
    {
        CreateMap<StateModel, StateDto>();
    }
}
