using INetwork.BLL;
using NetworkBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace NetworkMVC.MyRoleProvider
{
    public class NetworkRoleProvider : RoleProvider
    {
        private INetworkLogic NetworkLogic;

        public NetworkRoleProvider()
        {
            NetworkLogic = new NetworkLogic();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (NetworkLogic.IsUserInRole(username, roleName))
            {
                return true;
            }
            return false;
        }

        public override string[] GetRolesForUser(string username)
        {
            return NetworkLogic.GetRoles(username);
        }

        #region NotImplemented
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}