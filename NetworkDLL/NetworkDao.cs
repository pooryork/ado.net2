using Entities;
using INetwork.DLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDLL
{
    public class NetworkDao : INetworkDao
    {
        private string connectionString = "Data Source=DESKTOP-60HJP9E;Initial Catalog=Network;Integrated Security=True";

        public NetworkDao()
        {
        }

        public void AddFriend(int? IdUser, int? IdFriend)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddFriend", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUser", IdUser);
                cmd.Parameters.AddWithValue("@IDFriend", IdFriend);

                connection.Open();

                cmd.ExecuteNonQuery();
                LoggerUtil.getLog("Logger").Info("Friend was successfully added!");
            }
        }

        public void DeleteFriend(int? IdUser, int? IdFriend)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteFriend", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUser", IdUser);
                cmd.Parameters.AddWithValue("@IDFriend", IdFriend);

                connection.Open();

                cmd.ExecuteNonQuery();
                LoggerUtil.getLog("Logger").Info("Friend was successfully deleted!");
            }
        }

        public void Edit(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("EditUser", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Patronymic", ((object)user.Patronymic) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@YearOfBirth", ((object)user.YearOfBirth) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", ((object)user.PhoneNumber) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Town", ((object)user.Town) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", ((object)user.Gender) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Login", user.Login);
                cmd.Parameters.AddWithValue("@Id", user.IDUser);

                connection.Open();

                cmd.ExecuteNonQuery();
            }
            LoggerUtil.getLog("Logger").Info($"User with login={user.Login} successfully changed personal info!");
        }

        public IEnumerable<Friend> GetAllFriends(string username)
        {
            var result = new List<Friend>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllFriends", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", username);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var f = new Friend
                    {
                        IDUser = (int?)reader["IDFriend"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"],
                        TermOfFriend = (DateTime)reader["Term_Friends"],
                    };

                    result.Add(f);
                }
            }
            LoggerUtil.getLog("Logger").Info($"User with login={username} got his friends!");
            return result;
        }

        public User GetByLogin(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetByLogin", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", username);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                User u = null;
                if (reader.Read())
                {
                    u = new User
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"],
                        Login = username
                    };
                }
                LoggerUtil.getLog("Logger").Info($"User with login={username} was taken from db!");
                return u;
            }
        }

        public IEnumerable<Message> GetMessagesByFriend(int? IdUser, int? IdFriend)
        {
            var result = new List<Message>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetMessagesByIds", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUser", IdUser);
                cmd.Parameters.AddWithValue("@IDFriend", IdFriend);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var m = new Message
                    {
                        IDUser = (int?)reader["IDUser"],
                        IDFriend = (int?)reader["IDFriend"],
                        MessageValue = (string)reader["Message"],
                        MessageDate = (DateTime)reader["DateOfMessage"],
                    };

                    result.Add(m);
                }
            }
            return result;
        }

        public string[] GetRoles(string username)
        {
            var result = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetUsetRoles", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", username);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    result.Add((string)reader["Role"]);
                }
            }
            return result.ToArray();
        }

        public bool IsUserInRole(string username, string roleName)
        {
            foreach (string r in GetRoles(username))
            {
                if (r.Equals(roleName))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<UserSearch> SearchByName(string Name)
        {
            var result = new List<UserSearch>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SearchByName", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", ((object)Name) ?? DBNull.Value);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var f = new UserSearch
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"]
                    };
                    result.Add(f);
                }
            }
            return result;
        }

        public IEnumerable<UserSearch> SearchBySurname(string Surname)
        {
            var result = new List<UserSearch>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SearchBySurname", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Surname", ((object)Surname) ?? DBNull.Value);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var f = new UserSearch
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"]
                    };
                    result.Add(f);
                }
            }
            return result;
        }

        public IEnumerable<UserSearch> SearchByTown(string Town)
        {
            var result = new List<UserSearch>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SearchByTown", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Town", ((object)Town) ?? DBNull.Value);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var f = new UserSearch
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"]
                    };
                    result.Add(f);
                }
            }
            return result;
        }

        public IEnumerable<UserSearch> SearchByPhone(string Phone)
        {
            var result = new List<UserSearch>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SearchByPhone", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PhoneNumber", ((object)Phone) ?? DBNull.Value);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var f = new UserSearch
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"]
                    };
                    result.Add(f);
                }
            }
            return result;
        }

        public int SingUp(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddUser", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Patronymic", ((object)user.Patronymic) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@YearOfBirth", ((object)user.YearOfBirth) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", ((object)user.PhoneNumber) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Town", ((object)user.Town) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", ((object)user.Gender) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Login", user.Login);
                var id = new SqlParameter
                {
                    Direction = System.Data.ParameterDirection.Output,
                    ParameterName = "@Id",
                    DbType = System.Data.DbType.Int32
                };
                cmd.Parameters.Add(id);

                connection.Open();

                cmd.ExecuteNonQuery();
                LoggerUtil.getLog("Logger").Info($"Some new user with login = {user.Login}!");
                return (int)id.Value;
            }
        }

        public Message SendMessage(int? userId, int? friendId, string message)
        {
            DateTime now = DateTime.Now;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SendMessage", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDUser", userId);
                cmd.Parameters.AddWithValue("@IDFriend", friendId);
                cmd.Parameters.AddWithValue("@Message", ((object)message) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DateOfMessage", now);

                connection.Open();

                cmd.ExecuteNonQuery();
            }
            return new Message(userId, friendId, message, now);
        }

        public User LogIn(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("LogIn", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", password);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                User u = null;
                if (reader.Read())
                {
                    u = new User
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"]
                    };
                }
                LoggerUtil.getLog("Logger").Info($"User with login={login} logged in app!");
                return u;
            }
        }

        public User GetById(int? id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetById", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                User u = null;
                if (reader.Read())
                {
                    u = new User
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value)? null: (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"],
                        Login = (string)reader["Login"]
                    };
                }
                return u;
            }
        }

        public void DeleteMessages()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteMessages", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                cmd.ExecuteNonQuery();
                LoggerUtil.getLog("Logger").Info("All messages was successfully deleted!");
            }
        }

        public void DeleteUsers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteUsers", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                cmd.ExecuteNonQuery();
                LoggerUtil.getLog("Logger").Info("All users was successfully deleted!");
            }
        }

        public IEnumerable<Message> GetAllMessages()
        {
            var result = new List<Message>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllMessages", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var m = new Message
                    {
                        IDUser = (int?)reader["IDUser"],
                        IDFriend = (int?)reader["IDFriend"],
                        MessageValue = (string)reader["Message"],
                        MessageDate = (DateTime)reader["DateOfMessage"],
                    };

                    result.Add(m);
                }
            }
            return result.AsEnumerable();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var result = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllUsers", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var m = new User
                    {
                        IDUser = (int?)reader["IDUser"],
                        Name = (string)reader["Name"],
                        Surname = (string)reader["Surname"],
                        Patronymic = reader["Patronymic"].Equals(DBNull.Value) ? null : (string)reader["Patronymic"],
                        Town = reader["Town"].Equals(DBNull.Value) ? null : (string)reader["Town"],
                        Gender = reader["Gender"].Equals(DBNull.Value) ? null : (bool?)reader["Gender"],
                        YearOfBirth = reader["YearOfBirth"].Equals(DBNull.Value) ? null : (int?)reader["YearOfBirth"],
                        PhoneNumber = reader["PhoneNumber"].Equals(DBNull.Value) ? null : (string)reader["PhoneNumber"],
                        Login = (string)reader["Login"],
                        Password = (string)reader["Password"]
                    };

                    result.Add(m);
                }
            }
            return result.AsEnumerable();
        }
    }
}