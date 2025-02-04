using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Data.Dto.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

#region interface
public interface IGenreService
{
    Task<(List<GenreDto>? data, int totalCount, string message)> GetAll(GenreFilter filter);
    Task<(GenreDto? data, string? message)> GetById(Guid id);
    Task<(GenreDto? data, string? message)> Update(GenreUpdate form, Guid id);
    Task<(GenreDto? data, string? message)> Add(GenreForm form);
    Task<(bool? state, string? message)> Delete(Guid id);
}
#endregion

public class GenreService : IGenreService
{
    #region private
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    #endregion

    #region constructor
    public GenreService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    #endregion

    #region get
    public async Task<(List<GenreDto>? data, int totalCount, string message)> GetAll(GenreFilter filter)
    {
        var query = _context.Genres.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(g => g.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync();
        var genres = await query
            .OrderBy(g => g.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (genres, totalCount, null);
    }

    public async Task<(GenreDto? data, string? message)> GetById(Guid id)
    {
        var genre = await _context.Genres
            .Where(g => g.Id == id)
            .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return genre == null ? (null, "Genre not found.") : (genre, null);
    }
    #endregion

    #region update
    public async Task<(GenreDto? data, string? message)> Update(GenreUpdate form, Guid id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null) return (null, "Genre not found.");

        _mapper.Map(form, genre);
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();

        return (_mapper.Map<GenreDto>(genre), null);
    }
    #endregion

    #region add
    public async Task<(GenreDto? data, string? message)> Add(GenreForm form)
    {
        var genre = _mapper.Map<Genre>(form);
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        return (_mapper.Map<GenreDto>(genre), null);
    }
    #endregion

    #region delete
    public async Task<(bool? state, string? message)> Delete(Guid id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null) return (false, "Genre not found.");

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return (true, null);
    }
    #endregion
}
