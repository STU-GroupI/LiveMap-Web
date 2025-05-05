namespace LiveMapDashboard.Web.Models.Modal;

public record DeleteModalViewModel
{
    public string ModalId { get; set; } = Guid.NewGuid().ToString("N");
    public string ModalTitle { get; set; } = string.Empty;

    public string? WarningMessage { get; set; }
    public string? DangerMessage { get; set; }

    public string FormAction { get; set; } = string.Empty;
    public string FormController { get; set; } = string.Empty;
    public string FormMethod { get; set; } = "post";

    public Dictionary<string, string> HiddenInputs { get; set; } = new();
}