using System;
using Data.Entities;

namespace Data
{
    public class DatabaseInitializer
    {
        private readonly IDatabaseContext _databaseContext;


        public DatabaseInitializer(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public void GenerateMasterData()
        {
            _databaseContext.Groups.Insert(new Group
            {
                Id = 1,
                Name = "Regular users"
            });

            _databaseContext.Groups.Insert(new Group
            {
                Id = 2,
                Name = "Power Users"
            });

            _databaseContext.People.Insert(new Person
            {
                Id = 1,
                Name = "Admin",
                GroupId = 2
            });
        }
    }
}
