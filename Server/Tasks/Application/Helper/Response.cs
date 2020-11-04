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

        public Response(T data)
        {
            Data = data;
            OK = true;
        }

        public Response(bool ok, string message)
        {
            OK = ok;
            Message = message;
        }

        public T Data { get; set; }
        public bool OK { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
