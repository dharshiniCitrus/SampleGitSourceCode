using EmployeeAccess.BL;
using EmployeeAccess.DA;
using EmployeeAccess.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmployeeAccess.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public string msg = "";
        [Route("api/[controller]/Login")]
        [HttpPost]
        public IActionResult Login([FromBody] UserDetails userDetails)
        {
            //Create User Login Details
           
            msg = LoginBL.LoginDetails(userDetails).ToString();

            if (msg != "User Exists")
            {
                var response1 = new
                {

                    result = 1,
                    data = (object)null,
                    token = (string)null,
                    message =msg
                };
                return BadRequest(response1);
            }

            //Check User Login Details
            bool credentialsValid = LoginBL.AreCredentialsValid(userDetails.email, userDetails.password);
            if (credentialsValid==false)
            {
                // Return 401 Unauthorized with the specified error message
                return Unauthorized(new
                {
                    result = 0,
                    data = (object)null,
                    token = (string)null,
                    message = "Invalid username or password"
                });
            }
            else
            {
                var response1 = new
                {
                    result = 1,
                    data = new { id = userDetails.Id,firstname=userDetails.firstName,lastname=userDetails.lastName,email=userDetails.email, createdOn=userDetails.createdOn,updateOn=userDetails.updatedOn},
                    JWTToken = userDetails.JWTToken.ToString(),
                    message = "Success"
                };

                return Ok(response1);

            }

        }

        [Route("api/[controller]/forgot-password")]
        [HttpPost]
        public IActionResult ForgotPassword([FromBody] UserDetails userDetails)
        {
            bool userActive= LoginBL.CheckEmailExists(userDetails.email);
            if(userActive==true)
            {
                msg=LoginBL.GeneratePasswordResetLink(userDetails.email);
                if(msg=="Email Sent Successfully")
                {
                    var response1 = new
                    {

                        result = 1,
                        message = "Password reset link sent to your email."
                    };
                    return Ok(response1);
                }
                else
                {
                    var response1 = new
                    {

                        result = 0,
                        message = "Email Id not found to send a email.Please verify the email address and try again."
                    };
                    return BadRequest(response1);
                }
            }
            else
            {
                var response1 = new
                {

                    result = 0,
                    message = "User not found.Please verify the email address and try again."
                };
                return BadRequest(response1);
            }
            return Ok(msg);
        }

        [Route("api/[controller]/resetpassword")]
        [HttpPost]
        public IActionResult ResetPassword([FromBody]ResetPassword ResetPassword)
        {
            bool resetTokenIsValid=LoginBL.CheckResetTokenIsValid(ResetPassword.newPassword.ToString(), ResetPassword.confirmPassword.ToString(), ResetPassword.resetToken.ToString());
            if (resetTokenIsValid == true)
            {
                msg = LoginBL.ResetPassword(ResetPassword.newPassword.ToString(), ResetPassword.confirmPassword.ToString(), ResetPassword.resetToken.ToString());
                if(msg== "Password Reset Successfully")
                {
                    var response1 = new
                    {

                        result = 1,
                        message = msg
                    };
                    return Ok(response1);
                }
                else
                {
                    var response1 = new
                    {

                        result = 0,
                        message = msg
                    };
                    return BadRequest(response1);
                }
                
            }
            else
            {
                var response1 = new
                {

                    result = 0,
                    message = "Password Reset Link has been expired.Please try again."
                };
                return BadRequest(response1);
            }

            
        }
    }
}
