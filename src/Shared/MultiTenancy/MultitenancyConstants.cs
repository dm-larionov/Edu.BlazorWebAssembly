namespace Edu.WebApi.Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }

    public static class EduHead
    {
        public const string Id = "EduHead";
        public const string Name = "EduHead";
        public const string EmailAddress = "head_edu@root.com";
    }

    public static class EduEmployee
    {
        public const string Id = "EduEmployee";
        public const string Name = "EduEmployee";
        public const string EmailAddress = "employee_edu@root.com";
    }

    public static class Teacher
    {
        public const string Id = "Teacher";
        public const string Name = "Teacher";
        public const string EmailAddress = "teacher@root.com";
    }

    public const string DefaultPassword = "123Pa$$word!";
}