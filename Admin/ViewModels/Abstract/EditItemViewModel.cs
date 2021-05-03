using Admin.Core.Interfaces;
using Admin.Services;
using Admin.ViewModels.Interfaces;
using MVVM_Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Admin.ViewModels
{
    public class EditItemViewModel<T> : BasePageViewModel, IEditItemViewModel where T: class
    {
        private readonly PageService pageservice;
        private readonly FieldsGenerator fieldsGenerator;
        IEnumerable<IPropertyControl<T>> _props;
        IEnumerable<IPropertyControl<T>> _propsForValid;
        Type _type = typeof(T);

        public EditItemViewModel(PageService pageservice, Services.FieldsGenerator fieldsGenerator) : base(pageservice)
        {
            this.pageservice = pageservice;
            this.fieldsGenerator = fieldsGenerator;
            SelectedItem = fieldsGenerator.Item as T;
            _props = fieldsGenerator.GetControls<T>();
            _propsForValid = _props.Where(x => x.ValidRule != null);
            SetupStackPanel(_props);
        }

        public T SelectedItem { get; set; }

        public ICommand Accept => new Command(x =>
        {
            if (Check())
            {
                fieldsGenerator.SetItem(SelectedItem);
                Stack.Children.Clear();
                pageservice.ChangePage<Pages.ItemsPage>(DisappearAndToSlideAnim.ToRight);
            }
        });

        public ICommand Cancel => new Command(x =>
        {
            fieldsGenerator.Clear();
            Stack.Children.Clear();
            pageservice.Back<Pages.ItemsPage>(PoolIndex, DisappearAndToSlideAnim.ToRight);
            
        });

        void SetupStackPanel(IEnumerable<IPropertyControl<T>> props)
        {
            Stack.Children.Clear();
            foreach(var item in props)
            {
                var c = item.Control;


                if (c.Parent is Panel p)
                    p.Children.Clear();


                StackPanel stack = new StackPanel();
                if (item.DisplayName != null)
                {
                    TextBlock label = new TextBlock { Text = item.DisplayName, FontSize = 14 };
                    stack.Children.Add(label);
                }
                stack.Children.Add(c);
                stack.Margin = new Thickness(0, 15, 0, 0);


                Stack.Children.Add(stack);
            }
            Stack.DataContext = SelectedItem;
        }

        public string ErrorMessage { get; private set; }
        public bool IsErrorVisible { get; private set; }
        bool Check()
        {
            IsErrorVisible = false;

            _type = typeof(T);

            foreach(var p in _propsForValid)
            {
                var info = _type.GetProperty(p.PropertyName);
                var value = info.GetValue(SelectedItem);

                if (!p.ValidRule.IsCorrectValue(value))
                {
                    IsErrorVisible = true;
                    ErrorMessage = p.ValidRule.Messages.First();
                    return false;
                }

            }

            return true;
        }

        public override int PoolIndex => 1;

        public StackPanel Stack { get; set; } = new StackPanel();
    }
}