using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace BL
{
    public class RequestService : IRequestService
    {

        public Stream ExecuteUrl(string url)
        {
            var req = WebRequest.Create(url);
            req.Credentials = CredentialCache.DefaultCredentials;

            WebResponse resp = req.GetResponse();
            return resp.GetResponseStream();
        }
    }
}
