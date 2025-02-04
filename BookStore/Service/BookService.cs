using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Data.Dto.Book;
using BookStore.Data.Dto.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

#region interface



public interface IBookService
{
    
    Task<(List<BookGetAllDto>? data, int totalCount, string? message)> GetAll(BookFilter filter);
    Task<(BookDto? data, string? message)> GetById(Guid id);
    Task<(BookDto? data, string? message)> Update(BookUpdate form, Guid id);
    Task<(BookDto? data, string? message)> Add(BookForm form);
    Task<(bool? state, string? message)> Delete(Guid id);
}
#endregion

public class BookService : IBookService
{
    #region private

    

    private readonly DataContext _context;
    private readonly IMapper _mapper;
    #endregion

    #region constructor

    

    public BookService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    #endregion

    #region get 

    

    public async Task<(List<BookGetAllDto>? data, int totalCount, string? message)> GetAll(BookFilter filter)
    {
        var query = _context.Books .Include(b => b.Author)
            .Include(b => b.Genre)
            .Where(b => b.IsAvailable == filter.IsAvailable).AsQueryable();

        if (!string.IsNullOrEmpty(filter.Search))
        {
   query = query.Where(b =>
                b.Name.Contains(filter.Search) || b.Author.Name.Contains(filter.Search) ||
                b.Genre.Name.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync();

        // Step 2: Apply pagination and project to GenreDto
        var genres = await query
            .OrderBy(b => b.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ProjectTo<BookGetAllDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (genres, totalCount, null);
    }

    public async Task<(BookDto? data, string? message)> GetById(Guid id)
    {
        var book = await _context.Books
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return (null, "Book not found.");
        }


        return (book, null);
    }
    #endregion

    #region update

    

    public async Task<(BookDto? data, string? message)> Update(BookUpdate form, Guid id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return (null, "Book not found.");
        }
        _mapper.Map(form, book);

        _context.Books.Update(book);
        await _context.SaveChangesAsync();

        var bookDto = _mapper.Map<BookDto>(book);
        return (bookDto, null);
    }
    #endregion

    #region add

    

    public async Task<(BookDto? data, string? message)> Add(BookForm form)
    {
        var _genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == form.GenreId);
        if (_genre == null) return (null, "Genre not found.");
        var _author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == form.AuthorId);
        if (_author == null) return (null, "Author not found.");
        var book = _mapper.Map<Book>(form);

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var bookDto = _mapper.Map<BookDto>(book);
        return (bookDto, null);
    }
    #endregion

    #region delete
    
    public async Task<(bool? state, string? message)> Delete(Guid id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return (false, "Book not found.");
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return (true, null);
    }
    #endregion
}
