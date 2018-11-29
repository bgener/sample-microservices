using Data.Entities;
using NMemory.Tables;

namespace Data
{
    public interface IDatabaseContext
    {
        ITable<Person> People { get; }

        ITable<Group> Groups { get; }

        ITable<UserProjection> ActiveUsers { get; }
    }
}