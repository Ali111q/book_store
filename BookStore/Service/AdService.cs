using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using Microsoft.EntityFrameworkCore;
using BookStore.Data.Dto.Ad;


namespace BookStore.Service
{
    public interface IAdService
    {
        Task<(List<AdDto>? data, int totalCount, string? message)> GetAll(AdFilter filter);
        Task<(AdDto? data, string? message)> GetById(Guid id);
        Task<(AdDto? data, string? message)> Update(AdUpdate form, Guid id);
        Task<(AdDto? data, string? message)> Add(AdForm form);
        Task<(bool? state, string? message)> Delete(Guid id);
    }
   
    public class AdService : IAdService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AdService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region GetAll

        public async Task<(List<AdDto>? data, int totalCount, string? message)> GetAll(AdFilter filter)
        {
            var query = _context.Ads.AsQueryable();
            
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(a => a.Title.Contains(filter.Search));
            }

            if (filter.Type is not null)
            {
                query = query.Where(a => a.Type == filter.Type);
            }

            var totalCount = await query.CountAsync();

            var ads = await query
                .OrderBy(a => a.Id)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize)
                .ProjectTo<AdDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return (ads, totalCount, null);
        }

        #endregion

        #region GetById

        public async Task<(AdDto? data, string? message)> GetById(Guid id)
        {
            var ad = await _context.Ads
                .ProjectTo<AdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ad == null)
            {
                return (null, "Ad not found.");
            }

            return (ad, null);
        }

        #endregion

        #region Update

        public async Task<(AdDto? data, string? message)> Update(AdUpdate form, Guid id)
        {
            var ad = await _context.Ads.FirstOrDefaultAsync(a => a.Id == id);

            if (ad == null)
            {
                return (null, "Ad not found.");
            }

            _mapper.Map(form, ad);
            _context.Ads.Update(ad);
            await _context.SaveChangesAsync();

            var adDto = _mapper.Map<AdDto>(ad);
            return (adDto, null);
        }

        #endregion

        #region Add

        public async Task<(AdDto? data, string? message)> Add(AdForm form)
        {
            var ad = _mapper.Map<Ads>(form);

            await _context.Ads.AddAsync(ad);
            await _context.SaveChangesAsync();

            var adDto = _mapper.Map<AdDto>(ad);
            return (adDto, null);
        }

        #endregion

        #region Delete

        public async Task<(bool? state, string? message)> Delete(Guid id)
        {
            var ad = await _context.Ads.FirstOrDefaultAsync(a => a.Id == id);

            if (ad == null)
            {
                return (false, "Ad not found.");
            }

            _context.Ads.Remove(ad);
            await _context.SaveChangesAsync();

            return (true, null);
        }

        #endregion
    }
}
