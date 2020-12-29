using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOneFieldRequired : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //  Need to use reflection to get properties of "value"...
            var typeInfo = value.GetType();

            var propertyInfo = typeInfo.GetProperties();

            foreach (var property in propertyInfo)
            {
                if (null != property.GetValue(value, null))
                {
                    // We've found a property with a value
                    return true;
                }
            }

            // All properties were null.
            return false;
        }
    }
}





