using EntryLogManagement.SchoolDAL;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryLogManagement.SchoolBLL
{
    internal class UserService : BaseService
    {
        private readonly HandleLogRepository handleLogRepository;

        public UserService()
        {
            handleLogRepository = new HandleLogRepository();
        }

        public bool RegisterUser(string username, string password, int parentId)
        {
            if (handleLogRepository.HandleUserName(username))
            {
                return false; // Tên người dùng đã tồn tại
            }

            return handleLogRepository.HandleRegister(username, password, parentId);
        }

        public int LoginUser(string username , string password)
        {

            return handleLogRepository.HandleLogin(username, password);
        }
    }
}
