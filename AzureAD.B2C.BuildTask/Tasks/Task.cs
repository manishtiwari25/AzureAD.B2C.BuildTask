namespace AzureAD.B2C.BuildTask.Tasks
{
    public abstract class Task
    {
        public abstract bool ValidateArguments(string[] arguments);
    }
}
