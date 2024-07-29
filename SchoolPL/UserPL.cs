using EntryLogManagement.SchoolBLL;
using EntryLogManagement.SchoolPL.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryLogManagement.SchoolPL
{
    internal class UserPL
    {
        private readonly UserService userService;
        private readonly InputHepler inputHepler;

        public UserPL()
        {
            userService = new UserService();
            inputHepler = new InputHepler();

        }

        public int Login()
        {
            string UserName = inputHepler.PromptUserInput("Nhập [green]UserName :[/]");
            string Password = inputHepler.PromptUserInput("Nhập [green]Password :[/]");

            return userService.LoginUser(UserName, Password);
        }

        public  void Register()
        {
            string UserName = inputHepler.PromptUserInput("Nhập [green]UserName :[/]");
            string Password = inputHepler.PromptUserInput("Nhập [green]Password :[/]");
            int ID = Convert.ToInt32(Console.ReadLine());

            userService.RegisterUser(UserName, Password, ID);
        }
    }
}
