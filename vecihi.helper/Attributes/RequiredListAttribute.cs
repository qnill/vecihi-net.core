using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace vecihi.helper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IList list)
                return list.Count > 0;

            return false;
        }
    }
}