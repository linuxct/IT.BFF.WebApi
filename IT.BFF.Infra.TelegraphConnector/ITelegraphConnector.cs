using System;
using System.Threading.Tasks;
using IT.BFF.Domain.Contracts;

namespace IT.BFF.Infra.TelegraphConnector
{
    public interface ITelegraphConnector
    {
        public Task<TelegraphSimplePage> RetrieveTodaysPost(DateTimeOffset time);
    }
}