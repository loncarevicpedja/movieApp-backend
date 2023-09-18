using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class InRangeAttribute : ValidationAttribute
{
    private readonly int _minValue;
    private readonly int _maxValue;

    public InRangeAttribute(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int intValue)
        {
            if (intValue < _minValue || intValue > _maxValue)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
