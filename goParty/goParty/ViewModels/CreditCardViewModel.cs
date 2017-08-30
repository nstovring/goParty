using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Stripe;
using Xamarin.Forms;
using System.Threading.Tasks;
using goParty.Services;
using goParty.Helpers;
using goParty.Models.APIModels;
using Xamarin.Auth;
using Newtonsoft.Json;

namespace goParty.ViewModels
{
    public class CreditCardViewModel : BaseViewModel
    {
        string _cardNumber;
        string _expirationDateM;
        string _expirationDateY;
        string _CVC;
        Card card;
        Command saveCardDetailscmd;
        public Command SaveCardDetailsCommand => saveCardDetailscmd ?? (saveCardDetailscmd = new Command(async () => await ExecuteSaveCardDetailsCommand().ConfigureAwait(false)));

        Command getCardDetailscmd;
        public Command GetCardDetailsCommand => getCardDetailscmd ?? (getCardDetailscmd = new Command(async () => await ExecuteGetCardDetailsCommand().ConfigureAwait(false)));

        
        public CreditCardViewModel()
        {
        }

        public async Task ExecuteSaveCardDetailsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            card = new Card
            {
                Number = _cardNumber,
                ExpiryMonth = int.Parse(_expirationDateM),
                ExpiryYear = int.Parse(_expirationDateY),
                CVC = _CVC.ToString()
            };
            try
            {
                var token = await StripeClient.CreateToken(card);

                var stripeService = (StripeService)ServiceLocator.Instance.Resolve<IStripeProvider>();

                StripeTempToken stripeToken = new StripeTempToken(token);
                StripeResponse response = await stripeService.CreateCreditCard(stripeToken);

                var loginProvider = DependencyService.Get<ILoginProvider>();

                Account acc = loginProvider.RetreiveAccountFromSecureStore();
                acc.Properties.Add(Constants.stripeAccountIdPropertyName, response.ObjectJson);
                loginProvider.SaveAccountInSecureStore(acc);

                Console.WriteLine("Response: " + response.RequestDate);
                // Slightly different for non-Apple Pay use, see 
                // 'Sending the token to your server' for more info
                //await CreateBackendCharge(token);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Items Not Loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteGetCardDetailsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                string id;
                var loginProvider = DependencyService.Get<ILoginProvider>();
                Account acc = loginProvider.RetreiveAccountFromSecureStore();
                acc.Properties.TryGetValue(Constants.stripeAccountIdPropertyName, out id);

                var stripeService = (StripeService)ServiceLocator.Instance.Resolve<IStripeProvider>();

                StripeCard response = await stripeService.RetreiveCreditCard(id);

                CardNumber = "XXXX XXXX XXXX " + response.Number;
                CVC = response.CVC;
                ExpirationDateM = response.ExpiryMonth.ToString();
                ExpirationDateY = response.ExpiryYear.ToString();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Items Not Loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        public string CardNumber
        {
            get { return _cardNumber; }
            set { SetProperty(ref _cardNumber, value, "CardNumber"); }
        }

        public string ExpirationDateM
        {
            get { return _expirationDateM; }
            set { SetProperty(ref _expirationDateM, value, "ExpirationDateM"); }
        }

        public string ExpirationDateY
        {
            get { return _expirationDateY; }
            set { SetProperty(ref _expirationDateY, value, "ExpirationDateY"); }
        }

        public string CVC {
            get { return _CVC; }
            set { SetProperty(ref _CVC, value, "CVC"); }
        }
    }
}
