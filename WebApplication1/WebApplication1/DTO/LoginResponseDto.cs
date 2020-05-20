using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.DTO
{
    public class LoginResponseDto
    {

        public String Login { get; set; }

        public String Password { get; set; }

    }
}