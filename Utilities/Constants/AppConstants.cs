using System.Globalization;

namespace Burak.GoodJobGames.Utilities.Constants
{
    public class AppConstants
    {
        public const string SolutionName = "Burak.GoodJobGames";
        public const string DataStorageSection = "DataStorageSection";
        public const string AcceptedLanguageHeaderKey = "Accept-Language";
        public static CultureInfo DefaultCultureInfo = new CultureInfo("en-US");
        public const string AppCenterTokenHeaderKey = "X-API-Token";
        public const string JWTSecretKey = "This is a secret key, it should not be shared with others!";
    }
}