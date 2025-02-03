using AutoMapper;
using black_follow.Entity;
using BookStore.Data.Dto.Author;
using BookStore.Data.Dto.Genre;
using BookStore.Data.User;

namespace BookStore.Helper;

public class MappinProfile:Profile
{
    public MappinProfile()
    {
        // User Maps 
        CreateMap<AppUser, UserLoginDto>();
        CreateMap<RegisterForm, AppUser>();
        
        // Book Maps
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Genre.Color))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest=>dest.Genre, opt=>opt.MapFrom(src=>src.Genre.Name));
        CreateMap<Book, BookGetAllDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Genre.Color))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
        CreateMap<Author, AuthorInBookDto>();
        
        // Author Maps
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorForm, Author>();
        
        // Genre Maps
        CreateMap<Genre, GenreDto>();







    }
}