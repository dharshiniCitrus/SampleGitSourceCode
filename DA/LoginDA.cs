using EmployeeAccess.BL;
using EmployeeAccess.Model;
using System.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using System.Dynamic;
using System.Net.Mail;
using System.Net;


namespace EmployeeAccess.DA
{
    public class LoginDA
    {
        public static string message = "";
        public static string ConnectionString = "Data Source = PRIYADHARSHINI\\SQLEXPRESS; Initial Catalog = Employee; User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;";
        IConfiguration configuration;
        //Create User Login Details
        public static string LoginDetails(UserDetails login)
        {
            try
            {
               
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetLoginUserDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", login.email);
                    cmd.Parameters.AddWithValue("@password", login.password);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                        login.Id = Convert.ToInt32(rdr[0]);
                        login.firstName = rdr[1].ToString();
                        login.lastName = rdr[2].ToString();
                        login.email = rdr[3].ToString();
                        login.password = rdr[4].ToString();
                        login.isActive = Convert.ToString(rdr[5]) == "1" ? true : false;
                        if (Convert.ToString(rdr[6].ToString()) != "")
                        {
                            login.createdOn = Convert.ToString(Convert.ToDateTime(rdr[6]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            login.createdOn = "";
                        }
                        if (Convert.ToString(rdr[7].ToString()) != "")
                        {
                            login.updatedOn = Convert.ToString(Convert.ToDateTime(rdr[7]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            login.updatedOn = "";
                        }


                        string token = GenerateJSONWebToken(Convert.ToString(login.Id), login.password.ToString());

                        login.JWTToken = token;
                        message = "User Exists";


                    }
                    else
                    {
                        message = "Invalid username or password";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;

            }

            return message;
        }
        public static string GenerateJSONWebToken(string userId, string email)
        {
            // Create claims
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Email, email),
            // Add more claims as necessary
            };

            var builder = WebApplication.CreateBuilder();


            ConfigurationManager configuration = builder.Configuration;

            // Create a key from the secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // Create credentials using the key and the signing algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Set token expiration time
                signingCredentials: creds
            );

            // Return the token as a string
            return Convert.ToString(new JwtSecurityTokenHandler().WriteToken(token));
        }




        //Check if Credentials are valid
        public static bool AreCredentialsValid(string username, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetLoginDetailsExists", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", SqlDbType.NVarChar).Value = username.ToString();
                    cmd.Parameters.AddWithValue("@password", SqlDbType.NVarChar).Value = password.ToString();
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && (rdr[0].ToString()==username.ToString()) && (rdr[1].ToString()==password.ToString()) )
                    {
                        rdr.Close();
                        con.Close();
                        return true;

                    }
                    else
                    {
                        rdr.Close();
                        con.Close();
                        return false;

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static bool CheckEmailExists(string? email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetUserDetailsByEmail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email.ToString();
                    
                    con.Open();
                   
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && rdr[0].ToString() != null && rdr[1].ToString()=="1")
                    {
                        rdr.Close();
                        con.Close();
                        return true;

                    }
                    else
                    {
                        rdr.Close();
                        con.Close();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return false;
         }

        public static string GeneratePasswordResetLink(string? email)
        {
            string token = Guid.NewGuid().ToString();

            var builder = WebApplication.CreateBuilder();


            ConfigurationManager configuration = builder.Configuration;
            builder.Configuration.AddJsonFile($"appsettings.Development.json");

            string BE_Url = builder.Configuration.GetConnectionString("BE_URL");
            string resetLink = $""+BE_Url+"/resetpassword/"+token+"";

            bool IsResetToken= StoreResetToken(email, token);
         

            //Send Email Functionality
            bool IsEmailSent= SendEmail(email, resetLink);
            //bool IsEmailSent = true;
            if ((IsResetToken==true)  && (IsEmailSent == true ))
            {
                message = "Email Sent Successfully";
            }
            else
            {
                message = "Email Not Sent Successfully";
            }
            return message;
        }

       
        public static bool StoreResetToken(string? email, string token)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateRestTokenbyEmail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email.ToString();
                    cmd.Parameters.AddWithValue("@resetToken", SqlDbType.NVarChar).Value = token.ToString();
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        rdr.Close();
                        con.Close();
                        return true;
                    }
                    else
                    {
                        rdr.Close();
                        con.Close();
                        return false;
                    }
                   
                }
               
            } 
            catch (Exception ex) 
            { 

            }
            return false;
        }

        public static bool SendEmail(string? email, string resetLink)
        {
            bool sendMail = false;
            try
            {
                
                var message = new MailMessage("citrusbvp@gmail.com", email)
                {
                    Subject = "Password Reset Request",
                    Body = $"Please click the following link to reset your password:<p><a href='{resetLink}'>{resetLink}</a></p>",
                    IsBodyHtml = true
                };

                // Set up the SMTP client
                using (var smtpClient = new SmtpClient("smtp-relay.sendinblue.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("citrusbvp@gmail.com", "bK06zZIHN8kOsaYU");
                    smtpClient.EnableSsl = true; // Ensure SSL is enabled

                    // Send the email
                    smtpClient.Send(message);

                    sendMail = true;
                }
            }
            catch (Exception ex)
            {
                sendMail = false;
                throw ex;
            }
           return sendMail;
        }

        //Check If ResetTokenIsValid
        public static bool CheckResetTokenIsValid(string resettoken)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetUserDetailsByresettoken", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@resettoken", SqlDbType.NVarChar).Value = resettoken.ToString();

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && rdr[0].ToString() != null && rdr[1].ToString() != null && rdr[2].ToString() =="1")
                    {
                        UserDetails userDetails = new UserDetails();
                        userDetails.email = rdr[0].ToString();
                        rdr.Close();
                        con.Close();
                        return true;

                    }
                    else
                    {
                        rdr.Close();
                        con.Close();
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        public static string ResetPassword(string newPassword, string confirmPassword, string resettoken)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdatePasswordByresettoken", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@newpassword", SqlDbType.NVarChar).Value = confirmPassword.ToString();
                    cmd.Parameters.AddWithValue("@resettoken", SqlDbType.NVarChar).Value = resettoken.ToString();
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && rdr[0].ToString() != null)
                    {
                        message = "Password Reset Successfully";
                        rdr.Close();
                        con.Close();
                        

                    }
                    else
                    {
                        message = "Password Reset Link has been expired.Please try again.";
                        rdr.Close();
                        con.Close();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

       
    }

}
