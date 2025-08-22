namespace STC.ProductCatalog.WebAPI;

public static class AuthConstants
{
    public static class Roles
    {
        public const string Admin = "ADMIN";
        public const string Moderator = "MODERATOR";
    }

    public static class Policies
    {
        public const string CanUpdateProductPolicyName = "CanUpdateProductPolicy";
    }
}