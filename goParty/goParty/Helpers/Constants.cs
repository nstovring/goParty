using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Helpers
{
    public static class Constants
    {

        public static readonly string FacebookClientId = "838382242982955";
        public static readonly string FacebookRedirectUri = "https://gopartyc3799bc758f544faa52787e94a24730d.azurewebsites.net/.auth/login/facebook/callback";

        public static readonly string FacebookRequestNameURI = "https://graph.facebook.com/me";
        public static readonly string FacebookRequestScope = "public_profile user_friends";

        public static readonly string SearchServiceName = "35c50807-dd2d-4fd3-996f-8c4e8182868e";
        public static readonly string SearchAdminApiKey = "6841CB0D756DC0A63B82D3AD141FBB8B";

        public static readonly string userIDIndex = "userdetails-userid";
        public static readonly string attendeePartyIDIndex = "attendeedetails-partyid";
        public static readonly string attendeeUserIDIndex = "attendeedetails-userid";


        public enum JoinedPartyStates { JoinParty, RequestSent, EventJoined, CancelEvent };
        public static readonly string[] joinButtonTitles = { "Join Event", "Request Sent", "Event Joined", "Cancel Event" };

        public static readonly string wherePlaceholder = "Insert Adresse Here";
        public static readonly string picturePlaceholder = "party.jpg";
        public static readonly string titlePlaceholder = "Insert Title Here";
        public static readonly string descriptionPlaceholder = "Insert Description Here";

        public static readonly string stripeTestKey = "pk_test_iiM8BYGCcNk6AbWkx7joIQKv";
        public static readonly string stripeAccountIdPropertyName = "stripe_account_id";

        #region Page Names
        public static string profilePageTitle = "My Page";
        public static string locatePartyPageTitle = "Locate Party";
        public static string managePartyPageTitle = "Manage & Create";
        public static string PartyCarouselViewModelTitle = "Parties";
        public static string CreatePartyPageTitle = "Parties";

        #endregion
    }
}
