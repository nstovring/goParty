using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using goParty.Models.APIModels;

namespace goParty.Abstractions
{
    public interface IStripeProvider
    {
        Task<StripeCard> RetreiveCreditCard(string customerId);
        Task<StripeResponse> CreateCreditCard(StripeTempToken token);
        Task<StripeResponse> UpdateCreditCard();
        void DeleteCreditCard();


        Task<StripeBankAccount> RetreiveBankAccount(string customerId);
        Task<StripeResponse> CreateBankAccount(StripeBankAccount token);
        Task<StripeResponse> UpdateBankAccount();
        void DeleteBankAccount();


        Task<StripeResponse> ChargeCustomer(StripeCharge charge);
        Task<StripeResponse> PayCustomer(StripeCharge pay);
    }
}
