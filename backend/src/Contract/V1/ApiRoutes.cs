namespace OpenSchool.src.Contract.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Auths
        {
            public const string signUp = Base + "/signUp";
            public const string signIn = Base + "/signIn";
        }
        public static class Tests
        {
            public const string tst401 = Base + "/tst401";
            public const string tst403 = Base + "/tst403";
        }
        public static class Blog
        {
            public const string getAllArticles  = Base + "/getArticles";
            public const string getArticleById = Base + "/getArticle/{id}";
            public const string addArticle     = Base + "/addArticle";
            public const string removeArticle  = Base + "/removeArticle/{id}";
            public const string updateArticle = Base + "/updateArticle";
        }
    }
}
