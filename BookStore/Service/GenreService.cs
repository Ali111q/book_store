using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Data.Dto.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IGenreService
{
    Task<(List<GenreDto>? data, int totalCount, string message)> GetAll(GenreFilter filter);
    Task<(GenreDto? data, string? message)> GetById(Guid id);
    Task<(GenreDto? data, string? message)> Update(GenreUpdate form, Guid id);
    Task<(GenreDto? data, string? message)> Add(GenreForm form);
    Task<(bool? state, string? message)> Delete(Guid id);
}

public class GenreService : IGenreService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GenreService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(List<GenreDto>? data, int totalCount, string message)> GetAll(GenreFilter filter)
    {
        // Step 1: Calculate the total count with the filter
        var query = _context.Genres.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(g => g.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync();

        // Step 2: Apply pagination and project to GenreDto
        var genres = query
            .OrderBy(g => g.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (await genres, totalCount, null);
    }

    public async Task<(GenreDto? data, string? message)> GetById(Guid id)
    {
        var genre = await _context.Genres
            .Where(g => g.Id == id)
            .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (genre == null)
        {
            return (null, "Genre not found.");
        }

        return (genre, null); // If found, return the GenreDto
    }

    public async Task<(GenreDto? data, string? message)> Update(GenreUpdate form, Guid id)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
        {
            return (null, "Genre not found.");
        }

        // Step 1: Map the form data to the existing Genre entity
        _mapper.Map(form, genre);

        // Step 2: Save the updated entity
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();

        // Step 3: Return the updated genre as a DTO
        var genreDto = _mapper.Map<GenreDto>(genre);
        return (genreDto, null);
    }

    public async Task<(GenreDto? data, string? message)> Add(GenreForm form)
    {
        // Step 1: Map the form data to the Genre entity
        var genre = _mapper.Map<Genre>(form);

        // Step 2: Add the new genre to the context
        await _context.Genres.AddAsync(genre);

        // Step 3: Save the changes to the database
        await _context.SaveChangesAsync();

        // Step 4: Map the newly added genre back to a GenreDto for the response
        var genreDto = _mapper.Map<GenreDto>(genre);

        return (genreDto, null); // Return the added genre DTO with no error message
    }

    public async Task<(bool? state, string? message)> Delete(Guid id)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
        {
            return (false, "Genre not found.");
        }

        // Step 1: Remove the genre from the context
        _context.Genres.Remove(genre);

        // Step 2: Save changes to the database
        await _context.SaveChangesAsync();

        return (true, null);
    }
}
