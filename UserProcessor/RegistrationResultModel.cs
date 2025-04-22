using System.Net;

namespace UserProcessor
{
    public class RegistrationResultModel
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public HttpStatusCode ResponseCode { get; set; }
    }
}
