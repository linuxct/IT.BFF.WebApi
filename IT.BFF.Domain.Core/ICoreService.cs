using System.Threading.Tasks;
using IT.BFF.Domain.Contracts;

namespace IT.BFF.Domain.Core
{
    public interface ICoreService
    {
        public Task<TelegraphSimplePage> RetrieveTodaysPage();
    }
}