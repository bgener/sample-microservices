using System;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public interface IPersonRepository
    {
        IQueryable<Person> Get();
        long Insert(Person person);
        bool Exists(string userName);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly IDatabaseContext _databaseContext;


        public PersonRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool Exists(string userName)
        {
            var hasUser = _databaseContext.People
                .Any(x => x.Name.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            return hasUser;
        }

        public IQueryable<Person> Get()
        {
            var people = _databaseContext.People;
            return people;
        }


        public long Insert(Person entity)
        {
            _databaseContext.People.Insert(entity);
            return entity.Id;
        }
    }
}
