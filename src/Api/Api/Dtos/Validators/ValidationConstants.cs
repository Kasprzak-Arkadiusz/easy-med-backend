namespace Api.Dtos.Validators;

public static class ValidationConstants
{
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 64;
    public const int MinEmailAddressLength = 3;
    public const int MaxEmailAddressLength = 50;
    public const int MaxFirstNameLength = 40;
    public const int MaxLastNameLength = 40;
    public const int MaxTelephoneNumberLength = 9;
    public const int MaxDoctorDescriptionLength = 200;
    public const int MaxOfficeLocationLength = 109;
    public const int MinDescriptionLength = 6;
    public const int MaxDescriptionLength = 400;
    public const int ExactPersonalIdentityNumberLength = 11;
}
