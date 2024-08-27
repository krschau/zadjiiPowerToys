//using System.IO.Abstractions;

namespace AllApps.Programs;

/// <summary>
/// Contains user added folder location contents as well as all user disabled applications
/// </summary>
/// <remarks>
/// <para>Win32 class applications set UniqueIdentifier using their full file path</para>
/// <para>UWP class applications set UniqueIdentifier using their Application User Model ID</para>
/// <para>Custom user added program sources set UniqueIdentifier using their location</para>
/// </remarks>
public class ProgramSource
{
    //private static readonly IFileSystem FileSystem = new FileSystem();

    private string name;

    public string Location { get; set; }

    public string Name { get => name ?? "" /*FileSystem.DirectoryInfo.FromDirectoryName(Location).Name*/; set => name = value; }

    public bool Enabled { get; set; } = true;

    public string UniqueIdentifier { get; set; }
}