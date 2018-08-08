using System.Collections.Generic;

namespace Api.Models
{
    public class ApiRequest
    {
        public IEnumerable<string> Host { get; set; }
        public IEnumerable<string> Request { get; set; }
    }
}
