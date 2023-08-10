using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFullApi.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestFullApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UsersdbContext _category;
        IWebHostEnvironment _webHostEnvironment;

        public CategoryController(UserManager<ApplicationUser> userManager, UsersdbContext users, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _category = users;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var category = _category.Category.Select(u => u.Name);
            if (category?.Any() != true)
            {
                // Handle null or empty list
                return new JsonResult(new { Message = "Category Empty!" });
            }
            return new JsonResult(category);
        }

        // GET api/<CategoryController>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var emp = (from n in _category.Category
                       join u in _category.Users on n.CreatedBy equals u.Id
                       join u2 in _category.Users on n.UpdatedBy equals u2.Id
                       where n.Id == id
                       select new
                       {
                           Id = n.Id,
                           Name = n.Name,
                           CreatedBy = u.UserName,
                           UpdatedBy = u2.UserName,
                       }).ToList();

            if (emp?.Any() != true )  return new JsonResult(new { Message = "Category Not Found!" });
            return new JsonResult(emp);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category model)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);

            model.CreatedBy = user.Id;
            model.UpdatedBy = user.Id;

            _category.Add(model);
            _category.SaveChanges();

            int id = model.Id;

            return new JsonResult(new { Message = "Category Created", categoryId = id });
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Category model)
        {
            var category = await _category.Category.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (category == null) return new JsonResult(new { Message = "Category Not Found!" });


            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);
            model.CreatedBy = category.CreatedBy;
            model.UpdatedBy = user.Id;
            _category.Category.Attach(model);
            _category.Entry(model).State = EntityState.Modified;


            // _category.Category.Update(model);
            _category.SaveChanges();

            return new JsonResult(new { Message = "Category Updated" });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var users = await _category.Category.FindAsync(id);
            if (users == null) return new JsonResult(new { Message = "Category Not Found!" });

            _category.Category.Remove(users);
            _category.SaveChanges();

            return Ok(new { Message = "Category Deleted" });

        }
    }
}
