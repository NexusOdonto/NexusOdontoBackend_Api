using System.Text.RegularExpressions;

namespace Application.Services.People;

public static class EmployeeCodeGenerator
{
    private static readonly Regex EmployeeCodeRegex = new(@"^[A-Z]{2,3}\d{3}$", RegexOptions.Compiled);

    private static readonly Dictionary<string, string> PrefixByJobTitleCode =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["ADMINISTRADOR"] = "AD",
            ["RECEPCIONISTA"] = "RC",
            ["ODONTOLOGO"] = "OD",
            ["ASISTENTE"] = "BOT"
        };

    public static bool LooksLikeEmployeeCode(string loginId)
    {
        return !string.IsNullOrWhiteSpace(loginId) &&
               EmployeeCodeRegex.IsMatch(loginId.Trim().ToUpperInvariant());
    }

    public static bool IsValidEmployeeCodeFormat(string employeeCode)
    {
        return LooksLikeEmployeeCode(employeeCode);
    }

    public static string NormalizeEmployeeCode(string employeeCode)
    {
        return employeeCode.Trim().ToUpperInvariant();
    }

    public static string? GetPrefixForJobTitle(string jobTitleCode)
    {
        return PrefixByJobTitleCode.TryGetValue(jobTitleCode.Trim(), out var prefix)
            ? prefix
            : null;
    }

    public static string FormatCode(string prefix, int sequence)
    {
        return $"{prefix}{sequence:D3}";
    }
}
