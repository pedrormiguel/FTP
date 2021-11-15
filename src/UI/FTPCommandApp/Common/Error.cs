using System.Collections.Generic;
using CliFx.Infrastructure;

namespace CommandFtpApp.Common
{
    public static class Error
    {
        public static void ShowError(this IConsole console, string errorMessage, IEnumerable<string> errors)
        {
            console.Error.WriteLine($"Not was successful. Error  {errorMessage}");

            foreach (var error in errors)
            {
                console.Error.WriteLine($"*{error}\n");
            }
        }
    }
}