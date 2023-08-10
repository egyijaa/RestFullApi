using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFullApi.Models;
using System.Security.Claims;

namespace RestFullApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomPagesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UsersdbContext _custompages;
        IWebHostEnvironment _webHostEnvironment;

        public CustomPagesController(UserManager<ApplicationUser> userManager, UsersdbContext users, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _custompages = users;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: api/<CustomPagesController>
        [HttpGet]
        public IActionResult Get()
        {
            var category = _custompages.CustomPages.Select(u => u.CustomUrl);
            if (category?.Any() != true)
            {
                // Handle null or empty list
                return new JsonResult(new { Message = "CustomPages Empty!" });
            }
            return new JsonResult(category);
        }

        // GET api/<CustomPagesController>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var emp = (from n in _custompages.CustomPages
                       join u in _custompages.Users on n.CreatedBy equals u.Id
                       join u2 in _custompages.Users on n.UpdatedBy equals u2.Id
                       where n.Id == id
                       select new
                       {
                           Id = n.Id,
                           CustomUrl = n.CustomUrl,
                           PageContent = n.PageContent,
                           CreatedBy = u.UserName,
                           UpdatedBy = u2.UserName,
                       }).ToList();

            if (emp?.Any() != true) return new JsonResult(new { Message = "CustomPages Not Found!" });
            return new JsonResult(emp);
        }

        // POST api/<CustomPagesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomPages model)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);

            model.CreatedBy = user.Id;
            model.UpdatedBy = user.Id;

            _custompages.Add(model);
            _custompages.SaveChanges();

            int id = model.Id;

            return new JsonResult(new { Message = "CustomPages Created", customPagesId = id });
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomPages model)
        {
            var category = await _custompages.CustomPages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (category == null) return new JsonResult(new { Message = "CustomPages Not Found!" });


            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);
            if (model.CustomUrl == null) model.CustomUrl = category.CustomUrl;
            if (model.PageContent == null) model.PageContent = category.PageContent;
            model.CreatedBy = category.CreatedBy;
            model.UpdatedBy = user.Id;
            _custompages.CustomPages.Attach(model);
            _custompages.Entry(model).State = EntityState.Modified;


            // _custompages.CustomPages.Update(model);
            _custompages.SaveChanges();

            return new JsonResult(new { Message = "CustomPages Updated" });
        }

        // DELETE api/<CustomPagesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var users = await _custompages.CustomPages.FindAsync(id);
            if (users == null) return new JsonResult(new { Message = "CustomPages Not Found!" });

            _custompages.CustomPages.Remove(users);
            _custompages.SaveChanges();

            return Ok(new { Message = "CustomPages Deleted" });

        }
    }
}
