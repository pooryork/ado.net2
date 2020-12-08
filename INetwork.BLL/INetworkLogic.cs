using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetwork.BLL
{
    public interface INetworkLogic
    {

        int SingUp(User user);

        void AddFriend(int? IdUser, int? IdFriend);

        void DeleteFriend(int? IdUser, int? IdFriend);

        IEnumerable<UserSearch> SearchByName(string Name);

        IEnumerable<UserSearch> SearchBySurname(string Surname);

        IEnumerable<UserSearch> SearchByTown(string Town);

        IEnumerable<UserSearch> SearchByPhone(string Phone);

        IEnumerable<Message> GetMessagesByFriend(int? IdUser, int? IdFriend);

        IEnumerable<Friend> GetAllFriends(string username);

        User GetByLogin(string username);

        User GetById(int? id);

        void Edit(User user);

        bool IsUserInRole(string username, string roleName);

        string[] GetRoles(string username);

        void SendMessage(int? userId, int? friendId, string message);

        User LogIn(string login, string password);

        void DeleteMessages();

        void DeleteUsers();

        IEnumerable<Message> GetAllMessages();

        IEnumerable<User> GetAllUsers();
    }
}