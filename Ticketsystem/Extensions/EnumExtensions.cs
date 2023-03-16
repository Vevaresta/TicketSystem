using System.ComponentModel;

namespace Ticketsystem.Extensions
{
    public static class EnumExtensions
    {
        public static string GetText(this Enum value)
        {
            var memberInfo = value.GetType().GetMember(value.ToString())[0];
            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DescriptionAttribute));
            return descriptionAttribute.Description;
        }
    }
}
