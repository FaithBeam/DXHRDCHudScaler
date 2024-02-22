using System.ComponentModel.DataAnnotations;

namespace DXHRDCHudScaler.Validations;

public class ResolutionValidation : RequiredAttribute
{
    private static bool _visited;

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (!_visited)
        {
            _visited = true;
            return ValidationResult.Success!;
        }

        var str = value as string;
        if (string.IsNullOrWhiteSpace(str))
        {
            return new ValidationResult("Resolution must be a positive number");
        }

        return !uint.TryParse(str, out _)
            ? new ValidationResult("Resolution must be a positive number")
            : ValidationResult.Success!;
    }
}
