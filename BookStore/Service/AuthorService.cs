using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Data.Dto.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

#region interface

public interface IAuthorService
{
    Task<(List<AuthorDto>? data, int totalCount, string message)> GetAll(AuthorFilter filter);
    Task<(AuthorDto? data, string? message)> GetById(Guid id);
    Task<(AuthorDto? data, string? message)> Update(AuthorUpdate form, Guid id);
    Task<(AuthorDto? data, string? message)> Add(AuthorForm form);
    Task<(bool? state, string? message)> Delete(Guid id);
}

#endregion

public class AuthorService : IAuthorService
{
    #region private

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region constructor

    public AuthorService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region get

    public async Task<(List<AuthorDto>? data, int totalCount, string message)> GetAll(AuthorFilter filter)
    {
        var query = _context.Authors.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(a => a.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync();

        var authors = await query
            .OrderBy(a => a.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (authors, totalCount, null);
    }

    public async Task<(AuthorDto? data, string? message)> GetById(Guid id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (author == null)
        {
            return (null, "Author not found.");
        }

        return (author, null);
    }

    #endregion

    #region update

    public async Task<(AuthorDto? data, string? message)> Update(AuthorUpdate form, Guid id)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
        {
            return (null, "Author not found.");
        }

        _mapper.Map(form, author);
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();

        var authorDto = _mapper.Map<AuthorDto>(author);
        return (authorDto, null);
    }

    #endregion

    #region add

    public async Task<(AuthorDto? data, string? message)> Add(AuthorForm form)
    {
        var author = _mapper.Map<Author>(form);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        var authorDto = _mapper.Map<AuthorDto>(author);
        return (authorDto, null);
    }

    #endregion

    #region delete

    public async Task<(bool? state, string? message)> Delete(Guid id)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
        {
            return (false, "Author not found.");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return (true, null);
    }

    #endregion
}
