namespace Core.Application.Helper
{
    public class Response<T>
    {
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

        public T Data { get; set; }
        public bool OK { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
