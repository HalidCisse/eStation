using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web.Security;
using CLib;
using eStationCore.IManagers;
using eStationCore.Model;
using eStationCore.Model.Common.Views;
using eStationCore.Model.Security.Entity;
using eStationCore.Model.Security.Enums;

namespace eStationCore.Store.SqlServer
{
    public class SecurityManager : ISecurityManager
    {
        private readonly StationContext Db;

        public SecurityManager(StationContext stationContext)
        {
            Db = stationContext;
        }

        #region CRUD

        /// <summary>
        /// Authenticate l'utilisateur
        /// </summary>
        /// <param name="userName">Pseudo</param>
        /// <param name="userPassword">Mot de Passe</param>
        /// <returns>True si l'operation success</returns>
        /// <exception cref="SecurityException">CAN_NOT_FIND_USER</exception>
        public bool Authenticate(string userName, string userPassword)
        {           
            try
            {
                //using (var db = new StationContext())
                //{
                //    if (db.Database.Exists())
                //    {
                //        db.Database.Delete();
                //        db.Database.Create();
                //    }
                //}

                if (!Membership.ValidateUser(userName, userPassword))
                    {
                        if (Membership.GetAllUsers().Count != 0) return false;
                        MembershipCreateStatus status;
                        Membership.CreateUser(
                            "admin",
                            "admin00.",
                            "admin@admin.com",
                            "admin",
                            "admin",
                            true,
                            new Guid("53f258a3-f931-4975-b6ec-17d26aa95848"),
                            out status);
                        if (status != MembershipCreateStatus.Success) return false;
                        Roles.CreateRole(AdminClearances.SuperUser.ToString());
                        Roles.CreateRole(AdminClearances.StaffWrite.ToString());
                        Roles.CreateRole(UserSpace.AdminSpace.ToString());

                        Roles.AddUserToRole("admin", AdminClearances.SuperUser.ToString());
                        Roles.AddUserToRole("admin", AdminClearances.StaffWrite.ToString());
                        Roles.AddUserToRole("admin", UserSpace.AdminSpace.ToString());
                        return false;
                    }

                var user = Membership.GetUser(userName);
                if (user == null)
                    throw new SecurityException("CAN_NOT_FIND_USER");

                var identity = new GenericIdentity(user.UserName);
                var principal = new RolePrincipal(identity);
                Thread.CurrentPrincipal = principal;
                return true;
            }
            catch (SqlException sqlException)
            {
                DebugHelper.WriteException(sqlException);
                throw;
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                return false;
            }
        }


        /// <summary>
        /// Creer un Profile d'utilisateur
        /// </summary>
        /// <returns>true pour success</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.SuperUser)]
        public MembershipCreateStatus AddUser(User user)
        {
            MembershipCreateStatus status;
            if (!Membership.RequiresQuestionAndAnswer)
            {
                user.PasswordQuestion = "";
                user.PasswordAnswer = "";
            }

            if (user.UserGuid == Guid.Empty)
                user.UserGuid = Guid.NewGuid();
            user.IsApproved = true;

            Membership.CreateUser(
                user.UserName,
                user.Password,
                user.EmailAdress,
                user.PasswordQuestion,
                user.PasswordAnswer,
                user.IsApproved,
                user.UserGuid,
                out status);

            if (status != MembershipCreateStatus.Success) return status;

            SpaceClearance(UserSpace.AdminSpace, true, user.UserGuid);
            return status;
        }


        /// <summary>
        /// Modifier Les Informations d'un Utilisateur
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPass">Le Nouveau Mot de Pass</param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.SuperUser)]
        public bool UpdateUser(User user, string newPass = null)
        {
            var userToMod = Membership.GetUser(user.UserName);

            if (userToMod == null) throw new InvalidOperationException("CAN_NOT_FIND_USER");

            if (Membership.RequiresQuestionAndAnswer && !string.IsNullOrEmpty(user.PasswordQuestion))
                userToMod.ChangePasswordQuestionAndAnswer(user.Password, user.PasswordQuestion, user.PasswordAnswer);

            userToMod.IsApproved = user.IsApproved;

            if (!string.IsNullOrEmpty(newPass)) userToMod.ChangePassword(user.Password, newPass);

            if (string.IsNullOrEmpty(user.EmailAdress)) return true;
            userToMod.Email = user.EmailAdress;
            Membership.UpdateUser(userToMod);

            return true;
        }


        /// <summary>
        /// Blocker un utilisateur
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.SuperUser)]
        public bool IsApproved(Guid userGuid, bool isApproved = true)
        {
            var user = Membership.GetUser(userGuid);
            if (user == null) return true;
            user.IsApproved = isApproved;
            Membership.UpdateUser(user);
            return user.UnlockUser();
        }


        /// <summary>
        /// Retirer la permission de quelqu'un 
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="isCleared"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.SuperUser)]
        public bool Clearance(string permission, bool isCleared = true, Guid? userGuid = null)
        {
            if (userGuid == null && Membership.GetUser() == null)
                return false;

            var userName = userGuid != null ? Membership.GetUser(userGuid)?.UserName : Membership.GetUser()?.UserName;

            if (string.IsNullOrEmpty(permission))
                throw new SecurityException("CLEARENCE_CAN_NOT_BE_EMPTY");

            if (!Roles.GetAllRoles().Contains(permission))
                Roles.CreateRole(permission);

            if (isCleared)
            {
                if (!Roles.IsUserInRole(userName, permission))
                    Roles.AddUserToRole(userName, permission);
            }
            else
            {
                if (!Roles.IsUserInRole(userName, permission)) return true;

                if (permission.Equals(SecurityClearances.SuperUser) && Roles.GetUsersInRole(permission).Count() == 1)
                    throw new SecurityException("SYSTEM_MUST_HAVE_AT_LEAST_ONE_SUPERUSER");

                Roles.RemoveUserFromRole(userName, permission);
            }
            return true;
        }


        /// <summary>
        /// Retirer la permission de quelqu'un 
        /// </summary>
        /// <param name="userSpace"></param>
        /// <param name="isCleared"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = SecurityClearances.SuperUser)]
        public bool SpaceClearance(UserSpace userSpace, bool isCleared = true, Guid? userGuid = null)
        {
            if (userGuid == null && Membership.GetUser() == null)
                return false;
            var userName = userGuid != null ? Membership.GetUser(userGuid)?.UserName : Membership.GetUser()?.UserName;

            if (!Roles.GetAllRoles().Contains(userSpace.ToString()))
                Roles.CreateRole(userSpace.ToString());

            if (isCleared)
            {
                if (!Roles.IsUserInRole(userName, userSpace.ToString()))
                    Roles.AddUserToRole(userName, userSpace.ToString());
            }
            else
            {
                if (!Roles.IsUserInRole(userName, userSpace.ToString())) return true;

                if (Enum.GetValues(typeof(UserSpace))
                        .Cast<object>()
                        .Count(space => Roles.IsUserInRole(userName, space.ToString())) < 2)
                    throw new InvalidOperationException("USER_MUST_HAVE_AT_LEAST_ONE_SPACE");

                if (userSpace == UserSpace.AdminSpace && Roles.GetUsersInRole(UserSpace.AdminSpace.ToString()).Count() < 2)
                    throw new InvalidOperationException("SYSTEM_MUST_HAVE_AT_LEAST_ONE_ADMIN");

                Roles.RemoveUserFromRole(userName, userSpace.ToString());

                switch (userSpace)
                {
                    case UserSpace.AdminSpace:
                        foreach (var adminClear in Enum.GetValues(typeof(AdminClearances)).Cast<object>().Where(adminClear => Roles.IsUserInRole(userName, adminClear.ToString())))
                            Roles.RemoveUserFromRole(userName, adminClear.ToString());
                        break;
                    //case UserSpace.TeacherSpace:
                    //    foreach (var adminClear in Enum.GetValues(typeof(TeacherClearances)).Cast<object>().Where(adminClear => Roles.IsUserInRole(userName, adminClear.ToString())))
                    //        Roles.RemoveUserFromRole(userName, adminClear.ToString());
                    //    break;
                }
            }
            return true;
        }


        #endregion






        #region helpers  



        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetUserQuestion(string username)
        {
            try
            {
                var passwordQuestion = Membership.GetUser(username)?.PasswordQuestion;
                return passwordQuestion;
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                return null;
            }
        }


        /// <summary>
        /// Reset le mot de passe
        /// </summary>
        /// <param name="username"></param>
        /// <param name="questionAnswer"></param>
        /// <returns></returns>
        public string ResetPassword(string username, string questionAnswer)
        {
            string newPassword;
            var u = Membership.GetUser(username, false);

            if (u == null)
                return null;
            try
            {
                newPassword = u.ResetPassword(questionAnswer);
            }
            catch (Exception e)
            {
                DebugHelper.WriteException(e);
                return null;
            }
            return newPassword;
        }


        /// <summary>
        /// Changer le mot de pass d'un Utilisateur
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public bool ChangePassWord(string userName, string oldPass, string newPass)
        {
            var u = Membership.GetUser(userName);
            try
            {
                var statut = u != null && u.ChangePassword(oldPass, newPass);
                return statut;
            }
            catch (Exception e)
            {
                DebugHelper.WriteException(e);
            }
            return false;
        }


        /// <summary>
        /// Espaces Utilisateur
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, Enum>> UserSpaces(string userName)
            => new List<KeyValuePair<string, Enum>>(EnumsHelper.GetAllValuesAndDescriptions<UserSpace>()
                .Where(s => Roles.IsUserInRole(userName, s.Value.ToString())));


        /// <summary>
        /// Espaces Utilisateur
        /// </summary>
        /// <param name="profileGuid"></param>
        /// <returns></returns>
        public IEnumerable UserSpaces(Guid profileGuid)
        {
            try
            {
                return EnumsHelper.GetAllValuesAndDescriptions<UserSpace>().Select(role =>
                    new ViewCard()
                    {
                        Info1 = role.Key,
                        Info2 = role.Value.ToString(),
                        Info3 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString()) ? "Blue" : "Red",
                        Bool1 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString())
                    });                
            }
            catch (Exception e)
            {
                DebugHelper.WriteException(e);
                return null;
            }
        }


        /// <summary>
        /// Les roles dans ce espace
        /// </summary>
        /// <param name="profileGuid"></param>
        /// <param name="userSpace"></param>
        /// <returns></returns>
        public IEnumerable GetUserClearances(Guid profileGuid, UserSpace userSpace)
        {
            switch (userSpace)
            {
                case UserSpace.AdminSpace:
                    return EnumsHelper.GetAllValuesAndDescriptions<AdminClearances>().Select(role =>
                        new ViewCard
                        {
                            Info1 = role.Key,
                            Info2 = role.Value.ToString(),
                            Info3 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString()) ? "Blue" : "Red",
                            Bool1 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString())
                        });
                //case UserSpace.PompisteSpace:
                //    return EnumsHelper.GetAllValuesAndDescriptions<TeacherClearances>().Select(role =>
                //        new DataCard
                //        {
                //            Info1 = role.Key,
                //            Info2 = role.Value.ToString(),
                //            Info3 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString()) ? "Blue" : "Red",
                //            Bool1 = Roles.IsUserInRole(Membership.GetUser(profileGuid)?.UserName, role.Value.ToString())
                //        });
            }
            return null;
        }


        


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileGuid"></param>
        /// <returns></returns>

        public User GetUser(Guid profileGuid)
        {
            try
            {
                var membUser = Membership.GetUser(profileGuid);

                if (membUser == null)
                    return null;

                var theStaff = HrManager.StaticGetStaffByGuid(profileGuid);
                return new User
                {
                    // ReSharper disable once PossibleNullReferenceException
                    UserGuid = (Guid)membUser.ProviderUserKey,
                    UserName = membUser.UserName,
                    FullName = theStaff?.Person?.FullName,
                    PhotoIdentity = theStaff?.Person?.PhotoIdentity,
                    EmailAdress = membUser.Email,
                    IsApproved = membUser.IsApproved && !membUser.IsLockedOut,
                    IsLockedOut = membUser.IsLockedOut,
                    IsOnline = membUser.IsOnline,
                    //UserSpace     =(UserSpace)Enum.Parse(typeof(UserSpace), GetSetting(ProfileSettings.UserSpace, membUser.UserName)),
                    UserSpaces = UserSpaces(membUser.UserName)
                };
            }
            catch (Exception ex)
            {
                DebugHelper.WriteException(ex);
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public byte[] GetUserPic(string userName)
        {
            //using (var db = new StationContext())
            //{
            //    if (db.Database.Exists())
            //    {
            //        db.Database.Delete();
            //        db.Database.Create();
            //    }               
            //}
            
            var membUserGuid = (Guid)Membership.GetUser(userName)?.ProviderUserKey;
            return HrManager.StaticGetStaffByGuid(membUserGuid)?.Person?.PhotoIdentity;           
        }


        #endregion






        #region Protected Internal Static




        ///// <summary>
        ///// Parametres d'un utilisateur
        ///// </summary>
        ///// <returns></returns>
        //internal static bool StaticSetSetting(ProfileSettings profileSetting, string value, string userName = null)
        //{
        //    if (userName == null) userName = Membership.GetUser()?.UserName;

        //    var profile = ProfileBase.Create(userName);

        //    profile.SetPropertyValue(profileSetting.ToString(), value);

        //    profile.Save();

        //    return true;
        //}


        #endregion



    }
}
