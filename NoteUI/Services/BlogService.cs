using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using NoteUI.Models;
using NoteUI.Services.Configure;

namespace NoteUI.Services
{
    public class BlogService
    {
        private readonly string _urlApi = "";
        readonly private string _statusSuccess = "200";
        readonly CallApiService _callApiService;
        readonly ConvertDataService _convertDataService;

        ConfigureService _configureService = new ConfigureService();
        IConfiguration _getConfig;

        public BlogService()
        {
            _callApiService = new CallApiService();
            _convertDataService = new ConvertDataService();

            _getConfig = _configureService.GetConfigFromAppsetting();
            _urlApi = _getConfig["URLAPI:URL_NOTE_API"].ToString();
        }

        internal DataSet GetBlogList(ref string messageErrorFromService)
        {
            try
            {
                string url = _urlApi + "Blog/GetBlogList";
                var resultFromAPI = this._callApiService.Get(url);

                ResultModel objGet = JsonConvert.DeserializeObject<ResultModel>(resultFromAPI.Result);
                string statusObjectGet = objGet.status.ToString();
                if(statusObjectGet == _statusSuccess)
                {
                    List<BlogModel> objGetData = JsonConvert.DeserializeObject<List<BlogModel>>(objGet.data.ToString());

                    return this._convertDataService.ConvertListToDataSet(objGetData);
                }
                else
                {
                    messageErrorFromService += " Not Found GetBlogList;";
                    return null;
                }
            }
            catch
            {
                messageErrorFromService += " Not Connection API Number/GetBlogList;";
                return null;
            }
        }

        internal BlogModel GetBlogById(string blogId, ref string messageErrorFromService)
        {
            try
            {
                string url = _urlApi + "Blog/GetBlogById/" + blogId;
                var resultFromAPI = this._callApiService.Get(url);

                ResultModel objGet = JsonConvert.DeserializeObject<ResultModel>(resultFromAPI.Result);
                string statusObjectGet = objGet.status.ToString();
                if (statusObjectGet == _statusSuccess)
                {
                    BlogModel objGetData = JsonConvert.DeserializeObject<BlogModel>(objGet.data.ToString());

                    return objGetData;
                }
                else
                {
                    messageErrorFromService += " Not Found GetBlogById;";
                    return null;
                }
            }
            catch
            {
                messageErrorFromService += " Not Connection API Number/GetBlogById;";
                return null;
            }
        }

        public string SaveNewBlog(SaveNewBlogModel saveNewBlogModel, ref string messageError)
        {
            string url = _urlApi + "Blog/SaveNewBlog";

            try
            {
                var resultFromAPI = this._callApiService.Post(url, saveNewBlogModel);
                ResultModel objGet = JsonConvert.DeserializeObject<ResultModel>(resultFromAPI.Result);
                string statusObjectGet = objGet.status.ToString();
                string idNewBlog = objGet.data.ToString();

                if (statusObjectGet == _statusSuccess)
                {
                    return "200";
                }
                else
                {
                    messageError += "Submit data fail";
                    return "502";
                }
            }
            catch
            {
                messageError += "Submit data fail server;";
                return "503";
            }
        }
    }
}

