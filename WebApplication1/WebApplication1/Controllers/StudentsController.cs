using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DAL;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        
        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";
        private readonly IStudentDbServ _studentDbServ;

        public IConfiguration Configuration { get; set; }
        public StudentsController(IConfiguration configuration, IStudentDbServ studentDbServ)
        {
            Configuration = configuration;
            _studentDbServ = studentDbServ;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetStudents()
        {
            return Ok(_studentDbServ.GetStudents());
        }
        
        [HttpGet("{id}")]
        
        public IActionResult GetStudentsById([FromRoute] String id)
        {
            return Ok(_studentDbServ.GetStudentById(id));
        }

        [HttpPost]

        public IActionResult Login(LoginRequestDto request)
        {
            LoginResponseDto response = new LoginResponseDto();
            LoginDbService res = new LoginDbService();
            response.Login = res.GetLoginAndPass(request.Login).IndexNumber;
            response.Password = res.GetLoginAndPass(request.Login, request.Password).Password;
            
            if (request.Login == response.Login)
            {
                var check = (PasswordHashing.Validate(request.Password, res.GetPass(request.Login).Salt.ToString(), res.GetPass(request.Login).Password.ToString()));

                if (check)
                { 
                var claims = new[]
                {
                     new Claim(ClaimTypes.NameIdentifier, "1"),
                     new Claim(ClaimTypes.Name, "jan123"),
                     new Claim(ClaimTypes.Role, "admin"),
                     new Claim(ClaimTypes.Role, "student")
                 };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );
                SaveToken s = new SaveToken();
                String refToken = Guid.NewGuid().ToString();
                s.Save(response.Login, refToken);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refToken
                }
                );  }
                return StatusCode(401, "Bad Password");
            }  

            return StatusCode(401, "Bad login");
        }

        [HttpPost("refresh-token/{refToken}")]

        public IActionResult RefreshToken([FromRoute] String refToken)
        {
            StudentLogin student = new StudentLogin();
            CheckToken c = new CheckToken();
            student = c.Check(refToken);
            
            if (student.refToken != null)
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "jan123"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );
                SaveToken s = new SaveToken();
                String newRefToken = Guid.NewGuid().ToString();
                s.Save(student.IndexNumber, newRefToken);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    newRefToken
                }
                );
            }
            return StatusCode(401, "Wrong token");
            
        }

    }


}

