using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TradeProcessor.Models;

namespace TradeProcessor.Repositories
{
    public class TradesReader : ITradesReader
    {
        private readonly IConfiguration _configuration;
        private readonly CsvConfiguration _csvConfiguration;

        public TradesReader(IConfiguration configuration) {
            _configuration = configuration;
            _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ",",
                HasHeaderRecord = true,
                PrepareHeaderForMatch = header => Regex.Replace(header.Header, @"[\s-]+", string.Empty).ToLower()
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
                    try 
                    {
                        using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (var textReader = new StreamReader(fs, Encoding.UTF8))
                            using (var csv = new CsvReader(textReader, _csvConfiguration))
                            {
                                trades.AddRange(csv.GetRecords<Trade>());
                            }
                        }
                    } catch (CsvHelper.ValidationException e) {
                        return trades;
                    }
                    
                }
            }

            return trades;
        }

        public void ArchiveTrades()
        {
            var sourcePath = _configuration.GetValue<string>("UploadLocation");
            var destPath = _configuration.GetValue<string>("ArchiveLocation");
            if (Directory.Exists(sourcePath))
            {
                foreach (var file in new DirectoryInfo(sourcePath).GetFiles())
                {
                    if (!Directory.Exists(destPath)) {
                        Directory.CreateDirectory(destPath);
                    }

                    if (File.Exists(destPath + "/" + file.Name))
                    {
                        File.Delete(destPath + "/" + file.Name);
                    }
                    File.Move(sourcePath + "/" + file.Name, destPath + "/" + file.Name);
                }
            }
        }
    }
}
