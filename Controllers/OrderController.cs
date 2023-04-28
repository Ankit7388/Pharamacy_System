using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.server;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="user")]
    public class OrderController : ControllerBase
    {
        private readonly WebApiContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public OrderController(WebApiContext context, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

       
        [HttpGet]
        public IEnumerable<OrderModel> GetOrderModels()
        {
          
            string userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            IEnumerable<OrderModel> orderItems = _context.OrderModels.Where(c => c.UserEmail == userID);
            return orderItems;
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderModel(int id)
        {
          if (_context.OrderModels == null)
          {
              return NotFound();
          }
            var orderModel = await _context.OrderModels.FindAsync(id);

            if (orderModel == null)
            {
                return NotFound();
            }

            return orderModel;
        }

        
       
        [HttpPost]
        public async Task<ActionResult<OrderModel>> PostOrderModel(int CartId)
        {
            string userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_context.OrderModels == null)
            {
                return Problem("Entity set 'WebApiContext.OrderModels'  is null.");
            }
            CartDrugs cart = _context.Carts.Where(w=>w.CartID==CartId).FirstOrDefault();
            DrugModel drug = _context.Drugs.Where(w=>w.DrugId==cart.DrugID).FirstOrDefault();
            OrderModel order = new OrderModel
            {
                UserEmail = userID,
                OrderedQuantity = cart.Quantity,
                OrderPrice = cart.Amount,
                DrugName = drug.DrugName,
                OrderedDate = DateTime.Now
            };
            if(order.OrderedQuantity>drug.DrugQuantityAvailable)
            {
                return BadRequest("Out Of Stock Or Quantity Not Available");
            }
            _context.OrderModels.Add(order);
            drug.DrugQuantityAvailable = drug.DrugQuantityAvailable - cart.Quantity;
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            EmailDTO request = new EmailDTO();
            request.To = "amina74@ethereal.email";
            request.Subject = "Order SuccessFully Placed";
            request.Body = "Hello Dear Customer Your Order With OrderId-" + order.OrderId.ToString() + " of Drug named " + drug.DrugName + " has been placed Successfully.1" ;
            _emailService.SendEmail(request);
            

            return CreatedAtAction("GetOrderModel", new {id = order.OrderId },order);
  
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderModel(int id)
        {
            if (_context.OrderModels == null)
            {
                return NotFound();
            }
            var orderModel = await _context.OrderModels.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }

            _context.OrderModels.Remove(orderModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderModelExists(int id)
        {
            return (_context.OrderModels?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
