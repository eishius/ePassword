using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ePassword.Api
{
    public class Password : IPassword
    {
        private const int DefaultPasswordLength = 16;
        private const int DefaultMaxPasswordAttempts = 10000;
        private const bool DefaultIncludeLowercase = true;
        private const bool DefaultIncludeUppercase = true;
        private const bool DefaultIncludeNumeric = true;
        private const bool DefaultIncludeSpecial = true;
        private static RandomNumberGenerator _rng;

        public Password()
        {
            Settings = new PasswordSettings(DefaultIncludeLowercase, DefaultIncludeUppercase,
                DefaultIncludeNumeric, DefaultIncludeSpecial, DefaultPasswordLength, DefaultMaxPasswordAttempts,
                true);

            _rng = RandomNumberGenerator.Create();
        }

        public Password(IPasswordSettings settings)
        {
            Settings = settings;

            _rng = RandomNumberGenerator.Create();
        }

        public Password(int passwordLength)
        {
            Settings = new PasswordSettings(DefaultIncludeLowercase, DefaultIncludeUppercase,
                DefaultIncludeNumeric, DefaultIncludeSpecial, passwordLength, DefaultMaxPasswordAttempts, true);

            _rng = RandomNumberGenerator.Create();
        }

        public Password(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial)
        {
            Settings = new PasswordSettings(includeLowercase, includeUppercase, includeNumeric,
                includeSpecial, DefaultPasswordLength, DefaultMaxPasswordAttempts, false);

            _rng = RandomNumberGenerator.Create();
        }

        public Password(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial,
            int passwordLength)
        {
            Settings = new PasswordSettings(includeLowercase, includeUppercase, includeNumeric,
                includeSpecial, passwordLength, DefaultMaxPasswordAttempts, false);

            _rng = RandomNumberGenerator.Create();
        }

        public Password(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial,
            int passwordLength, int maximumAttempts)
        {
            Settings = new PasswordSettings(includeLowercase, includeUppercase, includeNumeric,
                includeSpecial, passwordLength, maximumAttempts, false);

            _rng = RandomNumberGenerator.Create();
        }

        public IPasswordSettings Settings { get; set; }

        public IPassword IncludeLowercase()
        {
            Settings = Settings.AddLowercase();
            return this;
        }

        public IPassword IncludeUppercase()
        {
            Settings = Settings.AddUppercase();
            return this;
        }

        public IPassword IncludeNumeric()
        {
            Settings = Settings.AddNumeric();
            return this;
        }

        public IPassword IncludeSpecial()
        {
            Settings = Settings.AddSpecial();
            return this;
        }

        public IPassword IncludeSpecial(string specialCharactersToInclude)
        {
            Settings = Settings.AddSpecial(specialCharactersToInclude);
            return this;
        }

        public IPassword LengthRequired(int passwordLength)
        {
            Settings.PasswordLength = passwordLength;
            return this;
        }

        public string Next()
        {
            string password;
            if (!LengthIsValid(Settings.PasswordLength, Settings.MinimumLength, Settings.MaximumLength))
            {
                password =
                    $"Password length invalid. Must be between {Settings.MinimumLength} and {Settings.MaximumLength} characters long";
            }
            else
            {
                var passwordAttempts = 0;
                do
                {
                    password = GenerateRandomPassword(Settings);
                    passwordAttempts++;
                } while (passwordAttempts < Settings.MaximumAttempts && !PasswordIsValid(Settings, password));

                password = PasswordIsValid(Settings, password) ? password : "Try again";
            }

            return password;
        }


        public IEnumerable<string> NextGroup(int numberOfPasswordsToGenerate)
        {
            var passwords = new List<string>();

            for (var i = 0; i < numberOfPasswordsToGenerate; i++)
            {
                var pwd = this.Next();
                passwords.Add(pwd);
            }

            return passwords;
        }

        private static string GenerateRandomPassword(IPasswordSettings settings)
        {
            const int maximumIdenticalConsecutiveChars = 2;
            var password = new char[settings.PasswordLength];

            var characters = settings.CharacterSet.ToCharArray();
            var shuffledChars = Shuffle(characters.Select(x => x)).ToArray();

            var shuffledCharacterSet = string.Join(null, shuffledChars);
            var characterSetLength = shuffledCharacterSet.Length;

            for (var characterPosition = 0; characterPosition < settings.PasswordLength; characterPosition++)
            {
                password[characterPosition] = shuffledCharacterSet[GetRandomNumberInRange(0, characterSetLength - 1)];

                var moreThanTwoIdenticalInARow =
                    characterPosition > maximumIdenticalConsecutiveChars
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow) characterPosition--;
            }

            return string.Join(null, password);
        }

        private static int GetRandomNumberInRange(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            var data = new byte[sizeof(int)];
            _rng.GetBytes(data);
            var randomNumber = BitConverter.ToInt32(data, 0);

            return (int)Math.Floor((double)(min + Math.Abs(randomNumber % (max - min))));
        }

        private static int GetRngCryptoSeed(RNGCryptoServiceProvider rng)
        {
            var rngByteArray = new byte[4];
            rng.GetBytes(rngByteArray);
            return BitConverter.ToInt32(rngByteArray, 0);
        }

        private static bool PasswordIsValid(IPasswordSettings settings, string password)
        {
            const string regexLowercase = @"[a-z]";
            const string regexUppercase = @"[A-Z]";
            const string regexNumeric = @"[\d]";

            var lowerCaseIsValid = !settings.IncludeLowercase ||
                                   settings.IncludeLowercase && Regex.IsMatch(password, regexLowercase);
            var upperCaseIsValid = !settings.IncludeUppercase ||
                                   settings.IncludeUppercase && Regex.IsMatch(password, regexUppercase);
            var numericIsValid = !settings.IncludeNumeric ||
                                 settings.IncludeNumeric && Regex.IsMatch(password, regexNumeric);

            var specialIsValid = !settings.IncludeSpecial;

            if (settings.IncludeSpecial && !string.IsNullOrWhiteSpace(settings.SpecialCharacters))
            {
                var listA = settings.SpecialCharacters.ToCharArray();
                var listB = password.ToCharArray();

                specialIsValid = listA.Any(x => listB.Contains(x));
            }

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && specialIsValid &&
                   LengthIsValid(password.Length, settings.MinimumLength, settings.MaximumLength);
        }
        private static bool LengthIsValid(int passwordLength, int minLength, int maxLength)
        {
            return passwordLength >= minLength && passwordLength <= maxLength;
        }

        private static IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
        {
            return from item in items orderby Guid.NewGuid() select item;
        }
    }
}
