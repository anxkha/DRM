using DOTP.Database;
using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace DOTP.Users
{
    public static class Manager
    {
        private static string GET_USER = @"
SELECT ID, FirstName, Email, Password, IsRaidTeam, IsAdmin
FROM Users
WHERE (Email = @Email)
";

        private static string ADD_USER = @"
INSERT INTO Users
    (FirstName, Email, Password, IsRaidTeam, IsAdmin)
VALUES
    (@FirstName, @Email, @Password, @IsRaidTeam, @IsAdmin)
";

        private static string CHANGE_PASSWORD = @"
UPDATE Users
SET Password = @Password
WHERE (ID = @ID)
";

        public static bool ValidateUser(string Email, string Password)
        {
            bool authenticated = false;

            Connection.ExecuteSql(new Query(GET_USER).AddParam("@Email", Email.ToLower()), delegate(SqlDataReader reader)
            {
                if(!reader.Read())
                    return;

                var id = (int)reader[0];
                var firstName = reader[1].ToString();
                var email = reader[2].ToString();
                var password = reader[3].ToString();
                var isRaidTeam = (bool)reader[4];
                var isAdmin = (bool)reader[5];

                if (0 == string.Compare(password, Security.Sha256(Password)))
                {
                    User.Store.Add(id, new User(id, firstName, email, isRaidTeam, isAdmin));

                    authenticated = true;
                }
            });

            return authenticated;
        }

        public static bool CreateUser(string firstName, string email, string password, bool isRaidTeam, bool isAdmin, out string error)
        {
            var success = false;

            error = "";

            Connection.ExecuteSql(new Query(GET_USER)
                                    .AddParam("@Email", email.ToLower()), delegate(SqlDataReader reader)
                                    {
                                        if(reader.Read())
                                            success = true;
                                    });

            if (success)
            {
                error = "An account with that email address already exists.";
                return false;
            }

            Connection.ExecuteSql(new Query(ADD_USER)
                                    .AddParam("@FirstName", firstName)
                                    .AddParam("@Email", email.ToLower())
                                    .AddParam("@Password", Security.Sha256(password))
                                    .AddParam("@IsRaidTeam", isRaidTeam)
                                    .AddParam("@IsAdmin", isAdmin), delegate(SqlDataReader reader)
                                    {
                                        if (reader.RecordsAffected > 0)
                                            success = true;
                                    });

            if (success)
                ValidateUser(email, password);

            if (!success)
                error = "Unable to create your account. Please contact the system administrator.";

            return success;
        }

        public static bool ChangePassword(int userID, string oldPassword, string newPassword, out string error)
        {
            error = "";

            if (!ValidateUser(User.Store.GetByID(userID).Email, oldPassword))
            {
                error = "Please enter the correct current password.";
                return false;
            }

            if (oldPassword == newPassword)
            {
                error = "Your new password cannot be the same as your old password.";
                return false;
            }

            var success = false;

            Connection.ExecuteSql(new Query(CHANGE_PASSWORD)
                                    .AddParam("@Password", Security.Sha256(newPassword))
                                    .AddParam("@ID", userID), delegate(SqlDataReader reader)
                                    {
                                        if (reader.RecordsAffected > 0)
                                            success = true;
                                    });

            if (!success)
                error = "Unable to change your password. Please contact the system administrator.";
            return success;
        }

        public static User GetCurrentUser()
        {
            return User.Store.GetByEmail(HttpContext.Current.User.Identity.Name);
        }

        public static bool IsReallyAuthenticated(HttpRequest request)
        {
            return IsReallyAuthenticatedInternal(request.IsAuthenticated);
        }

        public static bool IsReallyAuthenticated(HttpRequestBase request)
        {
            return IsReallyAuthenticatedInternal(request.IsAuthenticated);
        }

        private static bool IsReallyAuthenticatedInternal(bool formsState)
        {
            var currentUser = GetCurrentUser();

            if (formsState)
            {
                if (null == currentUser)
                {
                    FormsAuthentication.SignOut();
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
