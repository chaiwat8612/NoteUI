using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NoteUI.Models;
using NoteUI.Services;
using NoteUI.Services.Configure;

namespace NoteUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly BlogService _blogService;
        private string messageErrorFromService = "";

        ConfigureService _configureService = new ConfigureService();
        IConfiguration _getConfig;

        BlogResultModel blogResultModel;

        public BlogController(IHostingEnvironment hostingEnvironment)
        {
            _blogService = new BlogService();
            _hostingEnvironment = hostingEnvironment;
            _getConfig = _configureService.GetConfigFromAppsetting();
            blogResultModel = new BlogResultModel();
        }

        public IActionResult ResultAllBlog()
        {
            DataSet DSBlog = new DataSet();
            DSBlog = this._blogService.GetBlogList(ref messageErrorFromService);

            if (DSBlog != null)
            {
                if (DSBlog.Tables.Count == 0)
                {
                    ViewBag.errData = "ไม่พบข้อมูล";
                    return View();

                }
                else if (DSBlog.Tables[0].Rows.Count == 0)
                {
                    ViewBag.errData = "ไม่พบข้อมูล";
                    return View();
                }
                else
                {
                    List<BlogModel> blogList = new List<BlogModel>();
                    DataTable dtBlog = DSBlog.Tables[0];

                    foreach (DataRow dr in DSBlog.Tables[0].Rows)
                    {
                        BlogModel blogModel = new BlogModel();
                        blogModel.blogId = dr["blogId"].ToString().Trim();

                        blogList.Add(blogModel);
                    }

                    blogResultModel.blogList = blogList;

                }

            }

            return View(blogResultModel);
        }

        public IActionResult BlogDetail(string blogId)
        {
            BlogModel blogModel = this._blogService.GetBlogById(blogId, ref messageErrorFromService);
            
            if (blogModel == null)
            {
                ViewBag.errData = "ไม่พบข้อมูล";
                return View();
            }
            
            return View(blogModel);
        }

        public IActionResult NewBlog()
        {
            return View();
        }

        public IActionResult NewSummer()
        {
            return View();
        }

        public IActionResult NewCK()
        {
            return View();
        }

        [HttpPost]
        public int Add(int number1, int number2)
        {
            return number1 + number2;
        }

        [HttpPost]
        public string AddBlogPage(string blogDescription)
        {
            try
            {
                SaveNewBlogModel saveNewBlogModel = new SaveNewBlogModel();
                saveNewBlogModel.blogDescription = blogDescription;

                string saveStatus = "";

                saveStatus = this._blogService.SaveNewBlog(saveNewBlogModel, ref messageErrorFromService);

                return "Success";

            }
            catch (Exception ex)
            {
                return "Fail " + ex.Message;
            }

            //return "Data sent to AddBlogPage";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
