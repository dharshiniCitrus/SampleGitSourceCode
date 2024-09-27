using EmployeeAccess.BL;
using EmployeeAccess.DA;
using EmployeeAccess.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EmployeeAccess.Controllers
{

    //[AllowAnonymous]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public string message = "";
        public string ConnectionString = "";
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDetails userDetails)
        {
            try
            {
                // Save the User Details 
                message = UserDetailsBL.CreateUser(userDetails).ToString();
                if (message != "User Details Saved Successfully")
                {

                    //var response1 = new
                    //{
                    //    Status=400,
                    //    Error = message
                    //};
                    //return Ok(response1);

                    var response1 = new
                    {
                        Status = 400,
                        Error = message
                    };
                    return BadRequest(response1);
                }
                else
                {
                    var response1 = new
                    {
                        Status = 201,
                        Success= message,
                        data = new { id = userDetails.Id, firstname = userDetails.firstName, lastname = userDetails.lastName, email = userDetails.email, isActive = userDetails.isActive, createdOn = userDetails.createdOn, updateOn = userDetails.updatedOn }
                       
                    };

                    return Ok(response1);
                    
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(message);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                List<UserDetails> userDetails = new List<UserDetails>();
                //Get all the user details
                userDetails = UserDetailsBL.ListAll();
                message = "User details of all user(s)";
                var response1 = new
                {
                    Status = 200,
                    Success = message,
                    data = userDetails
                };
                return Ok(response1);
               
            }
            catch
            {
                var response1 = new
                {
                    Status = 400,
                    Error = "User Not Found. Please Enter a Valid User Id"
                };
                return BadRequest(response1);
            }
        }


        [HttpGet("{Id}")]
        public IActionResult GetUserDetailsById(int Id)
        {
            try
            {
                UserDetails userDetails = new UserDetails();
                // Get user details by ID
                userDetails = UserDetailsBL.ListById(Id);
                message = "User details of id: "+Id+"";
                if (userDetails.email != null)
                {
                    var response1 = new
                    {
                        Status = 200,
                        Success = message,
                        data =userDetails
                    };
                    return Ok(response1);
                }
                else
                {
                    var response1 = new
                    {
                        Status = 400,
                        Error = "User Not Found. Please Enter a Valid User Id"
                    };
                    return BadRequest(response1);
                }
                    
            }
            catch (Exception ex)
            {
                var response1 = new
                {
                    Status = 400,
                    Error =ex.ToString()
                };
                return BadRequest(response1);
            }

        }

        [HttpPut("{Id}")]
        public IActionResult UpdateUserDetailsById(int Id, [FromBody] UserDetails userDetails)
        {
            try
            {

                message = UserDetailsBL.UpdateUserDetailsById(Id, userDetails);
                if (message != "User Details Updated Successfully")
                {

                    var response1 = new
                    {
                        Status = 400,
                        Error = message
                    };
                    return BadRequest(response1);
                }
                else
                {
                    var response1 = new
                    {
                        Status = 200,
                        Success = message,
                        data = new { id = userDetails.Id, firstname = userDetails.firstName, lastname = userDetails.lastName, email = userDetails.email,isActive=userDetails.isActive, createdOn = userDetails.createdOn, updateOn = userDetails.updatedOn }
                    };


                    return Ok(response1);
                }

            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return BadRequest(message);
            }


        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteUserDetailsById(int Id)
        {
            try
            {
                message = UserDetailsBL.DeleteUserDetailsById(Id);
                if (message != "User Details Deleted Successfully")
                {

                    var response1 = new
                    {
                        Status = 400,
                        Error = message
                    };
                    return BadRequest(response1);
                }
                else
                {
                    var response1 = new
                    {
                        Status = 200,
                        Success = message
                    };


                    return Ok(response1);
                }
            }
            catch(Exception ex) 
            {
                message = ex.ToString();
                return BadRequest(message);
            }
        }

        [Route("api/[controller]/enabledisable-status")]
        [HttpPost]
        public IActionResult EnableDisableActiveStatus(UserDetailsActive userDetailsActive)
        {
            message=UserDetailsBL.EnableDisableActiveStatus(userDetailsActive);
            var response1 = new
            {
                Status = 200,
                Success = message
            };
            return Ok(response1);
        }

    }
}
