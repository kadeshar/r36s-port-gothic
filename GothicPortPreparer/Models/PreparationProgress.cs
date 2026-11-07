namespace GothicPortPreparer.Models
{
    public class PreparationProgress
    {
        public int Percentage { get; set; }
        public string StepTitle { get; set; } = string.Empty;
        public string LogMessage { get; set; } = string.Empty;
        public bool IsError { get; set; }

        public PreparationProgress(int percentage, string stepTitle, string logMessage = "", bool isError = false)
        {
            Percentage = percentage;
            StepTitle = stepTitle;
            LogMessage = logMessage;
            IsError = isError;
        }
    }
}
