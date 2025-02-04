using System.Runtime.CompilerServices;
using black_follow.Entity;
using BookStore.Data.Dto.Order;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("send-cart-order")]
    public async Task<ActionResult> SendCartOrder([FromBody] OrderCartForm form) => Ok(await _orderService.SendCartOrder(form, Id));
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("send-order")]
    public async Task<ActionResult> SendOrder([FromBody] OrderForm form) => Ok(await _orderService.SendOrder(form, Id));

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] OrderFilter filter) => Ok(await _orderService.GetAll(filter, Id, Role), filter.Page, filter.PageSize);

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpPut("change-state")]
    public async Task<ActionResult> ChangeState([FromBody] ChangeOrderStateForm form) =>
        Ok(await _orderService.ChangeOrderStatus(form));
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("cancel/{id}")]
    public async Task<ActionResult> ChangeState( Guid id) =>
        Ok(await _orderService.CancelOrder(id, Id));

}