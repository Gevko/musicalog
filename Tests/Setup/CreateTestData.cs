using System;
using DataModel.Enums;
using DataModel.Model;
using DevExpress.Xpo;
using Microsoft.EntityFrameworkCore;

namespace Tests.Setup
{

    internal class CreateTestData
    {
        internal const string FIRST_TITLE_NAME = "Reggae Roots";
        internal const string SECOND_TITLE_NAME = "Trench Town Rock";

        internal void Create(DbContextOptions<Context> opt)
        {
            using(var context = new TestingContext(opt))
            {
                context.Database.EnsureCreated();

                context.Albums.Add(new Album() { Id = Guid.NewGuid(), Title = FIRST_TITLE_NAME, ArtistName = "Bob Marley", Type = AlbumType.CD, Stock = 1 });

                context.Albums.Add(new Album() { Id = Guid.NewGuid(), Title = SECOND_TITLE_NAME, ArtistName = "Bob Marley", Type = AlbumType.CD, Stock = 1 });

                context.SaveChanges();
            }

        }
    }

}