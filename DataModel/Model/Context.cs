using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Model
{
    public partial class Context: DbContext
    {
        public Context() : base()
        { }

        public Context(DbContextOptions<Context> options) :base(options)
        { }

        public virtual DbSet<Album> Albums
        {
            get; set;
        }
    }
}
