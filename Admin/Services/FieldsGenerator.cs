using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Admin.Components;
using DAL;
using System.Data.Entity;
using Admin.Core.Interfaces;

namespace Admin.Services
{
    public class FieldsGenerator
    {
        public bool IsLastDetail { get; private set; }

        public object Item { get; private set; }

        public bool IsEdit { get; private set; }

        private object _controls;

        public void SetupControls<T>(IEnumerable<IPropertyControl<T>> propertyControls, bool isEdit)
        {
            _controls = propertyControls;
            IsEdit = isEdit;
        }

        public IEnumerable<IPropertyControl<T>> GetControls<T>()
        {
            if (_controls is IEnumerable<IPropertyControl<T>> collection)
                return collection;
            throw new ArgumentException($"type of collection is not IEnumerable<IPropertyControl<{typeof(T).Name}>>");
        }

        public async Task Generate<T>(T item, BindingComponent[] bindList, bool isEdit)
        {
            var type = typeof(T);
            Item = item;
            IsEdit = isEdit;

            var list = bindList.
                Where(x => x.PropertyType != PropertyType.OuterPropertyClassNonSerializable).ToList();
            controls.Clear();

            foreach (var bind in list)
            {
                controls.Add(await GetControlFrom(bind, type));
            }

        }

        public FieldsGenerator(AllDbContext allDbContext)
        {
            this.allDbContext = allDbContext;
        }

        public void Clear()
        {
            controls.Clear();
            IsEdit = false;
            Item = null;
            IsLastDetail = false;
        }

        public void SetItem(object item)
        {
            IsLastDetail = true;
            Item = item;
        }

        List<FrameworkElement> controls = new List<FrameworkElement>();
        private readonly AllDbContext allDbContext;

        public void GetGenerated(StackPanel stackPanel)
        {
            controls.ForEach(x => stackPanel.Children.Add(x));
            controls.Clear();
        }


        private async Task<FrameworkElement> GetControlFrom(BindingComponent bind, Type modelType)
        {

            PropertyInfo info = modelType.GetProperty(bind.PropertyName);


            Control control = new TextBox();
            TextBlock label = new TextBlock() { Text = bind.DisplayName };

            control.SetBinding(TextBox.TextProperty, info.Name);

            if (info.PropertyType == typeof(DateTime))
            {
                control = new DatePicker() { DisplayDateStart = new DateTime(1960, 1, 1)};
                if (!IsEdit)
                {
                    info.SetValue(Item, DateTime.Now);
                }

                control.SetBinding(DatePicker.SelectedDateProperty, info.Name);
            }

            if (info.PropertyType.IsEnum)
            {
                var list = Enum.GetNames(info.PropertyType);
                control = new ComboBox();

                if (list.Length > 0)
                {
                    (control as ComboBox).ItemsSource = list;
                    (control as ComboBox).SetBinding(ComboBox.SelectedItemProperty, bind.PropertyName);

                    if (!IsEdit)
                    {
                        (control as ComboBox).SelectedIndex = 0;
                        info.SetValue(Item, Enum.Parse(info.PropertyType, (control as ComboBox).SelectedItem.ToString()));
                    }
                    else
                    {
                        var value = info.GetValue(Item);
                        (control as ComboBox).SelectedItem = Enum.GetName(info.PropertyType, value);
                    }
                }
            }

            if(bind.PropertyType == PropertyType.OuterPropertyId || 
                bind.PropertyType == PropertyType.OuterPropertyIdNonVisible)
            {
                control = new ComboBox();

                string outerPropName = bind.PropertyName.TrimEnd('I', 'd');
                var outerProp = modelType.GetProperty(outerPropName);

                await allDbContext.Set(outerProp.PropertyType).LoadAsync();

                var list = await allDbContext.Set(outerProp.PropertyType).ToListAsync();

                if (list.Count > 0)
                {

                    (control as ComboBox).ItemsSource = list;
                    (control as ComboBox).SelectedValuePath = "Id";
                    (control as ComboBox).DisplayMemberPath = bind.DisplayMember;

                    (control as ComboBox).SetBinding(ComboBox.SelectedValueProperty, bind.PropertyName);

                    if (!IsEdit)
                    {                        
                        (control as ComboBox).SelectedIndex = 0;
                        info.SetValue(Item, (control as ComboBox).SelectedValue);
                    }
                }
            }

            control.Margin = new Thickness(0, 5, 0, 0);
            label.HorizontalAlignment = HorizontalAlignment.Left;


            StackPanel stack = new StackPanel();
            stack.Children.Add(label);
            stack.Children.Add(control);
            stack.Margin = new Thickness(0, 15, 0, 0);

            return stack;
        }
    }


}
