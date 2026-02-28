using ScanFolderToFile.Core.Abstractions;

namespace ScanFolderToFile.Core.Services;

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}
