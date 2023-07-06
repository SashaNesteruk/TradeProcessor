using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TradesProcessorService> _logger;

        public TradesProcessorService(ITradesReader tradesReader, ITradesRepository tradesRepository, ILogger<TradesProcessorService> logger) {
            _tradesReader = tradesReader;
            _tradesRepository = tradesRepository;
            _logger = logger;
        }

        public async Task ProcessTrades()
        {
            IEnumerable<Trade> trades = _tradesReader.ReadTrades();

            if (trades.Count() > 0) {
                _logger.LogInformation("New trades detected", trades);

                await _tradesRepository.TruncateData();
                await _tradesRepository.SaveData(trades);

                _tradesReader.ArchiveTrades();
            }
        }
    }
}
