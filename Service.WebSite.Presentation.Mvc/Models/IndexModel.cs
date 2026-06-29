using Service.WebSite.Domain;

namespace Service.WebSite.Presentation.Mvc.Models;

public class IndexModel
{
    public Quote.Domain.Model.Quote RandomQuote { get; }
    public string? ResultImageBase64 { get; }
    public long? ExecutionTime { get; }
    public string? ErrorMessage { get; }
    public string? FileName { get; }
    public DitherAlgorithm SelectedDitherAlgorithm { get; }
    public QuantizationAlgorithm SelectedQuantizationAlgorithm { get; }

    public List<GroupedSelectListItem> QuantizationOptions { get; }
    public List<GroupedSelectListItem> DitherOptions { get; }

    public IndexModel(Quote.Domain.Model.Quote randomQuote,
        string? resultImageBase64 = null,
        long? executionTime = null,
        string? errorMessage = null,
        string? fileName = null,
        DitherAlgorithm selectedDitherAlgorithm = DitherAlgorithm.QuantizeOnly,
        QuantizationAlgorithm selectedQuantizationAlgorithm = QuantizationAlgorithm.Linear2)
    {
        RandomQuote = randomQuote;
        ResultImageBase64 = resultImageBase64;
        ExecutionTime = executionTime;
        ErrorMessage = errorMessage;
        FileName = fileName;
        SelectedDitherAlgorithm = selectedDitherAlgorithm;
        SelectedQuantizationAlgorithm = selectedQuantizationAlgorithm;

        QuantizationOptions = Enum.GetValues<QuantizationAlgorithm>()
            .Select(v => new GroupedSelectListItem
            {
                Value = v.ToString(),
                Text = v.GetDisplayName(),
                Selected = v == selectedQuantizationAlgorithm
            })
            .ToList();

        DitherOptions = Enum.GetValues<DitherAlgorithm>()
            .Select(v => new GroupedSelectListItem
            {
                GroupName = v.GetGroupName(),
                Value = v.ToString(),
                Text = v.GetDisplayName(),
                Selected = v == selectedDitherAlgorithm
            })
            .ToList();
    }
}