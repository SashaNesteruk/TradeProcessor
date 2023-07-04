using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Models;

namespace TradeProcessor.Repositories
{
    public interface ITradesRepository
    {
        void TruncateData();
        void SaveData(IEnumerable<Trade> trades);
    }
}
