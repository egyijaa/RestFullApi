using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFullApi.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestFullApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private UsersdbContext _news;
        IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsController(UserManager<ApplicationUser> userManager, UsersdbContext news, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _news = news;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: api/<NewsController>
        [HttpGet]
        public IActionResult Get()
        {
            var news = _news.News.Select(u => u.Title);
            if (news?.Any() != true)
            {
                // Handle null or empty list
                return new JsonResult(new { Message = "News Empty!" });
            }
            return new JsonResult(news);
        }

        // GET api/<NewsController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var emp = (from n in _news.News
                       join c in _news.Category on n.CategoryId equals c.Id
                       join u in _news.Users on n.CreatedBy equals u.Id
                       join u2 in _news.Users on n.UpdatedBy equals u2.Id
                       where n.Id == id
                       select new
                       {
                           Id = n.Id,
                           Name = n.Title,
                           NewsContent = n.NewsContent,
                           Category = c.Name,
                           CreatedBy = u.UserName,
                           UpdatedBy = u2.UserName,
                       }).ToList();
            var comments = _news.Comments.Where(c => c.NewsId == id).Select(c => new { c.Name, c.Comment });

            if (emp?.Any() != true) return new JsonResult(new { Message = "News Not Found!" });
            return new JsonResult(new {detailNews = emp, commentNews = comments});
        }

        // POST api/<NewsController>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] News model)
        {
            var checkCategory = await _news.News.FindAsync(model.CategoryId);
            if (checkCategory == null) return new JsonResult(new { Message = "Category Not Found!" });

            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);

            if (model.Title == null) model.Title = checkCategory.Title;
            if (model.NewsContent == null) model.NewsContent = checkCategory.NewsContent;
            model.CreatedBy = user.Id;
            model.UpdatedBy = user.Id;

            _news.Add(model);
            _news.SaveChanges();

            int id = model.Id;

            return new JsonResult(new { Message = "News Created", categoryId = id });
        }

        // PUT api/<UsersController>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] News model)
        {
            var news = await _news.News.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (news == null) return new JsonResult(new { Message = "News Not Found!" });
            if (model.CategoryId == 0) model.CategoryId = news.CategoryId;

            var checkCategory = await _news.Category.FindAsync(model.CategoryId);
            if (checkCategory == null) return new JsonResult(new { Message = "Category Not Found!" });

            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);
            model.CreatedBy = news.CreatedBy;
            model.UpdatedBy = user.Id;

            _news.News.Attach(model);
            _news.Entry(model).State = EntityState.Modified;


            // _news.News.Update(model);
            _news.SaveChanges();

            return new JsonResult(new { Message = "News Updated" });
        }

        // DELETE api/<NewsController>/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _news.News.FindAsync(id);
            if (news == null) return new JsonResult(new { Message = "News Not Found!" });

            _news.News.Remove(news);
            _news.SaveChanges();

            return Ok(new { Message = "News Deleted" });

        }
    }
}
