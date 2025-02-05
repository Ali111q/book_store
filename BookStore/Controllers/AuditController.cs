using BookStore.Data.Dto.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

public class AuditController:BaseController
{
    private readonly DataContext _context;

    public AuditController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetAudit([FromQuery] BaseFilter filter)
    {
        var _query = _context.AuditLogs;
        var _count = await _query.CountAsync();
        var _AuditLogs = await _query.OrderBy(a => a.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize).ToListAsync();

        return Ok((_AuditLogs, _count, null), filter.Page, filter.PageSize);
    }
}