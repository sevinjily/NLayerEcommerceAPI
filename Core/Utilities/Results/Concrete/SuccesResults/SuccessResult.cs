﻿using System.Net;

namespace Core.Utilities.Results.Concrete.SuccesResults
{
    public class SuccessResult:Result
    {
        public SuccessResult(string message,HttpStatusCode statusCode):base(message,true,statusCode)
        {
            
        }
        public SuccessResult(HttpStatusCode statusCode):base(true,statusCode)
        {
            
        }
    }
}
