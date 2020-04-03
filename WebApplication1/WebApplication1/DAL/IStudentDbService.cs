﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public interface IStudentDbService
    {
        IEnumerable<Student> GetStudents();
        IEnumerable<Student> GetStudentById(String id);
    }
}