using FirstAPIDemo.Models;
using FirstAPIDemo.Models.DTOs;
using FirstAPIDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtGen;
        private readonly List<string> _listOfUsers;

        public UserController(IJwtService jwtService)
        {

            _jwtGen = jwtService;

            _listOfUsers = new List<string>
            {
                "suleiman s", "stanley u", "edore u", "blessing e", "isreal o"
            };
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser/{name}")]
        public IActionResult GetUser(string name)
        {
            var counter = 0;
            var userToReturn = new UsersToReturnDto();

            foreach (var user in _listOfUsers)
            {
                var splittedName = user.Split(" ");
                if (splittedName[0].Equals(name))
                {
                    userToReturn.FullName = $"{splittedName[0]} {splittedName[1]}";
                    if(!string.IsNullOrWhiteSpace(splittedName[2]))
                        userToReturn.Position = splittedName[2];
                    userToReturn.Title = "DecaDev";
                }

                counter++;
            }

            return Ok(userToReturn);
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = new List<UsersToReturnDto>();

            foreach(var user in _listOfUsers)
            {
                users.Add(new UsersToReturnDto { FullName = user, Position = "none", Title = "DecaDev" });
            }

            return Ok(users);

        }

        [Authorize]
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(UserDetailsDto model)
        {
            //throw new Exception("Error caught!");

            if (model == null)
                return BadRequest("Empty model object");

            //var newUser = new User() { 
            //  FirstName = model.FirstName,
            //  LastName = model.LastName,
            //  Position = model.Position,
            //  Title = model.Title
            //};

            _listOfUsers.Add($"{model.FirstName} {model.LastName} {model.Position}");

            return Ok("New user added!");
        }

        [HttpGet]
        [Route("JwtGen")]
        public IActionResult GenerateToken()
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                LastName = "Ibe",
                FirstName = "Francis"
            };

            var roles = new List<string> { "Staff", "Stack Lead" };

            var token = _jwtGen.JwtGen(user, roles);
            return Ok(token);
        }
    }
}
