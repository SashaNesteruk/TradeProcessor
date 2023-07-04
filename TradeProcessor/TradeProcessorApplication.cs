using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Services;

namespace TradeProcessor
{
    public class TradeProcessorApplication : BackgroundService
    {
        ITradesProcessorService _tradesProcessorService;

        public TradeProcessorApplication()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _tradesProcessorService.ProcessTrades();
                await Task.Delay(1_000, stoppingToken);
            }
        }
    }
}
