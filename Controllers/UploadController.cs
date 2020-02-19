using CRF_Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRF_Final_Project.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ImportFile import) 
        {
            if (ModelState.IsValid)
            {


                string path = Server.MapPath("~/Content/Upload/" + import.File.FileName);
                import.File.SaveAs(path);

                string excelConnectionString = @"Provider='Microsoft.ACE.OLEDB.12.0';Data Source='" + path + "';Extended Properties='Excel 12.0 Xml;IMEX=1'";
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);

                //Sheet Name
                excelConnection.Open();
                string tableName = excelConnection.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
                excelConnection.Close();
                //End

                OleDbCommand cmd = new OleDbCommand("Select * from [" + tableName + "] where Branch is not null", excelConnection);
                excelConnection.Open();

                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();
                SqlBulkCopy sqlBulk = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["CS"].ConnectionString);

                //Give your Destination table name
                sqlBulk.DestinationTableName = "CRF_Table";

                //Mappings
                sqlBulk.ColumnMappings.Add("Branch", "Branch");
                sqlBulk.ColumnMappings.Add("FPC", "FPC");
                sqlBulk.ColumnMappings.Add("Description", "Description");
                sqlBulk.ColumnMappings.Add("SKU", "SKU");
                sqlBulk.ColumnMappings.Add("Category", "Category");
                sqlBulk.ColumnMappings.Add("Target", "Target");

                sqlBulk.WriteToServer(dReader);
                excelConnection.Close();

                ViewBag.Result = "Successfully Imported";
            }
            return View();
        }
            
        
    }
}