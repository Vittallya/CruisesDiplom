using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BL
{
    public interface IRequestService
    {
        Stream ExecuteUrl(string url);
    }
}
