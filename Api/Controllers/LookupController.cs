using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Lookup")]
    public class LookupController : Controller
    {
        [HttpPost]
        public IEnumerable<NsResponse> Post(ApiRequest request)
        {
            List<NsResponse> res = new List<NsResponse>();
            if (request.Host.Count() <= 1000)
            {
                Parallel.ForEach(request.Host, (host) =>
                {
                    if (!string.IsNullOrWhiteSpace(host))
                    {
                        var cur = NsQuery.Query(host, request.Request.ToList());
                        lock (res)
                        {
                            res.Add(cur);
                        }
                    }
                });
            }
            return res;
        }
    }
}