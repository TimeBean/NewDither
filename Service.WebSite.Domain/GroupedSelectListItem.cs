namespace Service.WebSite.Domain;

public class GroupedSelectListItem
{
    public string? GroupName { get; init; }
    public string Value { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public bool Selected { get; init; }
}
