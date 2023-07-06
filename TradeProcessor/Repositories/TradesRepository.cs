using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Context;
using TradeProcessor.Models;

namespace TradeProcessor.Repositories
{
    public class TradesRepository : ITradesRepository
    {
        private readonly TradesContext _context;
        private readonly IConfiguration _configuration;

        public TradesRepository(TradesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Task SaveData(IEnumerable<Trade> trades)
        {
            _context.AddRange(trades);
            return _context.SaveChangesAsync();
        }

        public Task TruncateData()
        {
            string tableName = _configuration.GetValue<string>("TruncationTable");
            _context.Database.ExecuteSqlRaw("DELETE FROM " + tableName);

            return _context.SaveChangesAsync();
        }
    }
}
