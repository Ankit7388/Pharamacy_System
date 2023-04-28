using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="user")]
    public class CartController : ControllerBase
    {
        private readonly WebApiContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(WebApiContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<CartDrugs>> AddToCart(int drugID, int quantity)
        {
            
            string userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
         
            DrugModel drug = _context.Drugs.Find(drugID);

            
            double price = (drug.DrugPrice * quantity);

         
            CartDrugs cart = new CartDrugs
            {
                DrugID = drugID,
                UserEmail = userID,
                Quantity = quantity,
                Amount = price,
                DateAdded = DateTime.Now
            };

          
            if(quantity>drug.DrugQuantityAvailable)
            {
                return BadRequest("Quantity Not Avaialable");
            }
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return Ok("Product added to cart");
        }

      
        [HttpGet]
        public IEnumerable<CartDrugs> GetCartItems()
        {
            
            string userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

          
            IEnumerable<CartDrugs> cartItems = _context.Carts.Where(c => c.UserEmail == userID);

            return cartItems;
        }

      
        [HttpDelete]
        public async Task<ActionResult<CartDrugs>> RemoveFromCart(int cartID)
        {
            
            CartDrugs cart = _context.Carts.Find(cartID);

            if (cart == null)
            {
                return NotFound();
            }

        
            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return Ok("Product removed from cart");
        }

        
        
    }

}


