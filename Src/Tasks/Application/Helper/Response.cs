using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Core.Application.Helper
{
    public class Response<T> { 
        public Response() {
            OK = true;
            Message = string.Empty;
            Errors = null;
            Data = default(T);
        }

        public Response(bool ok, T data = default(T), string message = "", List<IdentityError> errors = null)
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
