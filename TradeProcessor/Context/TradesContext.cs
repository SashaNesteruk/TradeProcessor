using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeProcessor.Models;

namespace TradeProcessor.Context
{
    public class TradesContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TradesContext(IConfiguration configuration, DbContextOptions
                dbContextOptions) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        public DbSet<Trade> Trades { get; set; }
    }
}
