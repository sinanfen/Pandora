using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.Attributes;

public class MinimumDateOffsetAttribute : ValidationAttribute
{
    private readonly int _offset;
    private readonly TimeUnit _unit;

    public MinimumDateOffsetAttribute(int offset, TimeUnit unit)
    {
        _offset = offset;
        _unit = unit;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is DateTime date)
        {
            var minDate = _unit switch
            {
                TimeUnit.Day => DateTime.UtcNow.AddDays(_offset),
                TimeUnit.Month => DateTime.UtcNow.AddMonths(_offset),
                TimeUnit.Year => DateTime.UtcNow.AddYears(_offset),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (date < minDate)
            {
                return new ValidationResult($"Tarih en erken {minDate:dd/MM/yyyy} olabilir.");
            }
        }

        return ValidationResult.Success;
    }
}
