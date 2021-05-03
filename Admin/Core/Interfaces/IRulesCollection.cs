using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Core.Interfaces
{
    public interface IRulesCollection<TModel, TInterface>
    {
        TInterface AddRule();

        void SetupInstance<TInst>(TInst inst) where TInst : class, TInterface;
    }


    class RulesCollection<TModel, TInterface> : IRulesCollection<TModel, TInterface>
    {
        public TInterface AddRule()
        {
            throw new NotImplementedException();
        }

        public void SetupInstance<TInst>(TInst inst) where TInst : class, TInterface
        {
            throw new NotImplementedException();
        }
    }
}
