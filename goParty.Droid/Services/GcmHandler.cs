﻿using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
namespace goParty.Droid.Services
{
    [BroadcastReceiver(Permission = Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class GcmHandler : GcmBroadcastReceiverBase<GcmService>
    {
        // Replace with your Sender ID from the Firebase Console
        public static string[] SenderId = new string[] { "405846641962" };

    }

    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService() : base(GcmHandler.SenderId)
        {

        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info("GcmService", $"Message {intent.ToString()}");
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error("GcmService", $"ERROR: {errorId}");
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Info("GcmService", $"Registered: {registrationId}");
            GcmService.RegistrationID = registrationId;
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Info("GcmService", $"Unregistered device from FCM");
            GcmService.RegistrationID = null;
        }
    }
}