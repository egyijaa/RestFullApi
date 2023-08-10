using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFullApi.Models;
using System.Security.Claims;

namespace RestFullApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private UsersdbContext _comments;
        IWebHostEnvironment _webHostEnvironment;

        public CommentsController(UsersdbContext comments, IWebHostEnvironment webHostEnvironment)
        {
            _comments = comments;
            _webHostEnvironment = webHostEnvironment;
        }

        // POST api/<CustomPagesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Comments model)
        {
            var news = await _comments.News.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.NewsId);
            if (news == null) return new JsonResult(new { Message = "News Not Found!" });

            if (model.Name == null) model.Name = "Anonymous";

            _comments.Add(model);
            _comments.SaveChanges();

            int id = model.Id;

            return new JsonResult(new { Message = "Comment Created", commentId = id });
        }
    }
}
