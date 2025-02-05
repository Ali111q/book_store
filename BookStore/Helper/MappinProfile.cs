using AutoMapper;
using black_follow.Entity;
using BookStore.Controllers;
using BookStore.Data.Dto.Ad;
using BookStore.Data.Dto.Author;
using BookStore.Data.Dto.Book;
using BookStore.Data.Dto.Genre;
using BookStore.Data.Dto.Order;
using BookStore.Data.User;

namespace BookStore.Helper;

public class MappinProfile : Profile
{
    public MappinProfile()
    {
        #region User Maps
        CreateMap<AppUser, UserLoginDto>();
        CreateMap<RegisterForm, AppUser>();
        #endregion

        #region Book Maps
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Genre.Color))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

        CreateMap<Book, BookGetAllDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Genre.Color));

        CreateMap<Author, AuthorInBookDto>();

        CreateMap<BookUpdate, Book>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<BookForm, Book>();
        #endregion

        #region Author Maps
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorForm, Author>();

        CreateMap<AuthorUpdate, Author>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        #endregion

        #region Genre Maps
        CreateMap<Genre, GenreDto>();
        CreateMap<GenreForm, Genre>();

        CreateMap<GenreUpdate, Genre>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        #endregion

        #region Order Maps
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(x => x.Price * x.Count)));

        CreateMap<OrderItem, OrderItemDto>();
        #endregion

        #region Ads Maps
        CreateMap<Ads, AdDto>();
        CreateMap<AdForm, Ads>();
        CreateMap<AdUpdate, Ads>();
        CreateMap<AdDto, Ads>();
        #endregion
    }
}
