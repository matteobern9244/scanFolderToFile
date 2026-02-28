namespace ScanFolderToFile.Core.Exceptions;

public sealed class InvalidScanRequestException : Exception
{
    public InvalidScanRequestException(string message)
        : base(message)
    {
    }
}

public sealed class OutputGenerationException : Exception
{
    public OutputGenerationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
