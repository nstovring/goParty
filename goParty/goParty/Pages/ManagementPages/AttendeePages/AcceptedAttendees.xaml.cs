﻿using goParty.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace goParty.Pages.ManagementPages.AttendeePages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AcceptedAttendees : ContentPage , IRefreshable
	{
		public AcceptedAttendees ()
		{
			InitializeComponent ();
		}

        public Task Refresh()
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}