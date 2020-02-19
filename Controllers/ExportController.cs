using CRF_Final_Project.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRF_Final_Project.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        [HttpPost]
        public ActionResult ExcelExport()
        {
            CRFDBEntities context = new CRFDBEntities();

            List<CRF_Table> FileData = context.CRF_Table.ToList();


            try
            {

                DataTable Dt = new DataTable();

                Dt.Columns.Add("Branch", typeof(string));
                Dt.Columns.Add("FPC", typeof(string));
                Dt.Columns.Add("Description", typeof(string));
                Dt.Columns.Add("SKU", typeof(string));
                Dt.Columns.Add("Category", typeof(string));
                Dt.Columns.Add("Target", typeof(string));
                foreach (var data in FileData)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.Branch;
                    row[1] = data.FPC;
                    row[2] = data.Description;
                    row[3] = data.SKU;
                    row[4] = data.Category;
                    row[5] = data.Target;

                    Dt.Rows.Add(row);

                }

                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    if (Session["DownloadExcel_FileManager"] != null)
                    {
                        byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                        return File(data, "application/octet-stream", "FileManager.xlsx");
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

    }
}