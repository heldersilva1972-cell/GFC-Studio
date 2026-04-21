namespace GFC.Core.Interfaces;

public interface IVersionService
{
    string GetDeveloper();
    string GetYear();
    string GetRevision();
    string GetFullVersion();
}

