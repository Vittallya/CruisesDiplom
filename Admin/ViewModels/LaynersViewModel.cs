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
using System.Windows.Controls;

namespace Admin.ViewModels
{
    public class LaynersViewModel : ItemsViewModel<Layner>
    {
        public LaynersViewModel(PageService pageservice, AllDbContext dbContext, FieldsGenerator fieldsGenerator, CloneItemsSerivce cloneItems, EventBus eventBus) : base(pageservice, dbContext, fieldsGenerator, cloneItems, eventBus)
        {
        }

        protected override void OnColumnsBuild(ColumnBuilder<Layner> columnBuilder)
        {
            columnBuilder.AddColumn(x => nameof(x.Name), "Название лайнера");
            columnBuilder.AddColumn(x => nameof(x.Descr), "Описание");
            columnBuilder.AddColumn(x => nameof(x.ImageName), "Изображение");
        }

        protected override void OnPropertiesBuild(PropertiesBuilder<Layner> propertiesBuilder)
        {
            propertiesBuilder.AddStringProperty(x => x.Name, "Название лайнера", y => y.NotEmpty());
            propertiesBuilder.AddStringProperty(x => x.Descr, "Описание лайнера", y => y.NotNull()).UseControl(new TextBox {TextWrapping = System.Windows.TextWrapping.Wrap });
            propertiesBuilder.AddStringProperty(x => x.ImageName, "Изображение (включая расширение)",
                y => y.Regex("\\w+\\.png", errorMessage: "Название изображения должно включать расширение (.png или .jpeg или .jpg)").
                Or.Regex("\\w+\\.jpg").
                Or.Regex("\\w+\\.jpeg"));
        }
    }
}
