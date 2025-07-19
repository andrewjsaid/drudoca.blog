using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store;

internal class MarkdownFileHelper(MarkdownFile file, ILogger logger)
{
    public bool IsValid { get; private set; } = true;

    public string? GetOptionalString(string header)
    {
        file.Headers.TryGetValue(header, out var result);
        return result;
    }

    public string GetRequiredString(string header)
    {
        var result = GetOptionalString(header);
        if (result != null)
            return result;

        logger.LogWarning("File {file-name} has missing header {header}", file.Name, header);
        IsValid = false;
        return string.Empty;
    }

    public DateTime GetRequiredDate(string header)
    {
        var dateString = GetRequiredString(header);
        if (string.IsNullOrEmpty(dateString))
            return default;

        if (DateTime.TryParse(dateString, out var result))
            return result;

        logger.LogWarning("File {file-name} has invalid date {header}", file.Name, header);
        IsValid = false;
        return default;
    }

    public bool? GetOptionalBoolean(string header)
    {
        var boolString = GetOptionalString(header);
        if (string.IsNullOrEmpty(boolString))
            return null;

        if (bool.TryParse(boolString, out var result))
            return result;

        logger.LogWarning("File {file-name} has invalid bool {header}", file.Name, header);
        IsValid = false;
        return default;
    }

    public bool GetRequiredBoolean(string header)
    {
        var boolString = GetRequiredString(header);
        if (string.IsNullOrEmpty(boolString))
            return default;

        if (bool.TryParse(boolString, out var result))
            return result;

        logger.LogWarning("File {file-name} has invalid bool {header}", file.Name, header);
        IsValid = false;
        return default;
    }

    public int? GetOptionalInt32(string header)
    {
        var intString = GetRequiredString(header);
        if (string.IsNullOrEmpty(intString))
            return null;

        if (int.TryParse(intString, out var result))
            return result;

        logger.LogWarning("File {file-name} has invalid int {header}", file.Name, header);
        IsValid = false;
        return default;
    }

    public int GetRequiredInt32(string header)
    {
        var intString = GetRequiredString(header);
        if (string.IsNullOrEmpty(intString))
            return default;

        if (int.TryParse(intString, out var result))
            return result;

        logger.LogWarning("File {file-name} has invalid int {header}", file.Name, header);
        IsValid = false;
        return default;
    }

    public string[] GetOptionalStringList(string header)
    {
        var text = GetOptionalString(header);
        if (string.IsNullOrEmpty(text))
            return Array.Empty<string>();

        var split = text.Split(';', StringSplitOptions.RemoveEmptyEntries);
        split = Array.ConvertAll(split, s => s.Trim());
        return split;
    }

}