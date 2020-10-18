using System.Globalization;

namespace GoodJobGames.Utilities.Constants
{
    public class AppConstants
    {
        public const string SolutionName = "GoodJobGames";
        public const string DataStorageSection = "DataStorageSection";
        public const string AcceptedLanguageHeaderKey = "Accept-Language";
        public static CultureInfo DefaultCultureInfo = new CultureInfo("en-US");
        public const string AppCenterTokenHeaderKey = "X-API-Token";
    }
}