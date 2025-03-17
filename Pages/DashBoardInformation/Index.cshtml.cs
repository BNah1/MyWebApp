using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MyWebApp.Pages.DashBoardInformation
{
    public class Index : PageModel
    {
        public List<Information> list { get; set;} = [];
        public string ErrorMessage { get; set;} = "";


        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                  connection.Open();   
                  var sql = "select e.id,e.ten,t.email,t.sodienthoai,c.tenchucvu,y.tencongty from employees e JOIN thongtincanhannhanvien t on e.id = t.id JOIN chucvu c on e.machucvu = c.machucvu JOIN congty y on e.macongty = y.macongty";
                  using (var cmd = new SqlCommand(sql, connection))
                  {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()){
                            Information info = new Information();
                            info.id = reader.GetString(0);
                            info.ten = reader.GetString(1);
                            info.email = reader.GetString(2);
                            info.sodienthoai = reader.GetString(3);
                            info.tenchucvu = reader.GetString(4);
                            info.tencongty = reader.GetString(5);        
                            list.Add(info);     
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



        public class Information{
            public string id { get; set;}
            public string ten { get; set;}
            public string email { get; set;} = "";
            public string sodienthoai  { get; set;} = "";
            public string tenchucvu { get; set;} = "";
            public string tencongty { get; set;} = "";
        }


    }
}