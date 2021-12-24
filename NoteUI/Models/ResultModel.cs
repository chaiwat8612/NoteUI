using System;

namespace NoteUI.Models
{
    public class ResultModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public object error { get; set; }
        public object data { get; set; }
    }

    public class ErrorModel
    {
        public int code { get; set; }
        public string target { get; set; }
        public object message { get; set; }
    }
}