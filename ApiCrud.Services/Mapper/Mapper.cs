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
    }
}
