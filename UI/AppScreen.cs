using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSPROJECT.ATMAPP.App;
using Domain.Entities;
namespace CSPROJECT.UI
{
    public class AppScreen
    {

        internal const string cur = "N ";
        internal static void Welcome()
        {
            //clears the console
            Console.Clear();
            //sets the title of the console app
            Console.Title = "KellynCode Banking System";
            //sets the text color of foreground color to white;
            Console.ForegroundColor = ConsoleColor.White;

            //set the welcome message
            Console.WriteLine("\n\n---------Welcome to Back--------------------\n");
            //prompt the user to insert atm card;
            Console.WriteLine("Please Insert Your Atm Card Details");
            Console.WriteLine("Note: Actual ATM machine will accept and validate a physical ATM card, read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("Your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter Your Card Pin"));
            return tempUserAccount;
        }

        internal static void loginProgress()
        {
            Console.WriteLine("\nChecking Card number and Pin...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your Account Is Locked. Please nearest branch  unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }
        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome Back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine(".........My Atm App Menu");
            Console.WriteLine(":                       :");
            Console.WriteLine("1. Account Balance      :");
            Console.WriteLine("2. Cash Deposit         :");
            Console.WriteLine("3. Withdrawal           :");
            Console.WriteLine("4. Transfer             :");
            Console.WriteLine("5. Transaction          :");
            Console.WriteLine("6. Logout               :");
            Utility.PressEnterToContinue();
        }
        internal static void LogOutProgress()
        {
            Console.WriteLine("Thank you for using KellynTech.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1, {0}500    5:{0}10,000", cur);
            Console.WriteLine(":2, {0}1000    6:{0}15,000", cur);
            Console.WriteLine(":3, {0}2000    7:{0}20,000", cur);
            Console.WriteLine(":4 {0}5000     8{0}40,000", cur);
            Console.WriteLine(":0.Other", cur);
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("Option:");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Your Entered Invalid Input. Try again.", false);
                    return -1;
                    break;

            }
        }

        internal InternalTransfer InternalTransferForm()
        {
            var internTransfer = new InternalTransfer();
            internTransfer.RecepientBankAccountNumber = Validator.Convert<long>("recipient's acount number: ");
            internTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
            internTransfer.RecepientBankAccountName = Utility.GetUserInput("Recepient's name:");
            return internTransfer;
        }
    }
}