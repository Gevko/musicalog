using DataModel.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests.Setup
{
    public class TestingFactory : IDisposable
    {
        public TestingContext Context { get; private set; }

        private SqliteConnection _connection;

        private CreateTestData CreateTestData = new CreateTestData();

        public TestingFactory()
        {
            this.Context = CreateContext();
        }

        private TestingContext CreateContext()
        {
            _connection = GetConnection();
            var opt = GetContextOpt(_connection);
            _connection.Open();

            CreateTestData.Create(opt);
            TestingContext context = new TestingContext(opt);
            context.Database.EnsureCreated();
            return context;
        }

        private SqliteConnection GetConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            return connection;
        }

        private DbContextOptions<Context> GetContextOpt(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlite(connection)
                    .Options;

            return options;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    Context.Dispose();
                    _connection.Close();
                    _connection.Dispose();
                }

                Context = null;
                _connection = null;

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}