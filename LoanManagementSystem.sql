create database LoanManagementSystem

use LoanManagementSystem

CREATE TABLE Customer (
    customerId INT IDENTITY(1,1) PRIMARY KEY,              
    [name] VARCHAR(100) NOT NULL,              
    email_id VARCHAR(255),      
    phoneNumber VARCHAR(15) NOT NULL,        
    [address] VARCHAR(MAX) NOT NULL,           
    creditScore INT CHECK (creditScore BETWEEN 300 AND 850) 
);

CREATE TABLE Loan (
    loanId INT IDENTITY(1,1) PRIMARY KEY,                  
    customerId INT NOT NULL,                          
    principalAmount DECIMAL(15, 2) NOT NULL, 
    interestRate DECIMAL(5, 2) NOT NULL,     
    loanTerm INT NOT NULL,                   
    loanType VARCHAR(50) CHECK (loanType IN ('CarLoan', 'HomeLoan')), 
    loanStatus VARCHAR(20) CHECK (loanStatus IN ('Pending', 'Approved','Rejected')), 
    CONSTRAINT FK_Customer_Loan FOREIGN KEY (customerId) REFERENCES Customer(customerId) 
);

drop table Loan
select * from Customer
select * from Loan
select * from loan_payments

create table loan_payments(
   lp_id int identity(1,1) not null,
   loanId int not null,
   amount decimal(15,2),
   paymentDate varchar(200),
   foreign key(loanId) references Loan(loanId)
)