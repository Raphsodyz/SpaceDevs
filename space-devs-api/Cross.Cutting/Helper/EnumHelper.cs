using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Cross.Cutting.Helper
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
