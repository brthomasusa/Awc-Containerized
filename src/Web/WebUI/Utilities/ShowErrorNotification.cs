using Radzen;

namespace WebUI.Utilities
{
    public static class ShowErrorNotification
    {
        public static void ShowError
        (
            NotificationService notificationService,
            string errorMessage)
        {
            notificationService!.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = errorMessage,
                    Duration = 40000
                }
            );
        }
    }
}