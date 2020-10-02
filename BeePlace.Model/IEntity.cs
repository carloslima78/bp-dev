using BeePlace.Infra.DataBaseUtils;

namespace BeePlace.Model
{
    public interface IEntity
    {
        [DapperKey]
        int Id { get; set; }

        string DateCreated { get; set; }

        string DateUpdated { get; set; }
    }
}
