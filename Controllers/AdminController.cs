using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Version_Mark_3.Models;
using Version_Mark_3.Models;

public class AdminController : Controller
{
    private IConfiguration configuration;

    public AdminController(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    // ================= LIST =================
    public IActionResult Index()
    {
        SqlConnection con = new SqlConnection(
            configuration.GetConnectionString("ConnectionString"));

        SqlCommand cmd = new SqlCommand("Admin_SelectAll", con);
        cmd.CommandType = CommandType.StoredProcedure;

        DataTable dt = new DataTable();
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        dt.Load(dr);
        con.Close();

        return View(dt);
    }

    // ================= ADD =================
    public IActionResult Add()
    {
        return View();
    }

    // ================= SAVE =================
    [HttpPost]
    public IActionResult Save(AdminModel model)
    {
        SqlConnection con = new SqlConnection(
            configuration.GetConnectionString("ConnectionString"));

        SqlCommand cmd = new SqlCommand("Admin_Insert", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@adminName", model.AdminName);
        cmd.Parameters.AddWithValue("@adminMob", model.AdminMob);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        return RedirectToAction("Index");
    }
}
