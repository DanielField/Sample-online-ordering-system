using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CKService
{
    public class ServiceResult
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }

        public ServiceResult()
        {
            ErrorCode = 0;
            Message = "Successful operation";
            ExceptionType = "N/A";
            ExceptionMessage = "N/A";
        }

        public ServiceResult(int code, string msg, Exception ex)
        {
            ErrorCode = code;
            Message = msg;
            ExceptionType = ex.GetType().ToString();
            ExceptionMessage = ex.Message;
        }
    }
}