using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class BooksController : ControllerBase
    {
        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";
        private readonly IStudentDbService _studentDbService;

        public BooksController(IStudentDbService studentDbService)
        {
            _studentDbService = studentDbService;
        }

        [HttpGet]

        public IActionResult GetStudents()
        {
            return Ok(_studentDbService.GetStudents());
        }

        [HttpGet("{id}")]

        public IActionResult GetStudentsById([FromRoute] String id)
        {
            return Ok(_studentDbService.GetStudentById(id));
        }
    }
}