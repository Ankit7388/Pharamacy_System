using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class WebApiContext : DbContext
    {
        public WebApiContext (DbContextOptions<WebApiContext> options)
            : base(options)
        {
        }
        
        public DbSet<Users> Users { get; set; }
        public DbSet<DrugModel> Drugs { get; set; }
        public DbSet<CartDrugs> Carts { get; set; }
        public DbSet<OrderModel> OrderModels { get; set; }
        

            

    }
}
