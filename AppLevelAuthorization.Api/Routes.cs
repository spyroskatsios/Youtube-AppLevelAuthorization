using Microsoft.AspNetCore.Mvc;

namespace AppLevelAuthorization.Api;

public static class Routes
{
    private const string Base = "api";

    public static class Auth
    {
        public const string SignIn = Base + "/auth/signin";
        public const string SignUp = Base + "/auth/signup";
        public const string AddUserToRole = Base + "/auth/addusertorole";
    }

    public static class Messages
    {
        public const string User = Base + "/message/user";
        public const string Supervisor = Base + "/message/supervisor";
        public const string Manager = Base + "/message/manager";
        public const string Admin = Base + "/message/admin";
    }
}