using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.Attributes;

public class DateRangeOffsetAttribute : ValidationAttribute
{
    private readonly int _minOffset;
    private readonly TimeUnit _minUnit;
    private readonly int _maxOffset;
    private readonly TimeUnit _maxUnit;

    public DateRangeOffsetAttribute(int minOffset, TimeUnit minUnit, int maxOffset, TimeUnit maxUnit)
    {
        _minOffset = minOffset;
        _minUnit = minUnit;
        _maxOffset = maxOffset;
        _maxUnit = maxUnit;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is DateTime date)
        {
            var minDate = _minUnit switch
            {
                TimeUnit.Day => DateTime.UtcNow.AddDays(_minOffset),
                TimeUnit.Month => DateTime.UtcNow.AddMonths(_minOffset),
                TimeUnit.Year => DateTime.UtcNow.AddYears(_minOffset),
                _ => throw new ArgumentOutOfRangeException()
            };

            var maxDate = _maxUnit switch
            {
                TimeUnit.Day => DateTime.UtcNow.AddDays(_maxOffset),
                TimeUnit.Month => DateTime.UtcNow.AddMonths(_maxOffset),
                TimeUnit.Year => DateTime.UtcNow.AddYears(_maxOffset),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (date < minDate)
            {
                return new ValidationResult($"Tarih en erken {minDate:dd/MM/yyyy} olabilir.");
            }

            if (date > maxDate)
            {
                return new ValidationResult($"Tarih en geç {maxDate:dd/MM/yyyy} olabilir.");
            }
        }

        return ValidationResult.Success;
    }
} 