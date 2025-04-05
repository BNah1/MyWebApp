using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MyWebApp.Pages.DashBoardInformation
{
    public class Index : PageModel
    {
        public List<Information> list { get; set;} = [];
        public string ErrorMessage { get; set;} = "";
        
        // Search properties
        [BindProperty (SupportsGet = true)]
        public string? searchId { get; set;}
        [BindProperty (SupportsGet = true)]
        public string? searchTen { get; set;}
        [BindProperty (SupportsGet = true)]
        public string? searchEmail { get; set;}
        [BindProperty (SupportsGet = true)]
        public string? searchSDT { get; set;}
        [BindProperty (SupportsGet = true)]
        public string? searchTenChucVu { get; set;}
        [BindProperty (SupportsGet = true)]
        public string? searchTenCongTy { get; set;}



        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost;Database=StaffManagement;User ID=sa;Password=@Nhan1234;Encrypt=False;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                  connection.Open();   
                  var sql = "select e.id,e.ten,t.email,t.sodienthoai,c.tenchucvu,y.tencongty from employees e JOIN thongtincanhannhanvien t on e.id = t.id JOIN chucvu c on e.machucvu = c.machucvu JOIN congty y on e.macongty = y.macongty WHERE 1=1";
                  var parameters = new List<SqlParameter>();

                  if (!string.IsNullOrEmpty(searchId)){
                    sql += " and e.id like @searchId";
                    parameters.Add(new SqlParameter("@searchId", $"%{searchId}%"));
                  }
                  
                  if (!string.IsNullOrEmpty(searchTen)){
                    sql += " and e.ten like @searchTen";
                    parameters.Add(new SqlParameter("@searchTen", $"%{searchTen}%"));
                  }                  
                  
                  if (!string.IsNullOrEmpty(searchEmail)){
                    sql += " and t.email like @searchEmail";
                    parameters.Add(new SqlParameter("@searchEmail", $"%{searchEmail}%"));
                  }                  
                  
                  if (!string.IsNullOrEmpty(searchSDT)){
                    sql += " and t.sodienthoai like @searchSDT";
                    parameters.Add(new SqlParameter("@searchSDT", $"%{searchSDT}%"));
                  }
                                    
                  if (!string.IsNullOrEmpty(searchTenChucVu)){
                    sql += " and c.tenchucvu like @searchTenChucVu";
                    parameters.Add(new SqlParameter("@searchTenChucVu", $"%{searchTenChucVu}%"));
                  }                  
                  if (!string.IsNullOrEmpty(searchTenCongTy)){
                    sql += " and y.tencongty like @searchTenCongTy";
                    parameters.Add(new SqlParameter("@searchTenCongTy", $"%{searchTenCongTy}%"));
                  }      

                  sql += " ORDER BY id DESC";


                  using (var cmd = new SqlCommand(sql, connection))
                  {
                        foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
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
                            if (!list.Any(i => i.id == info.id))
                            {
                                list.Add(info);     
                            }      
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