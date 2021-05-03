using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Events
{
    public class UpdatePipe: IEvent
    {
        private readonly Type modelType;
        private readonly object id;

        public UpdatePipe(Type modelType, object id = null)
        {
            this.modelType = modelType;
            this.id = id;
        }
        public string GetString()
        {
            string message = modelType.FullName;
            message += id != null ? $"/{id}" : string.Empty;
            return message;

        }
    }
}
