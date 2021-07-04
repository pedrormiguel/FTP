using System;
using System.Threading.Tasks;
using FtpClientTest;
using static System.Console;

namespace FtpClientConsole.Utitity
{
    public class ConsoleUtility
    {
        public static void WaitAndClearScreen()
        {
            System.Console.ReadKey();
            System.Console.Clear();
        }

        public static void InsertBlankLine()
        {
            System.Console.WriteLine();
        }

        public static string ReadOption()
        {
            System.Console.Write("option : ");
            return System.Console.ReadLine();
        }

        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }

        public static void RepeatOptions(Action menu)
        {
            InsertBlankLine();
            WriteLine("Select a valid option");
            WaitAndClearScreen();
            menu();
        }

         public static async Task RepeatOptions(Func<Task> menu)
        {
            InsertBlankLine();
            WriteLine("Select a valid option");
            WaitAndClearScreen();
            await menu();
        }

        public static async Task RepeatOptions(Func<FTP, Task> menu, FTP client)
        {
            InsertBlankLine();
            WriteLine("Select a valid option");
            WaitAndClearScreen();
            await menu(client);
        }

    }
}