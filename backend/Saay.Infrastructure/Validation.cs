namespace Saay.Infrastructure
{
    public static class Validation
    {
        public const string PASSWORD_REGX = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z\d\s]).{8,}$";
        public const string EMAIL_REGX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    }
}
