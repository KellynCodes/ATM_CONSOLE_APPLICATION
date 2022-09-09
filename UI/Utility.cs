using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSPROJECT.UI
{
    public static class Utility
    {
        public static long tranId;
        private static CultureInfo culture = new CultureInfo("IG-NG");

        public static long GetTransactionId()
        {
            return ++tranId;
        }
        public static void PrintDotAnimation(int Timer = 10)
        {
            for (int i = 0; i < Timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }
        public static string GetSecretInput(string prompt)
        {
            bool IsPrompt = true;
            string asterics = "";

            StringBuilder input = new StringBuilder();
            while (true)
            {
                if (IsPrompt)
                    Console.WriteLine(prompt);
                IsPrompt = false;
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 4)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease Enter Four Digits.", false);
                        input.Clear();
                        IsPrompt = true;
                        continue;
                    }
                }
                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                }
                else if (inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterics + "*");
                }
            }

            return input.ToString();
        }
        public static void PrintMessage(string msg, bool success = true)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            System.Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }
        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }
        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress Enter to continue...\n");
            Console.ReadLine();
        }

        public static string FormatAmount(decimal amt)
        {
            return String.Format(culture, "{0:C2}", amt);
        }
    }
}