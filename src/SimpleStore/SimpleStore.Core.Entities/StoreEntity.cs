using SimpleStore.Entities;

namespace SimpleStore.Core.Entities
{
    public class StoreEntity: Entity
    {
        public string CreatedBy { get; set; }

        public string StoreId { get; set; }
    }
}
