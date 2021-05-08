using Admin.Core;
using Admin.Services;
using DAL;
using DAL.Models;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class InsViewModel : ItemsViewModel<Insurance>
    {
        public InsViewModel(PageService pageservice,
                            AllDbContext dbContext,
                            FieldsGenerator fieldsGenerator,
                            CloneItemsSerivce cloneItems,
                            EventBus eventBus) : base(pageservice, dbContext, fieldsGenerator, cloneItems, eventBus)
        {
        }


        protected override void OnColumnsBuild(ColumnBuilder<Insurance> columnBuilder)
        {
            columnBuilder.AddColumn(x => nameof(x.Name), "Название");
            columnBuilder.AddColumn(x => nameof(x.Cost), "Стоимость");
        }

        protected override void OnPropertiesBuild(PropertiesBuilder<Insurance> propertiesBuilder)
        {
            propertiesBuilder.AddStringProperty(x => x.Name, "Название", y => y.NotNull().NotEmpty());
            propertiesBuilder.AddValueTypeProperty(x => x.Cost, "Стоимость", y => y.MoreEquialThan(0));
        }
    }
}
