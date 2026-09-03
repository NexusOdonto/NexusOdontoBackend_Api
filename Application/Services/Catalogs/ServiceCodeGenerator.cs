using System.Globalization;
using System.Text;

namespace Application.Services.Catalogs;

public static class ServiceCodeGenerator
{
    public const int MaxCodeLength = 6;
    private const int SuffixLength = 2;

    public static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }

    public static string FromName(string name)
    {
        var words = name.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var initials = new StringBuilder();

        foreach (var word in words)
        {
            foreach (var c in RemoveAccents(word))
            {
                if (!char.IsLetter(c))
                {
                    continue;
                }

                initials.Append(char.ToUpperInvariant(c));
                break;
            }
        }

        if (initials.Length == 0)
        {
            throw new InvalidOperationException("Cannot generate a service code from the provided name.");
        }

        var acronym = initials.ToString();
        return acronym.Length <= MaxCodeLength
            ? acronym
            : acronym[..MaxCodeLength];
    }

    public static string GetCollisionPrefix(string baseAcronym)
    {
        var maxPrefixLength = MaxCodeLength - SuffixLength;
        return baseAcronym.Length <= maxPrefixLength
            ? baseAcronym
            : baseAcronym[..maxPrefixLength];
    }

    public static string FormatWithSuffix(string prefix, int sequence)
    {
        return $"{prefix}{sequence:D2}";
    }

    private static string RemoveAccents(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
