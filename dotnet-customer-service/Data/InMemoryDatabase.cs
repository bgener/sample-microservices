using Data.Entities;
using NMemory;
using NMemory.Indexes;
using NMemory.Tables;

namespace Data
{
    public class InMemoryDatabase : Database, IDatabaseContext
    {
        public InMemoryDatabase()
        {
            InitSchema();
        }


        public ITable<Person> People { get; private set; }

        public ITable<Group> Groups { get; private set; }

        public ITable<UserProjection> ActiveUsers { get; set; }


        private void InitSchema()
        {
            var peopleTable = Tables.Create(x => x.Id, new IdentitySpecification<Person>(person => person.Id));
            var groupTable = Tables.Create(x => x.Id, new IdentitySpecification<Group>(group => group.Id));
            var activeUsersTable = Tables.Create(x => x.UserId, new IdentitySpecification<UserProjection>(user => user.UserId));
            
            var peopleGroupIdIndex = peopleTable.CreateIndex(new RedBlackTreeIndexFactory(), person => person.GroupId);

            Tables.CreateRelation(
                groupTable.PrimaryKeyIndex,
                peopleGroupIdIndex,
                x => x,
                x => x,
                new RelationOptions());


            People = peopleTable;
            Groups = groupTable;
            ActiveUsers = activeUsersTable;
        }
    }
}
