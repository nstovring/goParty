namespace goParty.Helpers
{
    public static class Locations
    {
        //#if DEBUG
        //        public static readonly string AppServiceUrl = "http://localhost:17568/";
        //        public static readonly string AlternateLoginHost = "https://the-book.azurewebsites.net";
        //#else
        public static readonly string AppServiceUrl = "https://gopartyc3799bc758f544faa52787e94a24730d.azurewebsites.net";
        public static readonly string AlternateLoginHost = null;
        //#endif

        public static readonly string AadClientId = "b61c7d68-2086-43a1-a8c9-d93c5732cc84";


        public static readonly string AadAuthority = "https://login.windows.net/photoadrianoutlook.onmicrosoft.com";

        public static readonly string CommonAuthority = "https://login.windows.net/common";



        public static readonly string AppName = "PartyCrash2";
        public static readonly string AzureUrl = "http://partycrash.azurewebsites.net";

        public static readonly string FacebookClientId = "838382242982955";
        public static readonly string FacebookRedirectUri = "https://gopartyc3799bc758f544faa52787e94a24730d.azurewebsites.net/.auth/login/facebook/callback";

        public static readonly string FacebookRequestNameURI = "https://graph.facebook.com/me";

        public const string StorageConnection = "DefaultEndpointsProtocol=https;AccountName=partycrasherblob;AccountKey=WaDHg28g9iKacJXo/PZ6I2MCoSYfbITrRDspe/Cp5z9jp3AI7hGxodxytaRmYzjSH9nX69jPx1WzLmWYs7a1dw==;EndpointSuffix=core.windows.net";

        public static string FacebookRequestUserInfoUrl = "https://graph.facebook.com/v2.10/me/?fields=name,age_range,picture&width=800&height=800&redirect=false";


        public static string LocalDataBase = "LocalPartyCrasher.db";


        public static readonly string SearchServiceName = "partycrasher";
        public static readonly string SearchAdminApiKey = "7894A1D346740E86E50346E7BF657376";

        public static readonly string userIDIndex = "useridindex";
        public static readonly string partyIDIndex = "partyidindex";

        public static readonly string PartyPlaceholderImage = "party.jpg";
        public static readonly string placeholderEventTitle = "Insert Event Title";
        public static readonly string placeholderEventDescription = "Insert Event Description";
        public static readonly string placeholderSearchForLocationText = "Search for Event Location";
    }
}
