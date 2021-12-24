﻿using System;
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
    public class NumberController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly NumberService _numberService;
        private string messageErrorFromService = "";

        ConfigureService _configureService = new ConfigureService();
        IConfiguration _getConfig;

        NumberResultModel numberResultModel;

        public NumberController(IHostingEnvironment hostingEnvironment)
        {
            _numberService = new NumberService();
            _hostingEnvironment = hostingEnvironment;
            _getConfig = _configureService.GetConfigFromAppsetting();
            numberResultModel = new NumberResultModel();
        }

        public IActionResult NewNumber()
        {
            return View();
        }

        public IActionResult SubmitNewNumber(SaveNewNumberModel model)
        {
            try
            {
                string saveStatus = "";

                saveStatus = this._numberService.SaveNewNumber(model, ref messageErrorFromService);

                return RedirectToAction("ResultAllNumber", "Number", new { statusResult = saveStatus });

            }
            catch (Exception)
            {
                return RedirectToAction("NewNumber", "Number", new { statusResult = "504" });
            }

            //return RedirectToAction("ResultNumber", "Home");
        }

        public IActionResult ResultAllNumber()
        {
            DataSet DSNumber = new DataSet();
            DSNumber = this._numberService.GetNumberList(ref messageErrorFromService);

            if (DSNumber != null)
            {
                if (DSNumber.Tables.Count == 0)
                {
                    ViewBag.errData = "ไม่พบข้อมูล";
                    return View();

                }
                else if (DSNumber.Tables[0].Rows.Count == 0)
                {
                    ViewBag.errData = "ไม่พบข้อมูล";
                    return View();
                }
                else
                {
                    List<NumberModel> numberList = new List<NumberModel>();
                    DataTable dtNumber = DSNumber.Tables[0];

                    foreach (DataRow dr in DSNumber.Tables[0].Rows)
                    {
                        NumberModel numberModel = new NumberModel();
                        numberModel.numberId = dr["numberId"].ToString().Trim();
                        numberModel.status = dr["status"].ToString().Trim();
                        numberModel.numberValue = int.Parse(dr["numberValue"].ToString());

                        numberList.Add(numberModel);
                    }

                    numberResultModel.numberList = numberList;
                    
                }

            }

            return View(numberResultModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
