using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core
{
    public enum ViewModelLoadType
    {
        Transient, Singleton
    }
    public class ViewModelParam: Attribute
    {
        public ViewModelParam(ViewModelLoadType loadType = ViewModelLoadType.Transient)
        {
            LoadType = loadType;
        }

        public ViewModelLoadType LoadType { get; }
    }
}
