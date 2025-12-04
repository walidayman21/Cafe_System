using Cafe__System.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafe__System.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllCategory")]
        public ActionResult GetAllCategory()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }
    }
}
