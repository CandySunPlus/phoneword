using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword_Droid
{
	[Activity(Label = "Phoneword", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		static readonly List<String> phoneNumbers = new List<String>();

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
			Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
			Button callButton = FindViewById<Button>(Resource.Id.CallButton);
			Button callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

			callButton.Enabled = false;

			string translatedNumber = string.Empty;

			translateButton.Click += (object sender, EventArgs e) => {
				translatedNumber = Phoneword.PhoneTranslator.ToNumber(phoneNumberText.Text);

				if (String.IsNullOrWhiteSpace(translatedNumber)) {
					callButton.Text = "Call";
					callButton.Enabled = false;
				} else {
					callButton.Text = "Call " + translatedNumber;
					callButton.Enabled = true;
				}
			};

			callButton.Click += (object sender, EventArgs e) => {
				phoneNumbers.Add(translatedNumber);

				var callDialog = new AlertDialog.Builder(this);
				callDialog.SetTitle ("Call");
				callDialog.SetMessage("Call " + translatedNumber + "?");
				callDialog.SetNeutralButton("Call", delegate {
					var callIntent = new Intent(Intent.ActionCall);
					callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
					StartActivity(callIntent);
				});

				callDialog.SetNegativeButton("Cancel", delegate {
				});

				callDialog.Show();
			};

			callHistoryButton.Click += (object sender, EventArgs e) => {
				var intent = new Intent (this, typeof(CallHistoryActivity));
				intent.PutStringArrayListExtra ("phone_numbers", phoneNumbers);
				StartActivity (intent);
			};
		}
	}
}


