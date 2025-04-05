using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MyWebApp.Pages.Employees
{
    public class Detail : PageModel
    {
        public List<string> employee { get; set;} = [];
        public string ErrorMessage { get; set;} = "";

        public void OnGet(String id)
        {
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString)){
                    connection.Open();
                    string sql = " select e.id,e.ten,t.email,t.sodienthoai,c.tenchucvu,y.tencongty from employees e JOIN thongtincanhannhanvien t on e.id = t.id JOIN chucvu c on e.machucvu = c.machucvu JOIN congty y on e.macongty = y.macongty where e.id = @id";

                    using(SqlCommand command = new SqlCommand(sql,connection)){
                        command.Parameters.AddWithValue("@id", id);
                        using(SqlDataReader reader = command.ExecuteReader()){
                            reader.Read();
                            employee.Add(reader.GetString(0) ?? "");
                            employee.Add(reader.GetString(1) ?? "");
                            employee.Add(reader.GetString(2) ?? "");
                            employee.Add(reader.GetString(3) ?? "");
                            employee.Add(reader.GetString(4) ?? "");
                            employee.Add(reader.GetString(5) ?? "");

                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = $"An unexpected error occurred: {e.Message}";
                throw;            
            }
        }
    }
}