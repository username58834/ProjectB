public class NegativeSalaryException : Exception
{
    private const string DefaultMessage = "Salary cannot be lesser than zero";
    public NegativeSalaryException(string message = DefaultMessage) : base(message)
    {
    }
}