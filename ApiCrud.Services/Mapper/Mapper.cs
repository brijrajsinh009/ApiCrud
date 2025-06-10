namespace ApiCrud.Services.Mapper;

using ApiCrud.Data.CustomModels;
using ApiCrud.Data.Models;
using AutoMapper;


public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Book, BookViewModel>();
        CreateMap<BookViewModel, Book>();
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNo))
            .ReverseMap()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
            .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNumber));
            // .ForMember(dest => dest.Id, opt => opt.Ignore())  
            // .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
            // .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            // .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore());
    }
}
