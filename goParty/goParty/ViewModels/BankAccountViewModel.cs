using goParty.Abstractions;
using goParty.Helpers;
using goParty.Models.APIModels;
using goParty.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace goParty.ViewModels
{
    public class BankAccountViewModel: BaseViewModel
    {
        Command saveBankDetailscmd;
        public Command SaveBankDetailsCommand => saveBankDetailscmd ?? (saveBankDetailscmd = new Command(async () => await ExecuteSaveBankDetailsCommand().ConfigureAwait(false)));

        private async Task ExecuteSaveBankDetailsCommand()
        {
            //Get account details
            string id;
            var loginProvider = DependencyService.Get<ILoginProvider>();
            Account acc = loginProvider.RetreiveAccountFromSecureStore();
            acc.Properties.TryGetValue("stripe_account_id", out id);

            var stripeService = (StripeService)ServiceLocator.Instance.Resolve<IStripeProvider>();

            StripeBankAccount stripeBankAccount = new StripeBankAccount()
            {
                account_holder_name = _accountHolderName,
                account_number = _accountNumber,
                currency = _currency,
                country = _country,
                customerId = id
            };

            StripeResponse response = await stripeService.CreateBankAccount(stripeBankAccount);
        }

        Command getBankDetailscmd;
        public Command GetBankDetailsCommand => getBankDetailscmd ?? (getBankDetailscmd = new Command(async () => await ExecuteGetBankDetailsCommand().ConfigureAwait(false)));

        private async Task ExecuteGetBankDetailsCommand()
        {
            string id;
            var loginProvider = DependencyService.Get<ILoginProvider>();
            Account acc = loginProvider.RetreiveAccountFromSecureStore();
            acc.Properties.TryGetValue("stripe_account_id", out id);

            var stripeService = (StripeService)ServiceLocator.Instance.Resolve<IStripeProvider>();

            StripeBankAccount bankAccount = await stripeService.RetreiveBankAccount(id);

            AccountHolderName = bankAccount.account_holder_name;
            AccountNumber = bankAccount.account_number;
            Currency = bankAccount.currency;
            Country = bankAccount.country;

        }


        public BankAccountViewModel()
        {

        }


        string _accountNumber;
        string _country;
        string _currency;
        string _accountHolderName;

        public string AccountNumber
        {
            get { return _accountNumber; }
            set { SetProperty(ref _accountNumber, value, "AccountNumber"); }
        }

        public string Country
        {
            get { return _country; }
            set { SetProperty(ref _country, value, "Country"); }
        }

        public string Currency
        {
            get { return _currency; }
            set { SetProperty(ref _currency, value, "Currency"); }
        }

        public string AccountHolderName
        {
            get { return _accountHolderName; }
            set { SetProperty(ref _accountHolderName, value, "AccountHolderName"); }
        }
    }
}
