using EntryLogManagement.SchoolBLL;
using EntryLogManagement.SchoolPL.Utility;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMySql.Models;

namespace EntryLogManagement.SchoolPL
{
    internal class UserPL
    {
        private readonly UserService userService;
       

        public UserPL()
        {
            userService = new UserService();
          

        }

        public User Login()
        {
            while (true)
            {
                string UserName = InputHepler.PromptUserInput("Nhập [green]UserName: [/]");
                string Password = InputHepler.PromptUserInput("Nhập [green]Password: [/]");
                Console.WriteLine();

                var reuslt =  userService.LoginUser(UserName, Password);

                if (reuslt != null)
                {
                    AnsiConsole.MarkupLine("[green]Bạn đã đăng nhập thành công[/]");
                    return reuslt;
                    break;
                }
                
            }
                
            
        }

        public  void Register()
        {
            while(true)
            {
                string UserName = InputHepler.PromptUserInput("Nhập [green]UserName: [/]");
                string Password = InputHepler.PromptUserInput("Nhập [green]Password: [/]");
                int ID = InputHepler.GetIntPrompt("Nhập [green]ID của bạn : [/]");
               

                var result = userService.RegisterUser(UserName, Password, ID);
                if (result)
                {
                    AnsiConsole.Markup("[green]Bạn đã đăng kí thành công[/]");
                    Console.WriteLine();
                    break;
                }
                Console.WriteLine();
            }
           
           
        }
    }
}
