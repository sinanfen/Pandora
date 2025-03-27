using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.Attributes;

public class RequiredIfTrueAttribute : ValidationAttribute
{
    private readonly string _propertyName;

    public RequiredIfTrueAttribute(string propertyName)
    {
        _propertyName = propertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var instance = context.ObjectInstance;
        var type = instance.GetType();
        var boolValue = (bool)type.GetProperty(_propertyName).GetValue(instance);

        if (boolValue && value == null)
        {
            return new ValidationResult($"{context.DisplayName} alanı zorunludur.");
        }

        return ValidationResult.Success;
    }
}

public enum TimeUnit { Day, Month, Year }