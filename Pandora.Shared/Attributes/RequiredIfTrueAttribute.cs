using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.Attributes;

public class RequiredIfTrueAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;

    public RequiredIfTrueAttribute(string dependentProperty)
    {
        _dependentProperty = dependentProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (property == null)
            throw new ArgumentException("Property not found", _dependentProperty);

        var dependentValue = property.GetValue(validationContext.ObjectInstance);
        if (dependentValue is bool boolValue && boolValue && value == null)
        {
            return new ValidationResult($"{validationContext.DisplayName} field is required.");
        }

        return ValidationResult.Success;
    }
}

public enum TimeUnit { Day, Month, Year }