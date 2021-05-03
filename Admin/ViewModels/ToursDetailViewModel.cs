using Admin.Services;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class ToursDetailViewModel<TModel> : EditItemViewModel<TModel>
        where TModel : class
    {
        public ToursDetailViewModel(PageService pageservice, FieldsGenerator fieldsGenerator) : base(pageservice, fieldsGenerator)
        {
        }


    }
}
