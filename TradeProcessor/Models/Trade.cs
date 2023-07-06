using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeProcessor.Models
{
    public class Trade
    {
        [Name("TradeID")]
        public string TradeID { get; set; }
        [Name("ISIN")]
        public string ISIN { get; set;}
        [Name("Notional")]
        public decimal Notional { get; set;}
    }
}
