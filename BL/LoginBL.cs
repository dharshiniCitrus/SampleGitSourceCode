using EmployeeAccess.DA;
using EmployeeAccess.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace EmployeeAccess.BL
{
    public class LoginBL
    {
        public static string msg = "";

        //Create User Login Details
        public static string LoginDetails(UserDetails userDetails)
        {
            string Isvalid = "";
            if (userDetails.email == "")
            {
                msg = "Please Enter the Email";
                Isvalid = "false";
                return msg;
            }
            if (userDetails.password == "")
            {
                msg = "Please Enter the Password";
                Isvalid = "false";
                return msg;
            }
            else
            {
                Isvalid = "true";

            }
            if (Isvalid == "true")
            {
                return LoginDA.LoginDetails(userDetails);
            }


            return msg;
        }

        //Check if Credentials are valid
        public static bool AreCredentialsValid(string username,string password)
        {
          return LoginDA.AreCredentialsValid(username,password);
        }

        public static bool CheckEmailExists(string? email)
        {
            return LoginDA.CheckEmailExists(email); 
        }

        public static string GeneratePasswordResetLink(string? email)
        {
            return LoginDA.GeneratePasswordResetLink(email);
        }

        public static string ResetPassword(string newPassword, string confirmPassword, string resettoken)
        {
            string Isvalid = "";
            if (newPassword == "")
            {
                msg = "Please Enter the New Password";
                Isvalid = "false";
                return msg;
            }
            if (confirmPassword == "")
            {
                msg = "Please Enter the Confirm Password";
                Isvalid = "false";
                return msg;
            }
            else
            {
                Isvalid = "true";

            }
            if (newPassword != "")
            { 
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(newPassword, pattern) == false)
                {
                    if (newPassword.Length < 8 || newPassword.Length > 20)
                        msg = "New Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(newPassword, @"[A-Z]"))
                        msg = "New Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(newPassword, @"[a-z]"))
                        msg = "New Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(newPassword, @"\d"))
                        msg = "New Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(newPassword, @"[@$!%*?&]"))
                        msg = "New Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return msg;
                }
                else
                {
                    Isvalid = "true";
                }
            }

            if (confirmPassword != "")
            {
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(confirmPassword, pattern) == false)
                {
                    if (confirmPassword.Length < 8 || confirmPassword.Length > 20)
                        msg = "Confirm Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(confirmPassword, @"[A-Z]"))
                        msg = "Confirm Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(confirmPassword, @"[a-z]"))
                        msg = "Confirm Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(confirmPassword, @"\d"))
                        msg = "Confirm Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(confirmPassword, @"[@$!%*?&]"))
                        msg = "Confirm Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return msg;
                }
                else
                {
                    Isvalid = "true";
                }
            }
            if(newPassword.ToString()!=confirmPassword.ToString()) 
            {
                Isvalid = "false";
                msg = "New Password and Confirm Password doesn't match";
                return msg;  
            }
            else
            {
                Isvalid = "true";
            }

            if (Isvalid == "true")
            {
                return LoginDA.ResetPassword(newPassword, confirmPassword, resettoken);
            }
            return msg;
        }

        public static bool CheckResetTokenIsValid(string newPassword,string confirmPassword,string resettoken)
        {
            string Isvalid = "";
            if (newPassword == "")
            {
                msg = "Please Enter the New Password";
                Isvalid = "false";
                return false;
            }
            if (confirmPassword == "")
            {
                msg = "Please Enter the Confirm Password";
                Isvalid = "false";
                return false;
            }
            else
            {
                Isvalid = "true";

            }
            if (newPassword != "")
            {
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(newPassword, pattern) == false)
                {
                    if (newPassword.Length < 8 || newPassword.Length > 20)
                        msg = "New Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(newPassword, @"[A-Z]"))
                        msg = "New Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(newPassword, @"[a-z]"))
                        msg = "New Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(newPassword, @"\d"))
                        msg = "New Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(newPassword, @"[@$!%*?&]"))
                        msg = "New Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return false;
                }
                else
                {
                    Isvalid = "true";
                }
            }

            if (confirmPassword != "")
            {
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(confirmPassword, pattern) == false)
                {
                    if (confirmPassword.Length < 8 || confirmPassword.Length > 20)
                        msg = "Confirm Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(confirmPassword, @"[A-Z]"))
                        msg = "Confirm Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(confirmPassword, @"[a-z]"))
                        msg = "Confirm Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(confirmPassword, @"\d"))
                        msg = "Confirm Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(confirmPassword, @"[@$!%*?&]"))
                        msg = "Confirm Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return false;
                }
                else
                {
                    Isvalid = "true";
                }
            }
            if (newPassword.ToString() != confirmPassword.ToString())
            {
                Isvalid = "false";
                msg = "New Password and Confirm Password doesn't match";
                return false;
            }
            else
            {
                Isvalid = "true";
            }

            if (Isvalid == "true")
            {
                return LoginDA.CheckResetTokenIsValid(resettoken);
            }
            return false;
        }
    }
}
