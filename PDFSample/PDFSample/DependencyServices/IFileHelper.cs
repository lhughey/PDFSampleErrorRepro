using System;
namespace Connect.DependencyServices
{
    public interface IFileHelper
    {
        string DocumentFilePath { get; }

        string ResourcesBaseUrl { get; }
    }
}
