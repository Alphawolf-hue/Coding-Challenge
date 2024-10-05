using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementC_.model
{
    public class CarLoan : Loan
    {
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }

        public CarLoan() { }

        public CarLoan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string carMake, string carModel, int carYear)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, "CarLoan", loanStatus)
        {
            CarMake = carMake;
            CarModel = carModel;
            CarYear = carYear;
        }

        public override string ToString()
        {
            return $"Loan ID: {LoanId}, Customer: {Customer.Name}, Principal: {PrincipalAmount}, " +
                   $"Interest Rate: {InterestRate}%, Term: {LoanTerm} months, Loan Type: {LoanType}, " +
                   $"Loan Status: {LoanStatus}, Car Make: {CarMake}, Car Model: {CarModel}, Car Year: {CarYear}";
        }
    }
}
