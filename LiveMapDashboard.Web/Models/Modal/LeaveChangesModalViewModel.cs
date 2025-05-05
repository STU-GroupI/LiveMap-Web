namespace LiveMapDashboard.Web.Models.Modal
{
    public class LeaveChangesModalViewModel
    {
        public required string ModalId { get; set; }
        public string ModalTitle { get; set; } = "Leave changes";
        public string ModalMessage { get; set; } = "Are you sure you would like to abandon your changes?";
    }
}