using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeProcessor.Services
{
    internal interface ITradesProcessorService
    {
        Task ProcessTrades();
    }
}
