using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Service.WebSite.Domain;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var display = field?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? value.ToString();
    }

    public static string? GetGroupName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var display = field?.GetCustomAttribute<DisplayAttribute>();
        return display?.GroupName;
    }
}
