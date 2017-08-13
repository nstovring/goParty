namespace goParty.Helpers
{
    public static class Locations
    {
        //#if DEBUG
        //        public static readonly string AppServiceUrl = "http://localhost:17568/";
        //        public static readonly string AlternateLoginHost = "https://the-book.azurewebsites.net";
        //#else
        public static readonly string AppServiceUrl = "http://gopartyc3799bc758f544faa52787e94a24730d.azurewebsites.net";
        public static readonly string AlternateLoginHost = null;
        //#endif

        public static readonly string AadClientId = "b61c7d68-2086-43a1-a8c9-d93c5732cc84";

        public static readonly string AadRedirectUri = "http://gopartyc3799bc758f544faa52787e94a24730d.azurewebsites.net/.auth/login/done";

        public static readonly string AadAuthority = "https://login.windows.net/photoadrianoutlook.onmicrosoft.com";

        public static readonly string CommonAuthority = "https://login.windows.net/common";
    }
}
