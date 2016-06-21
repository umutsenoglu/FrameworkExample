using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Validation
{
    public class DataAnnotionValidator : ValidatorBase
    {
        public override bool Validate<T>(T item)
        {
            List<bool> ts = new List<bool>(); ;
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    
                   // if (attribute.GetType().BaseType == (typeof(ValidationAttribute)))
                   if(typeof(ValidationAttribute).IsAssignableFrom(attribute.GetType()))
                    {
                        var validationAttribute = attribute as ValidationAttribute;
                        ts.Add(validationAttribute.IsValid(property.GetValue(item)));
                    }
                }
            }
            return !ts.Any(w => !w);
        }
       
    }
}
