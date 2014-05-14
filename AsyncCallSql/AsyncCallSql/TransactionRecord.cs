using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsyncCallSql
{
    [Serializable]
    public class TransactionRecord
    {
        public string TransactionIdField { get; set; }

        public string AccountNumberField { get; set; }

        public string SubaccountNumberField { get; set; }

        public string AccountNameField { get; set; }

        public decimal TransactionAmountField { get; set; }

        public DateTime TransactionTimeField { get; set; }

        public string BillerConceptIdField { get; set; }

        public string ReferenceField { get; set; }

        public string TransactionMethodField { get; set; }

        public string DocumentNumberField { get; set; }

        public string BankIdField { get; set; }

        public string BankNameField { get; set; }

        public string CashierRegisterField { get; set; }

        public string BranchField { get; set; }
    }
}