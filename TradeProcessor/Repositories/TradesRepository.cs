using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Models;

namespace TradeProcessor.Repositories
{
    public class TradesRepository : ITradesRepository
    {
        public Task SaveData(IEnumerable<Trade> trades)
        {
            throw new NotImplementedException();
        }

        public Task TruncateData()
        {
            throw new NotImplementedException();
        }
    }
}
