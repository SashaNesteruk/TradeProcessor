using AutoFixture;
using Castle.Core.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Models;
using TradeProcessor.Repositories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TradeProcessor.Test.IntegrationTests
{
    public class TradeReaderTests
    {
        private readonly FileManager _fileManager;
        private readonly IEnumerable<Trade> _validTrades = new Trade[] {
            new Trade() { ISIN = "IB1000", Notional = 800, TradeID = "MX1345" },
            new Trade() { ISIN = "IB2000", Notional = 600, TradeID = "MX1346" },
        };

        private readonly Fixture _fixture;
        private readonly IConfiguration _configuration;

        private readonly ITradesReader _sut;

        public TradeReaderTests()
        {
            _fixture = new Fixture();
            _configuration = new ConfigurationBuilder()
                 .AddJsonFile("testconfig.json")
                 .AddEnvironmentVariables()
                 .Build();

            _fileManager = new FileManager();

            _sut = new TradesReader(_configuration);
        }

        [Fact]
        public void ReadTrades_GivenValidFile_Tests()
        {
            // Arrange
            _fileManager.MoveFiles(_configuration.GetValue<string>("InputLocation"), _configuration.GetValue<string>("UploadLocation"));
            // Act
            IEnumerable<Trade> result = _sut.ReadTrades().ToArray();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result?.Count(), _validTrades.Count());
            Assert.Equivalent(result, _validTrades);

            // Clean Up

            _fileManager.MoveFiles(_configuration.GetValue<string>("UploadLocation"), _configuration.GetValue<string>("InputLocation"));
        }

        [Fact]
        public void ReadTrades_GivenCorruptedFile_Tests()
        {
            // Arrange
            _fileManager.MoveFiles(_configuration.GetValue<string>("CorruptLocation"), _configuration.GetValue<string>("UploadLocation"));
            // Act

            var result = _sut.ReadTrades();

            // Assert
            Assert.Empty(result);

            //Clean Up

            _fileManager.MoveFiles(_configuration.GetValue<string>("UploadLocation"), _configuration.GetValue<string>("CorruptLocation"));
        }

        [Fact]
        public void ReadTrades_ArchiveFile_Tests()
        {
            // Arrange
            _fileManager.MoveFiles(_configuration.GetValue<string>("InputLocation"), _configuration.GetValue<string>("UploadLocation"));
            // Act

            _sut.ArchiveTrades();

            // Assert
            File.Exists(_configuration.GetValue<string>("ArchiveLocation") + "/trades.csv").Should().BeTrue();

            //Clean Up

            _fileManager.MoveFiles(_configuration.GetValue<string>("ArchiveLocation"), _configuration.GetValue<string>("InputLocation"));
        }
    }
}
