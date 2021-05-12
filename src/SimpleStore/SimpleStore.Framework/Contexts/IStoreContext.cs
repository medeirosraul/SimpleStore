using SimpleStore.Core.Entities.Stores;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Contexts
{
    public interface IStoreContext
    {
        public Store CurrentStore { get; }

        public Task SetCurrentStore();

        public string GetHost();

        public string GetSubDomain();

        public bool IsSimpleStore();

        public bool IsSubDomain();

        public bool IsCustomDomain();
    }
}
