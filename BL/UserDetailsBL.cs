using EmployeeAccess.DA;
using EmployeeAccess.Model;
using System.Text.RegularExpressions;

namespace EmployeeAccess.BL
{
    public class UserDetailsBL
    {
        public static string msg = "";
        public static string status = "";
        public static string CreateUser(UserDetails userDetails)
        {
            string Isvalid = "";
            if (userDetails.firstName == "")
            {
                msg = "Please Enter the First Name";
                Isvalid = "false";
                return msg;
            }
            if (userDetails.lastName == "")
            {
                msg = "Please Enter the Last Name";
                Isvalid = "false";
                return msg;
            }
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
            

            if(userDetails.firstName !="")
            {
                string firstname = userDetails.firstName.ToString();
                string pattern = @"^[a-zA-Z]{1}[a-zA-Z' -]{0,49}$";

                if (Regex.IsMatch(firstname, pattern)== false)
                {
                    if (firstname.Length > 50)
                        msg="First name must not exceed 50 characters.";
                    if (!Regex.IsMatch(firstname.Substring(0, 1), @"[a-zA-Z]"))
                        msg = "First name must start with an alphabetic character.";
                    if (!Regex.IsMatch(firstname, @"^[a-zA-Z' -]*$"))
                        msg = "First name can only contain alphabetic characters, spaces, hyphens, or apostrophes.";
                    Isvalid = "false";
                    return msg;
                }
                else
                {
                    Isvalid = "true";
                }
            }
            if (userDetails.lastName != "")
            {
                string lastName = userDetails.lastName.ToString();
                string pattern = @"^[a-zA-Z]{1}[a-zA-Z' -]{0,49}$";

                if (Regex.IsMatch(lastName, pattern) == false)
                {
                    if (lastName.Length > 50)
                        msg = "Last name must not exceed 50 characters.";
                    if (!Regex.IsMatch(lastName.Substring(0, 1), @"[a-zA-Z]"))
                        msg = "Last name must start with an alphabetic character.";
                    if (!Regex.IsMatch(lastName, @"^[a-zA-Z' -]*$"))
                        msg = "Last name can only contain alphabetic characters, spaces, hyphens, or apostrophes.";
                    Isvalid = "false";
                    return msg;
                }
                else
                {
                    Isvalid = "true";
                }
            }
            if (userDetails.password != "")
            {
                string password = userDetails.password.ToString();
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(password, pattern) == false)
                {
                    if (password.Length < 8 || password.Length > 20)
                        msg = "Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(password, @"[A-Z]"))
                        msg = "Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(password, @"[a-z]"))
                        msg = "Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(password, @"\d"))
                        msg = "Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(password, @"[@$!%*?&]"))
                        msg = "Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return msg;
                }
                else
                {
                    Isvalid = "true";
                }
            }
            if (Convert.ToString(userDetails.isActive) != "")
            {
                if ((ValidateIsActive(userDetails.isActive)) ? true : false)
                {
                    Isvalid = "true";
                }
            }
            else
            {
                msg = "Please mention isActive status";
                Isvalid = "false";
                return msg;
            }
            if (userDetails.email != "")
            {
                string email = userDetails.email.ToString();
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (isEmail == true)
                {
                    bool EmailExist = false;
                    EmailExist = UserDetailsDA.GetUserDetails(userDetails.email);

                    if (EmailExist == true)
                    {
                        msg = "Email already exists";
                        Isvalid = "false";
                        return msg;
                    }
                    else
                    {
                        Isvalid = "true";

                    }
                   
                }
                else
                {
                    msg = "Please Enter a valid Email ID";
                    Isvalid = "false";
                    return msg;

                }

            }
            if(Isvalid == "true")
            {
                return UserDetailsDA.CreateUserDetails(userDetails);
            }
            
            return msg;

        }

        public static List<UserDetails> ListAll()
        {
            return UserDetailsDA.ListAll();
        }

        public static UserDetails ListById(int id)
        {
            return UserDetailsDA.ListById(id);
        }

        public static string UpdateUserDetailsById(int id, UserDetails userDetails)
        {
            string Isvalid = "";
            if (Convert.ToString(id) == "")
            {
                msg = "Please Enter a Id to Update user details";
                Isvalid = "false";
                return msg;
            }
            if (userDetails.firstName == "")
            {
                msg = "Please Enter the First Name";
                Isvalid = "false";
                return msg;
            }
            if (userDetails.lastName == "")
            {
                msg = "Please Enter the Last Name";
                Isvalid = "false";
                return msg;
            }
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


            if (userDetails.firstName != "")
            {
                string firstname = userDetails.firstName.ToString();
                string pattern = @"^[a-zA-Z]{1}[a-zA-Z' -]{0,49}$";

                if (Regex.IsMatch(firstname, pattern) == false)
                {
                    if (firstname.Length > 50)
                        msg = "First name must not exceed 50 characters.";
                    if (!Regex.IsMatch(firstname.Substring(0, 1), @"[a-zA-Z]"))
                        msg = "First name must start with an alphabetic character.";
                    if (!Regex.IsMatch(firstname, @"^[a-zA-Z' -]*$"))
                        msg = "First name can only contain alphabetic characters, spaces, hyphens, or apostrophes.";
                    Isvalid = "false";
                    return msg;
                }
            }
            if (userDetails.lastName != "")
            {
                string lastName = userDetails.lastName.ToString();
                string pattern = @"^[a-zA-Z]{1}[a-zA-Z' -]{0,49}$";

                if (Regex.IsMatch(lastName, pattern) == false)
                {
                    if (lastName.Length > 50)
                        msg = "Last name must not exceed 50 characters.";
                    if (!Regex.IsMatch(lastName.Substring(0, 1), @"[a-zA-Z]"))
                        msg = "Last name must start with an alphabetic character.";
                    if (!Regex.IsMatch(lastName, @"^[a-zA-Z' -]*$"))
                        msg = "Last name can only contain alphabetic characters, spaces, hyphens, or apostrophes.";
                    Isvalid = "false";
                    return msg;
                }
            }
            if (userDetails.password != "")
            {
                string password = userDetails.password.ToString();
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

                if (Regex.IsMatch(password, pattern) == false)
                {
                    if (password.Length < 8 || password.Length > 20)
                        msg = "Password must be between 8 and 20 characters long.";
                    if (!Regex.IsMatch(password, @"[A-Z]"))
                        msg = "Password must contain at least one uppercase letter.";
                    if (!Regex.IsMatch(password, @"[a-z]"))
                        msg = "Password must contain at least one lowercase letter.";
                    if (!Regex.IsMatch(password, @"\d"))
                        msg = "Password must contain at least one numeric digit.";
                    if (!Regex.IsMatch(password, @"[@$!%*?&]"))
                        msg = "Password must contain at least one special character (@, $, !, %, *, ?, &).";
                    Isvalid = "false";
                    return msg;
                }
            }
            if(Convert.ToString(userDetails.isActive)!="")
            {
                if((ValidateIsActive(userDetails.isActive))? true : false)
                {
                    Isvalid = "true";
                }
            }
            else
            {
                msg = "Please mention isActive status";
                Isvalid = "false";
                return msg;
            }

            if (userDetails.email != "")
            {
                string email = userDetails.email.ToString();
                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                if (isEmail == true)
                {
                    bool EmailExist = false;
                    EmailExist = UserDetailsDA.GetUserDetails(userDetails.email);

                    if (EmailExist == true)
                    {
                        msg = "Email already exists";
                        Isvalid = "false";
                        return msg;
                    }
                    else
                    {
                        Isvalid = "true";

                    }
                }
                else
                {
                    msg = "Please Enter a valid Email ID";
                    Isvalid = "false";
                    return msg;

                }

                if (Isvalid == "true")
                {
                    return UserDetailsDA.UpdateUserDetailsById(id, userDetails);
                }

            }

            return msg;
        }

        public static bool ValidateIsActive(object isActive)
        {
            return isActive is bool;
        }

        public static string DeleteUserDetailsById(int id)
        {
            if (Convert.ToString(id) == "")
            {
                msg = "Please Enter a Id to Delete the user details";

                return msg;
            }
            else
            {
                return UserDetailsDA.DeleteUserDetailsById(id);
            }
            return msg;
        }

        public static string EnableDisableActiveStatus(UserDetailsActive userDetailsActive)
        {
            if ((userDetailsActive.userIds[0] == 0) && (userDetailsActive.userIds.Count>0))
            {
                msg = "No user found with Id(s).Please provide valid userId to update the Active Status";
                return msg;
            }
            if ((userDetailsActive.isActive != true) && (userDetailsActive.isActive != false))
            {
                msg = "Please provide Active Status of Users";
                return msg;
            }
            else
            {
                return UserDetailsDA.EnableDisableActiveStatus(userDetailsActive);
            }
        }
    }
}
