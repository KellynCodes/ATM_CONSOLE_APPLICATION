using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Transaction
    {
        public long TransactionID { get; set; }
        public long UserBankingAccountID { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public Decimal TransactionAmount { get; set; }
    }
}