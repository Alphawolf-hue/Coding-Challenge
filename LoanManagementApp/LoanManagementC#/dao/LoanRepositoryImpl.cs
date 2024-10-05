using LoanManagementC_.model;
using LoanManagementC_.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementC_.dao
{
    public class LoanRepositoryImpl : ILoanRepository
    {
        private string _connectionString;

        public LoanRepositoryImpl()
        {
            _connectionString = DBConnUtil.GetConnString();
        }

        public bool CustomerExists(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE customerId = @customerId", connection);
                cmd.Parameters.AddWithValue("@customerId", customerId);

                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public Customer GetCustomerById(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Customer WHERE customerId = @customerId", connection);
                cmd.Parameters.AddWithValue("@customerId", customerId);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Assuming Customer has a constructor or properties to set
                    return new Customer
                    {
                        CustomerId = (int)reader["customerId"],
                        Name = (string)reader["name"],
                        EmailAddress = (string)reader["email"],
                        PhoneNumber = (string)reader["phoneNumber"],
                        Address = (string)reader["address"],
                        CreditScore = (int)reader["creditScore"]
                    };
                }
            }
            return null; // Return null if customer is not found
        }
        public bool AddCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO Customer  VALUES (@name, @email_id, @phoneNumber, @address, @creditScore)";
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Email", customer.EmailAddress);
                    cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@CreditScore", customer.CreditScore);
                    cmd.Connection = connection;
                    connection.Open();
                    customer.CustomerId = Convert.ToInt32(cmd.ExecuteScalar()); // Set the ID after insertion
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }


        public bool ApplyForLoan(Loan loan)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.CommandText = "INSERT INTO Loan VALUES (@customerId	,@principalAmount,@interestRate	,@loanTerm,@loanType,@loanStatus)";
                    cmd.Parameters.AddWithValue("@customerId", loan.Customer.CustomerId);
                    cmd.Parameters.AddWithValue("@principalAmount", loan.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@interestRate", loan.InterestRate);
                    cmd.Parameters.AddWithValue("@loanTerm", loan.LoanTerm);
                    cmd.Parameters.AddWithValue("@loanType", loan.LoanType);
                    cmd.Parameters.AddWithValue("@loanStatus", loan.LoanStatus);
                    cmd.Connection = connection;
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public bool UpdateLoan(Loan loan)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "UPDATE loan SET amount = @amount, interest_rate = @interestRate, term = @term, status = @status WHERE loan_id = @loanId",
                        Connection = connection
                    };
                    cmd.Parameters.AddWithValue("@amount", loan.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@interestRate", loan.InterestRate);
                    cmd.Parameters.AddWithValue("@term", loan.LoanTerm);
                    cmd.Parameters.AddWithValue("@status", loan.LoanStatus);
                    cmd.Parameters.AddWithValue("@loanId", loan.LoanId);

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public Loan GetLoanById(int loanId)
        {
            Loan? loan = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM loan WHERE customerId = @customerId";
                    cmd.Parameters.AddWithValue("@loanId", loanId);
                    cmd.Connection = connection;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        loan = new Loan
     (
                      Convert.ToInt32(reader["loan_id"]),
                      new Customer { CustomerId = Convert.ToInt32(reader["@customerId"]) },
                     (decimal)Convert.ToDouble(reader["amount"]),
                     (decimal)Convert.ToDouble(reader["interest_rate"]),
                     Convert.ToInt32(reader["term"]),
                     Convert.ToString(reader["status"]),
         "" // Add a default or a relevant value for the additional parameter
     );
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return loan;
        }
        public bool DeleteLoan(int loanId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "DELETE FROM loan WHERE loan_id = @loanId";
                    cmd.Parameters.AddWithValue("@loanId", loanId);
                    cmd.Connection = connection;

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }


        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM Loan";
                    cmd.Connection = connection;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Loan loan = new Loan
                        (
                        Convert.ToInt32(reader["loanId"]), 
                        new Customer { CustomerId = Convert.ToInt32(reader["customerId"]) }, 
                        (decimal)Convert.ToDouble(reader["principalAmount"]), 
                        (decimal)Convert.ToDouble(reader["interestRate"]), 
                        Convert.ToInt32(reader["loanTerm"]), 
                        Convert.ToString(reader["loanStatus"]),""
                        );

                        loans.Add(loan);
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return loans;
        }
        public List<Loan> GetLoansByCustomer(int customerId)
        {
            List<Loan> loans = new List<Loan>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM loans WHERE customer_id = @customerId";
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Connection = connection;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Loan loan = new Loan
                        (
                            Convert.ToInt32(reader["loan_id"]),
                            new Customer { CustomerId = Convert.ToInt32(reader["customer_id"]) }, // Adjusted to create a Customer object
                            (decimal)Convert.ToDouble(reader["amount"]),
                            (decimal)Convert.ToDouble(reader["interest_rate"]),
                            Convert.ToInt32(reader["term"]),
                            Convert.ToString(reader["status"]),  // Pass the loanStatus here
                            "" // Add a default or relevant value for any additional required parameter
                        );
                        loans.Add(loan);
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return loans;
        }

        public bool LoanRepayment(int loanId, double amount, string paymentDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO loan_payments  VALUES (@loanId, @amount, @paymentDate)";
                    cmd.Parameters.AddWithValue("@loanId", loanId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@paymentDate", DateTime.Parse(paymentDate));
                    cmd.Connection = connection;

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public bool UpdateLoanStatus(int loanId, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "UPDATE loans SET status = @status WHERE loan_id = @loanId";
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@loanId", loanId);
                    cmd.Connection = connection;

                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }
    }
}
