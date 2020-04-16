namespace AzureAD.B2C.BuildTask.Commons
{
    public class Error
    {
        public Error()
        {
            AnyError = false;
            ErrorMessage = string.Empty;
        }
        public string ErrorMessage { get; set; }
        public bool AnyError { get; set; }
    }
}
