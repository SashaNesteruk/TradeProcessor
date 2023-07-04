using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeProcessor.Models
{
    public class Trade
    {
        public string TradeID { get; set; }
        public string ISIN { get; set;}
        public int Notional { get; set;}
    }
}
