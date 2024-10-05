using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementC_.model
{
    public class HomeLoan : Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan() { }

        public HomeLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "HomeLoan", loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public override string ToString()
        {
            return $"Loan ID: {LoanId}, Customer: {Customer.Name}, Principal: {PrincipalAmount}, " +
                   $"Interest Rate: {InterestRate}%, Term: {LoanTerm} months, Loan Type: {LoanType}, " +
                   $"Loan Status: {LoanStatus}, Property Address: {PropertyAddress}, Property Value: {PropertyValue}";
        }
    }
}
