using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace goParty.Models.APIModels
{
    public class StripeCard
    {
        public StripeCard()
        {

        }

        public StripeCard(Card stripeCard)
        {
            Number = stripeCard.Number;
            ExpiryYear = stripeCard.ExpiryYear;
            ExpiryMonth = stripeCard.ExpiryMonth;
            CVC = stripeCard.CVC;
        }

        public string Number { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public string CVC { get; set; }
        public string Id { get; set; }
        public string AddressState { get; set; }
    }
}
