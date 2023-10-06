using NimapInfotechMachineTest.Models;
using NimapInfotechMachineTest.SqlDbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NimapInfotechMachineTest.Controllers
{
    public class ProductController : Controller
    {
        #region
        SqlConnection scon;
        SqlCommand cmd;
        Connection con;
        SqlDataAdapter da;
        #endregion
        public ActionResult Index()
        {
            ViewBag.Category = CategoryList();
            return View();
        }
        public ActionResult Prtial_View()
        {
            return View();
        }
        public List<SelectListItem> CategoryList()
        {
            DataTable dt = new DataTable();
            var _selectList = new List<SelectListItem>();
            _selectList.Add(new SelectListItem { Value = "0", Text = "--Select--" });
            try
            {
                con = new Connection();
                dt = con.FillCombo("Select * From MCategory Where IsActiv='True'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _selectList.Add(new SelectListItem { Value = dt.Rows[i]["CategoryId"].ToString(), Text = dt.Rows[i]["CategoryName"].ToString() });

                }

            }
            catch (Exception ex)
            {

            }
            return _selectList;

        }
        public ActionResult SaveOrUpdate(ProductModel model)
        {
            int _server_responce = 0;
            int _return = 0;
            string Flag = "";
            try
            {
                if (model.ProductId == 0)
                {
                    Flag = "I";
                    _return = 1;
                    _server_responce = 1;
                }
                else
                {
                    Flag = "U";
                    _return = 2;
                    _server_responce = 2;
                }
                con = new Connection();
                scon = con.Connect();
                cmd = new SqlCommand();
                cmd.CommandText = "SpProduct";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = scon;
                cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);               
                cmd.Parameters.AddWithValue("@Flag", Flag);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _server_responce = 3;
            }
            finally
            {
                cmd.Dispose();
                scon.Close();
            }
            if (_server_responce == 1)
            {
                TempData["message"] = "Saved Data..";
            }
            else if (_server_responce == 2)
            {
                TempData["message"] = "Your Data Updated Successfuly";
            }
            else
            {
                TempData["Error"] = "Opps Somthing went Wrong !!!";
            }
            return RedirectToAction("index");
        }
        public ActionResult ProductReport()
        {
            List<ProductReportModel> _list = new List<ProductReportModel>();
            DataTable dt = new DataTable();
            con = new Connection();
            dt = con.FillCombo("Select ProductId,	ProductName,C.CategoryId,CategoryName From MProduct P inner join MCategory C on C.CategoryId=P.CategoryId");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductReportModel model = new ProductReportModel();

                model.ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]);
                model.ProductName = dt.Rows[i]["ProductName"].ToString();
                model.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                model.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                _list.Add(model);
            }
            return View(_list);


        }
        public ActionResult EditData(int id)
        {
            ViewBag.Category = CategoryList();

            DataTable dt = new DataTable();
            con = new Connection();
            dt = con.FillCombo("Select * From MProduct Where ProductId=" + id);

            {
                ProductModel model = new ProductModel();
                {

                    model.ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]);
                    model.ProductName = dt.Rows[0]["ProductName"].ToString();                  
                    model.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                }
                return PartialView("index", model);

            }
        }
        public ActionResult ReportDelete(int id)
        {
            ViewBag.Category = CategoryList();

            try
            {
                List<ProductReportModel> _list = new List<ProductReportModel>();
                Connection Con = new Connection();
                DataTable dt = new DataTable();
                con = new Connection();
                ProductModel model = new ProductModel();
                model.ProductId = id;
                dt = con.FillCombo("Delete From MProduct Where ProductId =" + id);
                TempData["Delete"] = "Your Data Delete Sucessfully!";
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            return View("Index");

        }
    }
}