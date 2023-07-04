using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Text;
using TradeProcessor.Models;

namespace TradeProcessor.Repositories
{
    internal class TradesReader : ITradesReader
    {
        private readonly IConfiguration _configuration;
        private readonly CsvConfiguration _csvConfiguration;

        public TradesReader(IConfiguration configuration) {
            _configuration = configuration;
            _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
                Delimiter = "," // The delimiter is a comma.
            };
        }
        public IEnumerable<Trade> ReadTrades()
        {
            List<Trade> trades = new List<Trade>();
            var path = _configuration.GetValue<string>("UploadLocation");
            if (!string.IsNullOrEmpty(path))
            {
                string[] fileNames = Directory.GetFiles(path, "*.csv");
                foreach (string fileName in fileNames)
                {
                    using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var textReader = new StreamReader(fs, Encoding.UTF8))
                        using (var csv = new CsvReader(textReader, _csvConfiguration))
                        {
                            trades.AddRange(csv.GetRecords<Trade>());
                        }
                    }
                }
            }

            return trades;
        }

        public void ArchiveTrades()
        {
            throw new NotImplementedException();
        }
    }
}
