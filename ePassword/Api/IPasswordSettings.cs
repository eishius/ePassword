namespace ePassword.Api
{
    public interface IPasswordSettings
    {
        bool IncludeLowercase { get; }
        bool IncludeUppercase { get; }
        bool IncludeNumeric { get; }
        bool IncludeSpecial { get; }
        int PasswordLength { get; set; }
        string CharacterSet { get; }
        int MaximumAttempts { get; }
        int MinimumLength { get; }
        int MaximumLength { get; }
        IPasswordSettings AddLowercase();
        IPasswordSettings AddUppercase();
        IPasswordSettings AddNumeric();
        IPasswordSettings AddSpecial();
        IPasswordSettings AddSpecial(string specialCharactersToAdd);
        string SpecialCharacters { get; set; }
    }
}
