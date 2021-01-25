using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AuthServer.Models
{
    public class HttpResponse<T> { 
        public HttpResponse() {
            OK = true;
            Message = string.Empty;
            Errors = null;
            Data = default(T);
        }

        public HttpResponse(bool ok, T data = default(T), string message = "", List<IdentityError> errors = null)
        {
            this.Data = data;
            this.OK = ok;
            this.Message = message;
            this.Errors = errors;
        }

        public T Data { get; set; }
        public bool OK { get; set; }
        public List<IdentityError> Errors { get; set; }
        public string Message { get; set; }
    }
}
