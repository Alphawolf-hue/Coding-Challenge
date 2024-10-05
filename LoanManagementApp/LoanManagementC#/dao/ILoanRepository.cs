using LoanManagementC_.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementC_.dao
{
    public interface ILoanRepository
    {
        bool ApplyForLoan(Loan loan);
        bool UpdateLoan(Loan loan);
        bool DeleteLoan(int loanId);
        Loan GetLoanById(int loanId);
        List<Loan> GetAllLoans();
        List<Loan> GetLoansByCustomer(int customerId);
        bool LoanRepayment(int loanId, double amount, string paymentDate);
        bool UpdateLoanStatus(int loanId, string status);
        bool CustomerExists(int customerId);
    }
}
