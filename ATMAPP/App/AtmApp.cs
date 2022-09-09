using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using CSPROJECT.UI;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace CSPROJECT.ATMAPP.App// Note: actual namespace depends on the project name.
{
    public class AtmApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount> UserAcountLists;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen Screen;

        public AtmApp()
        {
            Screen = new AppScreen();
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.fullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }

        }
        public void InitializeData()
        {
            UserAcountLists = new List<UserAccount>{
            new UserAccount{
            Id=1,
            fullName = "Kelechi Amos Omeh",
            AccountNumber=232343,
            CardNumber = 3224,
            CardPin = 1111,
            AccountBalance = 200000.00m,
            IsLocked=false },

            new UserAccount {
            Id = 2,
            fullName = "John Kennedy",
            AccountNumber = 654321,
            CardNumber = 222222,
            CardPin = 2222,
            AccountBalance = 100000.00m,
            IsLocked = false },

            new UserAccount {
            Id = 3,
            fullName = "John Doe",
            AccountNumber = 123456,
            CardNumber = 333333,
            CardPin = 3333,
            AccountBalance = 50000.00m,
            IsLocked = true},
          };
            _listOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumAndPassword()
        {
            bool IsCorrectLogin = false;
            UserAccount inputAccount = AppScreen.UserLoginForm();
            AppScreen.loginProgress();
            foreach (UserAccount account in UserAcountLists)
            {
                selectedAccount = account;
                if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                {
                    selectedAccount.TotalLogin++;
                    if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                    {
                        selectedAccount = account;
                        if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                        {
                            //Print a lock message
                            AppScreen.PrintLockScreen();
                        }
                        else
                        {
                            selectedAccount.TotalLogin = 0;
                            IsCorrectLogin = true;
                        }
                    }
                }
            }
            if (IsCorrectLogin == false)
            {
                Utility.PrintMessage("\nInvalid Card Number or Pin", false);
                selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                if (selectedAccount.IsLocked)
                {
                    AppScreen.PrintLockScreen();
                }
            }
            Console.Clear();

        }
        private void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("An option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = Screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogOutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM CARD");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid Option.", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiple of 500 and 1000 naira allowed. \n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            //simulate counting
            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            //some gaurd clause
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiples of 500 or 1000. Try again.", false);
                return;
            }

            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled you action.", false);
                return;
            }

            //bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            //udpate account balance
            selectedAccount.AccountBalance += transaction_amt;

            //print success message on the console
            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was succesful.", true);


        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if (selectedAmount == -1)
            {
                // selectedAmount = AppScreen.SelectAmount();\
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            }

            //input validation 
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("You can only withdraw in myltiples of 500 or 1000 naira. Try Again.", false);
                return;
            }
            //Business logic validations
            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw" + $"{Utility.FormatAmount(transaction_amt)}.", false);
                return;
            }
            if ((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " + $"minimum{Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            //Bind withdrawal details tgo transactions object 
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;
            //success message
            Utility.PrintMessage($"You have successfully withdrawn" + $"{Utility.FormatAmount(transaction_amt)}.", true);
        }

        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{AppScreen.cur} 1000 x {thousandNotesCount} = {thousandNotesCount}");
            Console.WriteLine($"{AppScreen.cur} 500 X {fiveHundredNotesCount} = {5000 * fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");
            int opt = Validator.Convert<int>("1 to Confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            //create a new transaciton object;
            var transaciton = new Transaction()
            {
                TransactionID = Utility.GetTransactionId(),
                UserBankingAccountID = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc,

            };

            //add transaction object to the list 
            _listOfTransactions.Add(transaciton);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankingAccountID == selectedAccount.Id).ToList();
            //check if thier is transaction 
            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transactrion Date", "Type", "Description", "Amount " + AppScreen.cur);
                foreach (var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionID, tran.TransactionDate, tran.TransactionType, tran.Description, tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
            //check sender's accout balance
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance" + $"to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}");
                return;
            }

            //check the minimum amount kept amount.
            if ((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account needs a to have minimum" + $"{Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            //check if receivers bank account number is valid 
            var selectedBankAccountReceiver = (from userAcc in UserAcountLists where userAcc.AccountNumber == internalTransfer.RecepientBankAccountNumber select userAcc).FirstOrDefault();
            if (selectedBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer failed. Receiver bank account number is invalid.", false);
                return;
            }
            //check if receiver's name 
            if (selectedBankAccountReceiver.fullName != internalTransfer.RecepientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Receipient's Bank account name does not match.");
                return;
            }
            //add transaction to transaction record sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered" + $"To {selectedBankAccountReceiver.AccountNumber}({selectedBankAccountReceiver.fullName})");

            //update sender's balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //add transaction record receiver
            InsertTransaction(selectedBankAccountReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from" + $"{selectedAccount.AccountNumber}({selectedAccount.fullName})");

            //update receiver's account balance
            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            //print success message
            Utility.PrintMessage($"You have successfully transfered" + $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " + $"{internalTransfer.RecepientBankAccountName}", true);

        }
    }

}