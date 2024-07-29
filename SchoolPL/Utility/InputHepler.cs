using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryLogManagement.SchoolPL.Utility
{
    internal class InputHepler
    {
        public string PromptUserInput(string promptMessage)
        {
            AnsiConsole.Markup(promptMessage);
            string input = Console.ReadLine();

            return input;
        }
    }
}
