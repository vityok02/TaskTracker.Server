using Application.Modules.States;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping;

public class StateProfile : Profile
{
    public StateProfile()
    {
        CreateMap<StateEntity, StateDto>();

        CreateMap<StateModel, StateDto>();
    }
}
