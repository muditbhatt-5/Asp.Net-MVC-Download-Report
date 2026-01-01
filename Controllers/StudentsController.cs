using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Version_Mark_3.Models;

namespace Version_Mark_3.Controllers
{
    public class StudentsController : Controller
    {
        private IConfiguration configuration;

        public StudentsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        { 
            string constr = configuration.GetConnectionString("ConnectionString");

            using SqlConnection con = new SqlConnection(constr);
            using SqlCommand cmd = new SqlCommand("Student_SelectAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View(dt);
        }

        #region Add Edit
        public IActionResult AddEdit(int? StudentID)
        {
            StudentsModel model = new StudentsModel();
            model.Adminlist = GetAdmin();

            if(StudentID!=null)
            {
                string constr = configuration.GetConnectionString("ConnectionString");

                using SqlConnection con = new SqlConnection(constr);
                using SqlCommand cmd = new SqlCommand("Student_SelectById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", StudentID);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    model.StudentID = Convert.ToInt32(reader["StudentID"]);
                    model.StudentName = reader["StudentName"].ToString();
                    model.StudentAge = Convert.ToInt32(reader["StudentAge"]);
                    model.StudentMob = Convert.ToInt32(reader["StudentMob"]);
                    model.AdminID = Convert.ToInt32(reader["AdminID"]);
                }
            }
            return View(model);
        }
        #endregion

        [HttpPost]
        public IActionResult Save(StudentsModel model)
        {
            string constr = configuration.GetConnectionString("ConnectionString");

            using SqlConnection con = new SqlConnection(constr);
            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;

            if(model.StudentID==null)
            {
                cmd.CommandText = "Student_Insert";
            }
            else
            {
                cmd.CommandText = "Student_Update";
                cmd.Parameters.AddWithValue("@studentID", model.StudentID);
            }
            cmd.Parameters.AddWithValue("@studentName", model.StudentName);
            cmd.Parameters.AddWithValue("@studentAge", model.StudentAge);
            cmd.Parameters.AddWithValue("@studentMob", model.StudentMob);
            cmd.Parameters.AddWithValue("@adminID", model.AdminID);

            con.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int StudentID)
        {
            string constr = configuration.GetConnectionString("ConnectionString");

            using SqlConnection con = new SqlConnection(constr);
            using SqlCommand cmd = new SqlCommand("Student_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@studentID", StudentID);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        private List<AdminModel> GetAdmin()
        {
            List<AdminModel> list = new List<AdminModel>();
            string conStr = configuration.GetConnectionString("ConnectionString");

            using SqlConnection con = new SqlConnection(conStr);
            using SqlCommand cmd = new SqlCommand("Admin_SelectAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                list.Add(new AdminModel
                {
                    AdminID = Convert.ToInt32(reader["AdminID"]),
                    AdminName = reader["AdminName"].ToString(),
                    AdminMob = Convert.ToInt32(reader["AdminMob"])
                });
            }
            return list;
        }

        //public IActionResult DownloadReport()
        //{
        //    string constr = configuration.GetConnectionString("ConnectionString");
        //    DataTable dt = new DataTable();

        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using SqlCommand cmd = new SqlCommand("Student_SelectAll", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        dt.Load(reader);
        //    }

        //    StringBuilder sb = new StringBuilder();

        //    // Header
        //    sb.AppendLine("StudentID,StudentName,StudentAge,StudentMob,AdminID,AdminName,AdminMob");

        //    // Rows
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        sb.AppendLine($"{row["StudentID"]},{row["StudentName"]},{row["StudentAge"]},{row["StudentMob"]},{row["AdminID"]},{row["AdminName"]},{row["AdminMob"]}");
        //    }

        //    return File(
        //        Encoding.UTF8.GetBytes(sb.ToString()),
        //        "text/csv",
        //        "Student_Report.csv"
        //    );
        //}

    }
}
