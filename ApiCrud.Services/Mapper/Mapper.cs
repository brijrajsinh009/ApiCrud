namespace ApiCrud.Services.Mapper;

using ApiCrud.Data.CustomModels;
using ApiCrud.Data.Models;
using AutoMapper;


public class Mapper : Profile
{
    public Mapper()
    {

        CreateMap<BookViewModel, Book>()
            // .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
            .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

        CreateMap<Book, BookViewModel>();
        // Entity → ViewModel
        // CreateMap<Book, BookViewModel>()
        //     .ForMember(dest => dest.Image, opt => opt.Ignore());

        CreateMap<User, UserViewModel>()
    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNo))
    .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore()) // You likely won't map back to ConfirmPassword
    .ForMember(dest => dest.Image, opt => opt.Ignore()) // Image (IFormFile) only used in upload
    .ReverseMap()
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
    .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNumber))
    .ForMember(dest => dest.ProfileUrl, opt => opt.Ignore()) // set manually after upload
    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
    .ForMember(dest => dest.ModifiedOn, opt => opt.Ignore())
    .ForMember(dest => dest.IsDelete, opt => opt.Ignore());
    }
}
