using System;
using DataModel.Model;
using DevExpress.Xpo;
using Microsoft.EntityFrameworkCore;

namespace Tests.Setup
{
    public class TestingContext : Context
    {
        public TestingContext(DbContextOptions<Context> opt) : base(opt)
        {

        }

    }

}   