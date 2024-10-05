using LoanManagementC_.model;
using LoanManagementC_.dao;
using LoanManagementC_.util;
using System;
using System.Collections.Generic;

namespace LoanManagementC_.MainModule
{
    public class LoanManagementMenu
    {
        private readonly ILoanRepository _loanRepository;

        public LoanManagementMenu()
        {
            // Instantiate the loan repository implementation
            _loanRepository = new LoanRepositoryImpl();
        }

        public void DisplayMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Loan Management System -- Welcome User");
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Apply for Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ApplyForLoan();
                            break;
                        case 2:
                            GetAllLoans();
                            break;
                        case 3:
                            GetLoanById();
                            break;
                        case 4:
                            LoanRepayment();
                            break;
                        case 5:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void ApplyForLoan()
        {
            Console.WriteLine("Enter Customer Name:");
            string customerName = Console.ReadLine();
            Console.WriteLine("Enter Principal Amount:");
            decimal principalAmount = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Interest Rate (%):");
            decimal interestRate = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Loan Term (months):");
            int loanTerm = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Loan Type (e.g., HomeLoan, CarLoan):");
            string loanType = Console.ReadLine();

            Customer customer = new Customer { Name = customerName }; // Assuming Customer class has a Name property
            Loan newLoan;

            if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter Property Address:");
                string propertyAddress = Console.ReadLine();
                Console.WriteLine("Enter Property Value:");
                int propertyValue = int.Parse(Console.ReadLine());

                newLoan = new HomeLoan(0, customer, principalAmount, interestRate, loanTerm, "Pending", propertyAddress, propertyValue);
            }
            else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter Car Make:");
                string carMake = Console.ReadLine();
                Console.WriteLine("Enter Car Model:");
                string carModel = Console.ReadLine();
                Console.WriteLine("Enter Car Year:");
                int carYear = int.Parse(Console.ReadLine());

                newLoan = new CarLoan(0, customer, principalAmount, interestRate, loanTerm, "Pending", carMake, carModel, carYear);
            }
            else
            {
                Console.WriteLine("Invalid Loan Type.");
                return;
            }
            bool isLoanApplied = _loanRepository.ApplyForLoan(newLoan);
            Console.WriteLine(isLoanApplied ? "Loan applied successfully." : "Failed to apply for loan.");
        }

        private void GetAllLoans()
        {
            List<Loan> loans = _loanRepository.GetAllLoans();
            foreach (var loan in loans)
            {
                Console.WriteLine(loan.ToString());
            }
        }

        private void GetLoanById()
        {
            Console.WriteLine("Enter Customer ID:");
            int loanId = int.Parse(Console.ReadLine());
            Loan loan = _loanRepository.GetLoanById(loanId);

            if (loan != null)
            {
                Console.WriteLine(loan.ToString());
            }
            else
            {
                Console.WriteLine("Loan not found.");
            }
        }

        private void LoanRepayment()
        {
            Console.WriteLine("Enter Loan ID for repayment:");
            int loanId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Repayment Amount:");
            decimal repaymentAmount = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Payment Date");
            string paymentDate=Console.ReadLine();

            bool isRepaymentProcessed = _loanRepository.LoanRepayment(loanId, (double)repaymentAmount,paymentDate);
            Console.WriteLine(isRepaymentProcessed ? "Repayment processed successfully." : "Failed to process repayment.");
        }
    }
}
