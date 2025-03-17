using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MyWebApp.Pages.Employees
{
    public class Create : PageModel
    {
        [BindProperty, Required(ErrorMessage = "id is required")]
        public required string id {get; set;}
        [BindProperty, Required(ErrorMessage = "ten is required")]
        public required string ten {get; set;}
        [BindProperty]
        public string machucvu {get; set;} = "void";
        [BindProperty]
        public string macongty {get; set;} = "void";
        public string ErrorMessage { get; set;} = "";

        public void OnPost()
        {
            if (!ModelState.IsValid){
                return;
            }

            if (machucvu == null) machucvu = "";
            if (macongty == null) machucvu = "";

            // create new object
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "insert into employees " +
                        "(id,ten,machucvu,macongty) values " +
                        "(@id,@ten,@machucvu,@macongty);";

                SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@machucvu", machucvu);
                    cmd.Parameters.AddWithValue("@macongty", macongty);
                    cmd.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return;
            }

            Response.Redirect("/Employees/Index");
        }


       

    }
}