using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using eStationCore.Model.Security.Entity;
using eStationCore.Model.Security.Enums;

namespace eStationCore.IManagers
{
    public interface ISecurityManager
    {
        MembershipCreateStatus AddUser(User user);
        bool Authenticate(string userName, string userPassword);
        bool ChangePassWord(string userName, string oldPass, string newPass);
        bool Clearance(string permission, bool isCleared = true, Guid? userGuid = default(Guid?));
        User GetUser(Guid profileGuid);
        IEnumerable GetUserClearances(Guid profileGuid, UserSpace userSpace);
        byte[] GetUserPic(string userName);
        string GetUserQuestion(string username);
        bool IsApproved(Guid userGuid, bool isApproved = true);
        string ResetPassword(string username, string questionAnswer);
        bool SpaceClearance(UserSpace userSpace, bool isCleared = true, Guid? userGuid = default(Guid?));
        bool UpdateUser(User user, string newPass = null);
        List<KeyValuePair<string, Enum>> UserSpaces(string userName);
        IEnumerable UserSpaces(Guid profileGuid);
    }
}