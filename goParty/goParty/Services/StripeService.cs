using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using goParty.Models.APIModels;
using System.Threading.Tasks;
using Stripe;
using Newtonsoft.Json;
using System.Net.Http;
using goParty.Helpers;
using Xamarin.Forms;

namespace goParty.Services
{
    class StripeService : IStripeProvider
    {
        public AzureCloudService cloudService;
        public StripeService()
        {
            StripeClient.DefaultPublishableKey = Constants.stripeTestKey;
            cloudService = (AzureCloudService) ServiceLocator.Instance.Resolve<ICloudService>();
        }
        static StripeService defaultInstance = new StripeService();

        public static StripeService DefaultService
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public async Task<StripeResponse> ChargeCustomer(StripeCharge charge)
        {
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("customerId", customerId);

            StripeResponse temp = await cloudService.client.InvokeApiAsync<StripeCharge, StripeResponse>("StripeCharge", charge);

            StripeCard card = JsonConvert.DeserializeObject<StripeCard>(temp.ObjectJson);
            return temp;
        }

        public async Task<StripeResponse> CreateBankAccount(StripeBankAccount token)
        {
            try
            {
                StripeResponse temp = await cloudService.client.InvokeApiAsync<StripeBankAccount, StripeResponse>("StripeBank", token);
                return temp;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Stripe Failure", ex.Message, "OK");
            }
            return null;
        }

        public async Task<StripeResponse> CreateCreditCard(StripeTempToken token)
        {
            string JToken = JsonConvert.SerializeObject(token);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("posttoken", JToken);
            try
            {
                StripeResponse temp = await cloudService.client.InvokeApiAsync<string, StripeResponse>("Stripe", JToken, HttpMethod.Post, parameters);
                return temp;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Stripe Failure", ex.Message, "OK");
            }
            return null;
        }

        public void DeleteBankAccount()
        {
            throw new NotImplementedException();
        }

        public void DeleteCreditCard()
        {
            throw new NotImplementedException();
        }

        public Task<StripeResponse> PayCustomer(StripeCharge pay)
        {
            throw new NotImplementedException();
        }

        public async Task<StripeBankAccount> RetreiveBankAccount(string customerId)
        {
            //string JToken = JsonConvert.SerializeObject(token);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("customerId", customerId);
            try
            {
                StripeResponse temp = await cloudService.client.InvokeApiAsync<StripeResponse>("StripeBank", HttpMethod.Get, parameters);
                StripeBankAccount bankAccount = JsonConvert.DeserializeObject<StripeBankAccount>(temp.ObjectJson);
                return bankAccount;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Stripe Failure", ex.Message, "OK");
            }
            return null;
        }

        public async Task<StripeCard> RetreiveCreditCard(string customerId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("customerId", customerId);

            StripeResponse temp = await cloudService.client.InvokeApiAsync<StripeResponse>("Stripe", HttpMethod.Get, parameters);

            StripeCard card = JsonConvert.DeserializeObject<StripeCard>(temp.ObjectJson);
            return card;
        }

        public Task<StripeResponse> UpdateBankAccount()
        {
            throw new NotImplementedException();
        }

        public Task<StripeResponse> UpdateCreditCard()
        {
            throw new NotImplementedException();
        }
    }
}
