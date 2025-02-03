

using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Data.Dto.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IAuthorService
{
    Task<(List<AuthorDto> data, int totalCount, string message )> GetAll(AuthorFilter filter);
    Task<(AuthorDto? data, string?message)> GetById(Guid id);
    Task<(AuthorDto? data, string? message)> Update(AuthorUpdateForm form);
    Task<(AuthorDto? data, string? message)> Add(AuthorForm form);
    Task<(bool? state, string? message)> Delete(Guid id);
}
public class AuthorService:IAuthorService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AuthorService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(List<AuthorDto> data, int totalCount, string message)> GetAll(AuthorFilter filter)
    {
        // Step 1: Calculate the total count with the filter
        var query = _context.Authors.AsQueryable();
        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(a => a.Name.Contains(filter.Name));
        }

        var totalCount = await query.CountAsync();  

        // Step 2: Apply pagination and project to AuthorDto
        var authors = query
            .OrderBy(a => a.Id)  
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (await authors, totalCount, null);
    }


    public async Task<(AuthorDto? data, string? message)> GetById(Guid id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();  // Returns null if not found

        if (author == null)
        {
            return (null, "Author not found.");
        }

        return (author, null);  // If found, return the AuthorDto
    }


    public async Task<(AuthorDto? data, string? message)> Update(AuthorUpdateForm form)
    {
        var author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == form.Id);  // Fetch the author by ID

        if (author == null)
        {
            return (null, "Author not found.");  // If the author doesn't exist, return an error message
        }

        // Step 1: Map the form data to the existing Author entity
        _mapper.Map(form, author);  // Map only the properties provided in the form to the author entity

        // Step 2: Save the updated entity
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();

        // Step 3: Return the updated author as a DTO
        var authorDto = _mapper.Map<AuthorDto>(author);  // Map back to AuthorDto for return
        return (authorDto, null);
    }


    public async Task<(AuthorDto? data, string? message)> Add(AuthorForm form)
    {
        // Step 1: Map the form data to the Author entity
        var author = _mapper.Map<Author>(form);  // Map the form to an Author entity

        // Step 2: Add the new author to the context
        await _context.Authors.AddAsync(author);
    
        // Step 3: Save the changes to the database
        await _context.SaveChangesAsync();

        // Step 4: Map the newly added author back to an AuthorDto for the response
        var authorDto = _mapper.Map<AuthorDto>(author);

        return (authorDto, null);  // Return the added author DTO with no error message
    }


    public async Task<(bool? state, string? message)> Delete(Guid id)
    {
        var author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id);  // Find the author by ID

        if (author == null)
        {
            return (false, "Author not found.");  // If the author doesn't exist, return false with a message
        }

        // Step 1: Remove the author from the context
        _context.Authors.Remove(author);

        // Step 2: Save changes to the database
        await _context.SaveChangesAsync();

        return (true, "Author deleted successfully.");  // Return true indicating successful deletion
    }

}