using Api.GraphQL.Data.Entities;
using AutoMapper;

namespace Api.GraphQL.Models.Mappers;

public class Maps : Profile
{
    public Maps()
    {
        CreateMap<Post, PostModel>();
        CreateMap<User, UserModel>();
    }
}