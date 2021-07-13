using System;
using System.Threading.Tasks;
using FTPLib;
using static System.Console;

namespace ConsoleApp.Utility
{
    public static class ConsoleUtility
    {
        public static void WaitAndClearScreen()
        {
            ReadKey();
            Clear();
        }

        public static void InsertBlankLine()
        {
            WriteLine();
        }

        public static string ReadOption()
        {
            Write("option : ");
            return ReadLine();
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