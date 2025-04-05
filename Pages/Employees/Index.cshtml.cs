using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MyWebApp.Pages.Employees
{
    public class Index : PageModel
    {
        public List<EmployeesInfo> EmployeeList { get; set;} = [];


        // Post properties
        [BindProperty, Required(ErrorMessage = "id is required")]
        public required string id { get; set; }
        [BindProperty, Required(ErrorMessage = "ten is required")]
        public required string ten { get; set; }
        [BindProperty]
        public string? machucvu { get; set; }
        [BindProperty]
        public string? macongty { get; set; }


        // Search properties
        [BindProperty (SupportsGet = true)]
        public string? searchId { get; set; }
        [BindProperty (SupportsGet = true)]
        public string? searchTen { get; set; }
        [BindProperty (SupportsGet = true)]
        public string? searchMachucvu { get; set; }
        [BindProperty (SupportsGet = true)]
        public string? searchMacongty { get; set; }



        public string ErrorMessage { get; set;} = "";

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString)){
                    connection.Open();

                    // Build dynamic SQL query with search filters
                    string sql = "SELECT * FROM employees WHERE 1=1";
                    var parameters = new List<SqlParameter>();

                    if (!string.IsNullOrEmpty(searchId))
                    {
                        sql += " AND id LIKE @searchId";
                        parameters.Add(new SqlParameter("@searchId", $"%{searchId}%"));
                    }
                    if (!string.IsNullOrEmpty(searchTen))
                    {
                        sql += " AND ten LIKE @searchTen";
                        parameters.Add(new SqlParameter("@searchTen", $"%{searchTen}%"));
                    }
                    if (!string.IsNullOrEmpty(searchMachucvu))
                    {
                        sql += " AND machucvu LIKE @searchMachucvu";
                        parameters.Add(new SqlParameter("@searchMachucvu", $"%{searchMachucvu}%"));
                    }
                    if (!string.IsNullOrEmpty(searchMacongty))
                    {
                        sql += " AND macongty LIKE @searchMacongty";
                        parameters.Add(new SqlParameter("@searchMacongty", $"%{searchMacongty}%"));
                    }

                    sql += " ORDER BY id DESC";

                    using(SqlCommand cmd = new SqlCommand(sql, connection)){
                    foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                        using(SqlDataReader reader = cmd.ExecuteReader()){
                            while(reader.Read()){
                                EmployeesInfo employeeInfo = new EmployeesInfo();
                                employeeInfo.id = reader.GetString(0);
                                employeeInfo.ten = reader.GetString(1);
                                employeeInfo.machucvu = reader.GetString(2);
                                employeeInfo.macongty = reader.GetString(3);
                                EmployeeList.Add(employeeInfo);
                            }
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


        public IActionResult OnPostDeleteMulti(List<string> selectedIds){

            if (selectedIds == null || !selectedIds.Any())
            {
                TempData["ErrorMessage"] = "No employees selected for deletion.";
                return RedirectToPage();
            }
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString)){
                    connection.Open();
                    int deletedNumber = 0;
                    foreach(var id in selectedIds){
                        var sql = "delete from thongtincanhannhanvien where id = @id ; delete from employees where id = @id";
                        using(SqlCommand cmd = new SqlCommand(sql,connection)){
                        cmd.Parameters.AddWithValue("@id",id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0){
                            deletedNumber++;
                            } 
                        }
                    }
                    
                    TempData["SuccessMessage"] = $"{deletedNumber} employee(s) successfully deleted.";
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {e.Message}";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(){
            // if(!ModelState.IsValid){
            //     return RedirectToPage();
            // }
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                var connection = new SqlConnection(connectionString);
                    connection.Open();
                    var sql = "UPDATE employees SET ten = @ten, machucvu = @machucvu, macongty = @macongty WHERE id = @id";
                    var cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ten", ten);
                        cmd.Parameters.AddWithValue("@machucvu", machucvu);
                        cmd.Parameters.AddWithValue("@macongty", macongty);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0){
                            TempData["SuccessMessage"] = $"No employee found with the specified ID. {id}";
                            return RedirectToPage();
                        } 
                    
                
                TempData["SuccessMessage"] = $"Employee with ID: {id} successfully updated.";
             
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {e.Message}";
                return RedirectToPage();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(){
            
            if(string.IsNullOrEmpty(id)){
                TempData["ErrorMessage"] = "ID cannot be empty.";
                return RedirectToPage();
            }

            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                
                using(var connection  = new SqlConnection(connectionString)){
                    connection.Open();
                    var sql = "delete from thongtincanhannhanvien where id = @id; delete from employees where id = @id ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id",id); // lay id tai hang ma minh click 
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0){
                            TempData["SuccessMessage"] = "No employee found with the specified ID.";
                            return RedirectToPage();
                        } 
                    }                           
                }
                TempData["SuccessMessage"] = $"Employee with ID: {id} successfully deleted.";                                               
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {e.Message}";
                return RedirectToPage();
            }
        return RedirectToPage();
        }
    }

    public class EmployeesInfo {
        public  string id {get; set;}
        public  string ten {get; set;}
        public string? machucvu {get; set;}
        public string? macongty {get; set;}
    }

}