
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Xml.Linq;
using EmployeeAccess.BL;
using EmployeeAccess.Model;


namespace EmployeeAccess.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {


        [HttpPost("Save")]
        public IActionResult Save([FromBody] Employee employee)
        {
            try
            {
                EmployeeBL.Save(employee);
                return Ok("Employee Saved Successfully");
            }
            catch
            {
                return Ok("Something Went Wrong!!");
            }

        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                List<Employee> employees = new List<Employee>();
                employees = EmployeeBL.ListAll();
                return Ok(employees);
            }
            catch
            {
                return Ok("Something Went Wrong!!");
            }

        }
        [HttpGet("{Id}")]
        public IActionResult GetEmployeeById(int Id)
        {
            try
            {
                Employee employee = new Employee();
                employee = EmployeeBL.ListById(Id);
                return Ok(employee);
            }
            catch
            {
                return Ok("Something Went Wrong!!");
            }

        }

    }
    }
