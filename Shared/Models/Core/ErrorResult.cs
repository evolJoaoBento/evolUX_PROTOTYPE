﻿namespace Shared.Models.Areas.Core
{
    public class ErrorResult
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int Code { get; set; }
    }
}
