//using Microsoft.AspNetCore.Mvc;
//using RestFullApi.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.EntityFrameworkCore;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace RestFullApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private UsersdbContext _users;
//        IWebHostEnvironment _webHostEnvironment;

//        public UsersController(UsersdbContext users, IWebHostEnvironment webHostEnvironment)
//        {
//            _users = users;
//            _webHostEnvironment = webHostEnvironment;
//        }
//        // GET: api/<UsersController>
//        [HttpGet]
//        public IActionResult Get()
//        {
//            var user = _users.Users.Select(u => u.Name);
//            return new JsonResult(user);
//        }


//        public Users GetById(int id)
//        {
//            var users = _users.Users.Find(id);
//            if (users == null) { throw new KeyNotFoundException("User Not Found"); }
//            return users;
//        }


//        // GET api/<UsersController>/5
//        [AllowAnonymous]
//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            var emp = _users.Users.Find(id);
//            return new JsonResult(emp);
//        }

//        // POST api/<UsersController>
//        [HttpPost]
//        public IActionResult Post([FromBody] Users model)
//        {

//            var userExist = _users.Users.Any(e => e.Username == model.Username || e.Email == model.Email);
//            if (userExist == true)
//            {
//                return Ok(new { Message = "User Already Created" });

//            }

//            _users.Add(model);
//            _users.SaveChanges();

//            return Ok(new { Message = "User Created" });
//        }

//        // PUT api/<UsersController>/5
//        [HttpPut("{id}")]
//        public IActionResult Put([FromBody] Users model)
//        {

//            _users.Users.Attach(model);
//            _users.Entry(model).State = EntityState.Modified;


//            // _users.Users.Update(model);
//            _users.SaveChanges();

//            return Ok(new { Message = "User Updated" });
//        }

//        // DELETE api/<UsersController>/5
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            var user = GetById(id);

//            _users.Users.Remove(user);
//            _users.SaveChanges();

//            return Ok(new { Message = "User Deleted" });

//        }
//    }
//}
