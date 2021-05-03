using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Components
{
    public enum PropertyType
    {
        Simple,

        OuterPropertyClass,

        /// <summary>
        /// Отображается, но не сериализуется в field...
        /// </summary>
        OuterPropertyClassNonSerializable,

        OuterPropertyId,

        /// <summary>
        /// Не отображается, но сериализуется в field...
        /// </summary>
        OuterPropertyIdNonVisible
    }


    public class BindingComponent
    {
        public BindingComponent(string bindingPath, PropertyType propertyType = PropertyType.Simple)
        {
            PropertyType = propertyType;
            BindingPath = bindingPath;
            DisplayName = bindingPath;
            DisplayName = bindingPath;

            if((propertyType == PropertyType.OuterPropertyClass || 
                propertyType == PropertyType.OuterPropertyClassNonSerializable) && 
                bindingPath.Length >= 3 &&
                bindingPath.Contains('.'))
            {
                PropertyName = bindingPath.Split('.')[0];
            }
            else
            {
                PropertyName = bindingPath;
            }
        }

        public BindingComponent(string bindingPath, string name, PropertyType propertyType = PropertyType.Simple):
            this(bindingPath, propertyType)
        {
            DisplayName = name;
        }

        public BindingComponent(string propName, string displayMemberPath, string displayName)
        {
            PropertyType = PropertyType.OuterPropertyIdNonVisible;
            DisplayMember = displayMemberPath;
            DisplayName = displayName;
            PropertyName = propName;
        }


        public PropertyType PropertyType { get; private set; }

        public string BindingPath { get; private set; }

        public string DisplayName { get; private set; }
        public string DisplayMember { get; private set; }
        public string PropertyName { get; private set; }
       
    }
}
