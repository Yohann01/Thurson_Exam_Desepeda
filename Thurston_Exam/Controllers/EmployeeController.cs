using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Thurston_Exam.Models;

namespace Thurston_Exam.Controllers
{
    public class EmployeeController : Controller
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["DBModel"].ConnectionString;
     
        [HttpGet]
        public ActionResult Index(string search)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Employee";
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;

                    if (!string.IsNullOrEmpty(search))
                    {
                        int employeeId;
                        if (int.TryParse(search, out employeeId))
                        {
                            query += " WHERE EmployeeID = @EmployeeID";
                            command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        }
                        else
                        {
                            query += " WHERE FirstName LIKE @SearchString OR LastName LIKE @SearchString OR Email LIKE @SearchString OR Position LIKE @SearchString";
                            command.Parameters.AddWithValue("@SearchString", "%" + search + "%");
                        }
                    }

                    command.CommandText = query;
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            Position = reader.GetString(4),
                        };
                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
                ViewBag.Error = "Error fetching employees: " + ex.Message;
            }

            ViewBag.CurrentFilter = search; // Maintain the search term in the view
            return View(employees);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        //GET: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Employee (FirstName, LastName, Email, Position) VALUES (@FirstName, @LastName, @Email, @Position)";
                    SqlCommand command = new SqlCommand(query, conn);

                    //Parameters
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Position", employee.Position);

                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false, message = "No rows affected." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Employee/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeID = (int)reader["EmployeeID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Position = reader["Position"].ToString()
                        };
                    }
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }


        // GET: Employee/Edit/1
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeID = (int)reader["EmployeeID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Position = reader["Position"].ToString()
                        };
                    }
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        // POST: Employee/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Position = @Position WHERE EmployeeID = @EmployeeID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@Position", employee.Position);
                    cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Model state is invalid" });
        }

        // GET: Employee/Delete/1
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeID = (int)reader["EmployeeID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Position = reader["Position"].ToString()
                        };
                    }
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }


        // POST: Employee/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EmployeeID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }
    }
}