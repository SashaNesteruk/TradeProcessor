using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using TradeProcessor.Models;
using TradeProcessor.Repositories;
using TradeProcessor.Services;
using Xunit;

namespace TradeProcessor.Test
{
    public class TradeProcesorTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ITradesReader> _tradesReader;
        private readonly Mock<ITradesRepository> _tradesRepository;
        private readonly Mock<ILogger<TradesProcessorService>> _logger;
        private readonly TradesProcessorService _sut;

        public TradeProcesorTests()
        {
            _tradesReader = new Mock<ITradesReader>();
            _tradesRepository = new Mock<ITradesRepository>();
            _logger = new Mock<ILogger<TradesProcessorService>>();

            _fixture = new Fixture();

            _sut = new TradesProcessorService(_tradesReader.Object, _tradesRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task ProcessTrades_DoesNotProcessEmptyFiles_TestAsync()
        {
            // Arrange
            var trades = new List<Trade>();
            _tradesReader.Setup(x => x.ReadTrades()).Returns(trades);

            //Act
            await _sut.ProcessTrades();

            //Assert
            _tradesReader.Verify(x => x.ReadTrades(), Times.Once);
            _tradesRepository.Verify(x => x.TruncateData(), Times.Never);
            _tradesRepository.Verify(x => x.SaveData(trades), Times.Never);
        }

        [Fact]
        public async Task ProcessTrades_ProcessesFiles_TestAsync()
        {
            // Arrange
            var trades = _fixture.Build<Trade>().CreateMany(5);
            _tradesReader.Setup(x => x.ReadTrades()).Returns(trades);
            _tradesReader.Setup(x => x.ArchiveTrades());
            _tradesRepository.Setup(x => x.SaveData(trades));


            //Act
            await _sut.ProcessTrades();

            //Assert
            _tradesReader.Verify(x => x.ReadTrades(), Times.Once);
            _tradesReader.Verify(x => x.ArchiveTrades(), Times.Once);
            _tradesRepository.Verify(x => x.TruncateData(), Times.Once);
            _tradesRepository.Verify(x => x.SaveData(trades), Times.Once);
        }
    }
}