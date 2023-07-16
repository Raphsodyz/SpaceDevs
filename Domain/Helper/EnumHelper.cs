using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            try
            {
                DisplayAttribute display = enumValue?.GetType().GetField(enumValue.ToString()).GetCustomAttribute<DisplayAttribute>(inherit: false);
                if (display != null)
                {
                    string name = display.GetName();
                    if (!string.IsNullOrEmpty(name))
                    {
                        return name;
                    }
                }

                return enumValue?.ToString();
            }
            catch
            {
                return string.Empty;
            }

        }
    }
}
