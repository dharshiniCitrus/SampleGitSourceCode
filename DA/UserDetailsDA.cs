using EmployeeAccess.BL;
using System.Data.SqlClient;
using System.Data;
using EmployeeAccess.Model;
using System.Runtime.Intrinsics.Arm;

namespace EmployeeAccess.DA
{
    public class UserDetailsDA
    {
        public static string ConnectionString = "Data Source = PRIYADHARSHINI\\SQLEXPRESS; Initial Catalog = Employee; User ID = PRIYADHARSHINI\\Priyadharshini J;Trusted_Connection=True;";
        public static string msg = "";

        //Save User Details in the Table
        public static string CreateUserDetails(UserDetails userDetails)
        {
            try
            {
               

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SaveUserDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstName", userDetails.firstName);
                    cmd.Parameters.AddWithValue("@lastName", userDetails.lastName);
                    cmd.Parameters.AddWithValue("@email", userDetails.email);
                    cmd.Parameters.AddWithValue("@password", userDetails.password);
                    cmd.Parameters.AddWithValue("@isactive", userDetails.isActive);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        userDetails.Id= Convert.ToInt32(rdr[0]);
                        userDetails.firstName = rdr[1].ToString();
                        userDetails.lastName = rdr[2].ToString();
                        userDetails.email = rdr[3].ToString();
                        userDetails.password = rdr[4].ToString();
                        userDetails.isActive = Convert.ToString(rdr[5]) == "1" ? true : false;
                        if (Convert.ToString(rdr[6].ToString()) != "")
                        {
                            userDetails.createdOn = Convert.ToString(Convert.ToDateTime(rdr[6]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            userDetails.createdOn = "";
                        }
                        if (Convert.ToString(rdr[7].ToString()) != "")
                        {
                            userDetails.updatedOn = Convert.ToString(Convert.ToDateTime(rdr[7]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            userDetails.updatedOn = "";
                        }
                    }

                    msg = "User Details Saved Successfully";
                    rdr.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }

        //Check if Email Already exists
        public static bool GetUserDetails(string? email)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetUserDetailsByEmail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", SqlDbType.NVarChar).Value = email.ToString();

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read() && rdr[0].ToString()!= null)
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
            return false;
        }

        //Get User Details by ID
        public static UserDetails ListById(int id)
        {
            var user = new UserDetails();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetUserDetailsByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = id;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        user.Id = Convert.ToInt32(rdr[0]);
                        user.firstName = rdr[1].ToString();
                        user.lastName = rdr[2].ToString();
                        user.email = rdr[3].ToString();
                        user.password = rdr[4].ToString();
                        user.isActive = Convert.ToString(rdr[5]) == "1" ? true : false;
                        if (Convert.ToString(rdr[6].ToString()) != "")
                        {
                            user.createdOn = Convert.ToString(Convert.ToDateTime(rdr[6]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            user.createdOn = "";
                        }
                        if (Convert.ToString(rdr[7].ToString()) != "")
                        {
                            user.updatedOn = Convert.ToString(Convert.ToDateTime(rdr[7]).ToString("dd/MM/yy hh:mm:ss"));
                        }
                        else
                        {
                            user.updatedOn = "";
                        }
                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Something wrong in connection");
            }
            return user;
        }

        // Get All the User Details
        public static List<UserDetails> ListAll()
        {
            var user = new List<UserDetails>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT id,firstName,LastName,email,password,isActive,createdOn,updatedOn from UserDetails", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        user.Add(new UserDetails
                        {
                            Id=Convert.ToInt32(rdr[0]),
                            firstName = rdr[1].ToString(),
                            lastName = rdr[2].ToString(),
                            email = rdr[3].ToString(),
                            password = rdr[4].ToString(),
                            isActive = Convert.ToString(rdr[5]) == "1" ? true : false,
                            createdOn = (Convert.ToString(rdr[6].ToString())!="")?(Convert.ToString(Convert.ToDateTime(rdr[6]).ToString("dd/MM/yy hh:mm:ss"))):"",
                            updatedOn = (Convert.ToString(rdr[7].ToString()) != "") ? (Convert.ToString(Convert.ToDateTime(rdr[7]).ToString("dd/MM/yy hh:mm:ss"))) : "",
                        });
                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Something wrong in connection");
            }
            return user;

        }

        //Update user details by Id
        public static string UpdateUserDetailsById(int id, UserDetails userDetails)
        {
           
            try
            {
                var user = new UserDetails();
                user = UserDetailsDA.ListById(id);

                if (user.email != null)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("UpdateUserDetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@firstName", userDetails.firstName);
                        cmd.Parameters.AddWithValue("@lastName", userDetails.lastName);
                        cmd.Parameters.AddWithValue("@email", userDetails.email);
                        cmd.Parameters.AddWithValue("@password", userDetails.password);
                        cmd.Parameters.AddWithValue("@isactive", userDetails.isActive);
                        cmd.Parameters.AddWithValue("@isUpdate", 1);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {

                            userDetails.Id = Convert.ToInt32(rdr[0]);
                            userDetails.firstName = rdr[1].ToString();
                            userDetails.lastName = rdr[2].ToString();
                            userDetails.email = rdr[3].ToString();
                            userDetails.password = rdr[4].ToString();
                            userDetails.isActive = Convert.ToString(rdr[5]) == "1" ? true : false;
                            if (Convert.ToString(rdr[6].ToString()) != "")
                            {
                                userDetails.createdOn = Convert.ToString(Convert.ToDateTime(rdr[6]).ToString("dd/MM/yy hh:mm:ss"));
                            }
                            else
                            {
                                userDetails.createdOn = "";
                            }
                            if (Convert.ToString(rdr[7].ToString()) != "")
                            {
                                userDetails.updatedOn = Convert.ToString(Convert.ToDateTime(rdr[7]).ToString("dd/MM/yy hh:mm:ss"));
                            }
                            else
                            {
                                userDetails.updatedOn = "";
                            }
                        }

                        msg = "User Details Updated Successfully";
                        rdr.Close();
                        con.Close();
                    }
                }
                else
                {
                    msg = "User Not Found. Please try with a valid User Id";
                    return msg;
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }

        // Delete User Details by ID
        public static string DeleteUserDetailsById(int id)
        {
            try
            {

                var user = new UserDetails();
                user = UserDetailsDA.ListById(id);

                if (user.email != null)
                {
                   
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("Delete From UserDetails Where Id= "+id+";", con);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        con.Close();
                    }

                    msg = "User Details Deleted Successfully";
                   
                } 
                else
                {
                 msg = "User Not Found. Please try with a valid User Id";
                 return msg;
                 }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return msg;
        }

        public static string EnableDisableActiveStatus(UserDetailsActive userDetailsActive)
        {
           try
            {
                bool IsUpdate=false;
                string UserActiveStatus = "false";
                if (userDetailsActive.isActive == true)
                {
                    UserActiveStatus = "active";
                }
                else
                {
                    UserActiveStatus = "in-active";
                }
                if (userDetailsActive.userIds.Count() == 1 )
                {
                    IsUpdate=UpdateUserActiveStauts(Convert.ToInt32(userDetailsActive.userIds[0]), userDetailsActive.isActive);
                    

                }
                else if (userDetailsActive.userIds.Count() > 1)
                {
                    for (int i = 0; i < userDetailsActive.userIds.Count(); i++)
                    {
                        IsUpdate= UpdateUserActiveStauts(Convert.ToInt32(userDetailsActive.userIds[i]), userDetailsActive.isActive);
                    }
                }

                if (IsUpdate == true)
                {
                    msg = "Updated " + userDetailsActive.userIds.Count() + " user(s) status as " + UserActiveStatus + "";
                }
                else
                {
                    msg = "User Status Cannot be Updated";
                }


            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            return msg;
        }

        public static bool UpdateUserActiveStauts(int Id, bool? isActive)
        {
            bool isUpdate = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("UpdateUserDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@firstName", "");
                    cmd.Parameters.AddWithValue("@lastName", "");
                    cmd.Parameters.AddWithValue("@email", "");
                    cmd.Parameters.AddWithValue("@password","");
                    cmd.Parameters.AddWithValue("@isactive", isActive);
                    cmd.Parameters.AddWithValue("@isUpdate", 2);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        isUpdate = true;
                    }
                    else
                    {
                        isUpdate = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                isUpdate = false;
                msg = ex.ToString();

            }
            return isUpdate;
        }
    }
}
