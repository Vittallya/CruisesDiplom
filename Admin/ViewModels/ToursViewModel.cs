using Admin.Components;
using Admin.Core;
using Admin.Services;
using DAL;
using DAL.Models;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Admin.ViewModels
{
    public class ToursViewModel : ItemsViewModel<Tour>
    {
        public ToursViewModel(PageService pageservice, 
            AllDbContext dbContext, 
            FieldsGenerator fieldsGenerator, 
            CloneItemsSerivce cloneItems, 
            EventBus eventBus) : base(pageservice, dbContext, fieldsGenerator, cloneItems, eventBus)
        {
        }

        protected override void OnColumnsBuild(ColumnBuilder<Tour> columnBuilder)
        {
            columnBuilder.AddColumn(x => nameof(x.Name), "Название");
            columnBuilder.AddColumn(x => nameof(x.Cost), "Стоимость");
            columnBuilder.AddColumn(x => nameof(x.ChildCost), "Стоимость за ребенка");
            columnBuilder.AddColumn(x => nameof(x.DaysCount), "Длительность тура (д.)");
        }
        protected override async Task LoadItems()
        {
            await dbContext.Layners.LoadAsync();
            await dbContext.Set<Tour>().Include(x => x.Layner).LoadAsync();
            Items = new ObservableCollection<Tour>(dbContext.Set<Tour>());
        }

        protected override void OnSetDefaults(Tour item)
        {
            item.StartDate = DateTime.Now.AddDays(1);
            
        }

        protected override async void OnPropertiesBuild(PropertiesBuilder<Tour> propertiesBuilder)
        {
            List<Layner> layners;
            while (true)
            {
                try
                {
                    layners = dbContext.Layners.AsNoTracking().ToList();
                    break;
                }
                catch (Exception) { await Task.Delay(500); }
            }

            propertiesBuilder.AddStringProperty(x => x.Name, "Название тура", y => y.NotEmpty().NotNull());
            propertiesBuilder.AddValueTypeProperty(x => x.Cost, "Стоимость (р.)",
                y => y.MoreEquialThan(0));

            propertiesBuilder.AddStringProperty(x => x.Desctiprion, "Описание тура", y => y.NotNull()).UseControl(new TextBox { TextWrapping = System.Windows.TextWrapping.Wrap });

            propertiesBuilder.AddValueTypeProperty(x => x.ChildCost, "Стоимость за ребенка (р.)", 
                y => y.MoreEquialThan(0));

            var combo = new ComboBox { ItemsSource = layners, SelectedValuePath = "Id", DisplayMemberPath = "Name" };
            combo.SetBinding(ComboBox.SelectedValueProperty, "LaynerId");


            propertiesBuilder.AddValueTypeProperty(x => x.LaynerId, "Лайнер",
                y => y.MoreThan(0, "Лайнер должен быть выбран")).UseCombobox<ComboBox, Layner>(layners, x => x.Id, x => x.Name);

            propertiesBuilder.AddValueTypeProperty(x => x.DaysCount, "Кол-во дней",
                y => y.MoreEquialThan(1));

            propertiesBuilder.AddValueTypeProperty(x => x.StartDate, "Дата отплытия",
                y => y.MoreEquialThan(DateTime.Now)).
                UseControl<DatePicker>(new DatePicker { DisplayDateStart = DateTime.Now.AddDays(1)}, bindingProp: DatePicker.SelectedDateProperty);

            propertiesBuilder.AddStringProperty(x => x.StartPlace, "Место отплытия",
                y => y.NotNull().NotEmpty());



            propertiesBuilder.AddStringProperty(x => x.ImageName, "Название изображения (включая расширение)", 
                y => y.Regex("\\w+\\.png", errorMessage: "Название изображения должно включать расширение (.png или .jpeg или .jpg)").
                Or.Regex("\\w+\\.jpg").
                Or.Regex("\\w+\\.jpeg"));
        }
    }
}
