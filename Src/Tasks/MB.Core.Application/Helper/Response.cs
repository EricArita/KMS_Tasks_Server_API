using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MB.Core.Application.Helper
{
    public class HttpResponse<T> { 
        public HttpResponse() {
            OK = true;
            Message = string.Empty;
            Errors = null;
            Data = default(T);
        }

        public HttpResponse(bool ok, T data = default(T), string message = "", IEnumerable<object> errors = null)
        {
            Data = data;
            OK = ok;
            Message = message;
            Errors = errors;
        }

        public T Data { get; set; }
        public bool OK { get; set; }
        public IEnumerable<object> Errors { get; set; }
        public string Message { get; set; }
    }
}
