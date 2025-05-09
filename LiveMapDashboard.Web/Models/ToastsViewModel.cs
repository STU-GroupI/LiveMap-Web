namespace LiveMapDashboard.Web.Models
{
    public class ToastViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Message { get; set; } = "";
        public ToastType Type { get; set; } = ToastType.Success;
    }

    public enum ToastType
    {
        Success,
        Error,
        Info,
        Warning
    }
}