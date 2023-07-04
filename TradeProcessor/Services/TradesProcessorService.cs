using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Models;
using TradeProcessor.Repositories;

namespace TradeProcessor.Services
{
    public class TradesProcessorService : ITradesProcessorService
    {
        private readonly ITradesReader _tradesReader;
        private readonly ITradesRepository _tradesRepository;

        public TradesProcessorService(ITradesReader tradesReader, ITradesRepository tradesRepository) {
            _tradesReader = tradesReader;
            _tradesRepository = tradesRepository;
        }

        public void ProcessTrades()
        {
            IEnumerable<Trade> trades = _tradesReader.ReadTrades();

            if (trades.Count() > 0) {
                _tradesRepository.TruncateData();
                _tradesRepository.SaveData(trades);

                _tradesReader.ArchiveTrades();
            }
        }
    }
}
