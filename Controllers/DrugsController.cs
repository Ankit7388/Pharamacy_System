using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class DrugsController : ControllerBase
    {
        private readonly WebApiContext _context;

        public DrugsController(WebApiContext context)
        {
            _context = context;
        }



        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrugModel>>> GetDrugs()
        {
          if (_context.Drugs == null)
          {
              return NotFound();
          }
            return await _context.Drugs.ToListAsync();
        }



        [Authorize(Roles = "admin,user")]
        [HttpGet("GetDrugModelById/{id}")]
        public async Task<ActionResult<DrugModel>> GetDrugModelById(int id)
        {
          if (_context.Drugs == null)
          {
              return NotFound();
          }
            var drugModel = await _context.Drugs.FindAsync(id);

            if (drugModel == null)
            {
                return NotFound();
            }

            return drugModel;
        }


        [Authorize(Roles = "admin,user")]
        [HttpGet("GetDrugModelByName/{DrugName}")]
        public async Task<ActionResult<DrugModel>> GetDrugModelByName(string DrugName)
        {
            if (_context.Drugs == null)
            {
                return NotFound();
            }
            var drugModel = _context.Drugs.Where(w=>w.DrugName.Contains(DrugName)).FirstOrDefault();

            if (drugModel == null)
            {
                return NotFound();
            }

            return drugModel;
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrugModel(int id, DrugModel drugModel)
        {
            if (id != drugModel.DrugId)
            {
                return BadRequest();
            }

            _context.Entry(drugModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrugModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<DrugModel>> PostDrugModel(DrugModel drugModel)
        {
          if (_context.Drugs == null)
          {
              return Problem("Entity set 'WebApiContext.Drugs'  is null.");
          }
            _context.Drugs.Add(drugModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrugModelById", new { id = drugModel.DrugId }, drugModel);
        }



        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrugModel(int id)
        {
            if (_context.Drugs == null)
            {
                return NotFound();
            }
            var drugModel = await _context.Drugs.FindAsync(id);
            if (drugModel == null)
            {
                return NotFound();
            }

            _context.Drugs.Remove(drugModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DrugModelExists(int id)
        {
            return (_context.Drugs?.Any(e => e.DrugId == id)).GetValueOrDefault();
        }
    }
}
