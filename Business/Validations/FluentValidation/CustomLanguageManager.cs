using FluentValidation.Resources;

namespace Business.Validations.FluentValidation
{
    public class CustomLanguageManager : LanguageManager
    {
        public CustomLanguageManager()
        {
            AddTranslation("az", "FirstnameIsRequired", "Ad boş ola bilməz!");
            AddTranslation("ru-RU", "FirstnameIsRequired", "Имя не может быть пустым!");
            AddTranslation("en-US", "FirstnameIsRequired", "First name can't be empty!");

            AddTranslation("az", "LastnameIsRequired", "Soyad boş ola bilməz!");
            AddTranslation("ru-RU", "LastnameIsRequired", "Фамилия не может быть пустой!");
            AddTranslation("en-US", "LastnameIsRequired", "Last name can't be empty!");

            AddTranslation("az", "UsernameInvalid", "İstifadəçi adı yalnız hərflər və rəqəmlərdən ibarət olmalıdır!");
            AddTranslation("ru-RU", "UsernameInvalid", "Имя пользователя должно содержать только буквы и цифры!");
            AddTranslation("en-US", "UsernameInvalid", "Username must consist of only letters and numbers!");

            AddTranslation("az", "EmailIsRequired", "Email boş ola bilməz!");
            AddTranslation("ru-RU", "EmailIsRequired", "Email не может быть пустым!");
            AddTranslation("en-US", "EmailIsRequired", "Email can't be empty!");

            AddTranslation("az", "InvalidEmailFormat", "Email formatı düzgün deyil!");
            AddTranslation("ru-RU", "InvalidEmailFormat", "Неверный формат электронной почты!");
            AddTranslation("en-US", "InvalidEmailFormat", "Invalid email format!");

            AddTranslation("az", "PasswordIsRequired", "Şifrə boş ola bilməz!");
            AddTranslation("ru-RU", "PasswordIsRequired", "Пароль не может быть пустым!");
            AddTranslation("en-US", "PasswordIsRequired", "Password can't be empty!");

            AddTranslation("az", "PasswordMinLength", "Şifrə ən azı 8 simvol olmalıdır!");
            AddTranslation("ru-RU", "PasswordMinLength", "Пароль должен содержать не менее 8 символов!");
            AddTranslation("en-US", "PasswordMinLength", "Password must be at least 8 characters long!");

            AddTranslation("az", "PasswordMustContainUppercase", "Şifrə böyük hərf olmalıdır!");
            AddTranslation("ru-RU", "PasswordMustContainUppercase", "Пароль должен содержать заглавную букву!");
            AddTranslation("en-US", "PasswordMustContainUppercase", "Password must contain an uppercase letter!");

            AddTranslation("az", "PasswordMustContainLowercase", "Şifrə kiçik hərf olmalıdır!");
            AddTranslation("ru-RU", "PasswordMustContainLowercase", "Пароль должен содержать строчную букву!");
            AddTranslation("en-US", "PasswordMustContainLowercase", "Password must contain a lowercase letter!");

            AddTranslation("az", "PasswordMustContainDigit", "Şifrədə rəqəm olmalıdır!");
            AddTranslation("ru-RU", "PasswordMustContainDigit", "Пароль должен содержать цифру!");
            AddTranslation("en-US", "PasswordMustContainDigit", "Password must contain a number!");

            AddTranslation("az", "ConfirmPasswordIsRequired", "Təsdiq şifrəsi boş ola bilməz!");
            AddTranslation("ru-RU", "ConfirmPasswordIsRequired", "Подтверждение пароля не может быть пустым!");
            AddTranslation("en-US", "ConfirmPasswordIsRequired", "Confirm password can't be empty!");

            AddTranslation("az", "PasswordsDoNotMatch", "Şifrələr uyğun gəlmir!");
            AddTranslation("ru-RU", "PasswordsDoNotMatch", "Пароли не совпадают!");
            AddTranslation("en-US", "PasswordsDoNotMatch", "Passwords do not match!");

            AddTranslation("az", "UserNotFound", "Istifadəçi tapılmadı  !");
            AddTranslation("ru-RU", "UserNotFound", "Пользователь не найден!");
            AddTranslation("en-US", "UserNotFound", "User not found!");
        }
    }
}
