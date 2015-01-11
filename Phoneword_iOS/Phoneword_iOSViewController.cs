using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoneword_iOS
{
	public partial class Phoneword_iOSViewController : UIViewController
	{

		public List<String> PhoneNumbers { get; set; }

		public Phoneword_iOSViewController(IntPtr handle)
			: base(handle)
		{
			PhoneNumbers = new List<String>();
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			// Perform any additional setup after loading the view, typically from a nib.
			string translatedNumber = "";

			TranslateButton.TouchUpInside += (object sender, EventArgs e) => {
				translatedNumber = Phoneword.PhoneTranslator.ToNumber(PhoneNumberText.Text);
				PhoneNumberText.ResignFirstResponder();

				if (translatedNumber == "") {
					CallButton.SetTitle("Call", UIControlState.Normal);
					CallButton.Enabled = false;
				} else {
					CallButton.SetTitle("Call " + translatedNumber, UIControlState.Normal);
					CallButton.Enabled = true;
				}
			};

			CallButton.TouchUpInside += (object sender, EventArgs e) => {

				PhoneNumbers.Add(translatedNumber);

				var url = new NSUrl("tel:" + translatedNumber);


				var confirmDialog = new UIAlertView("Call", "Call " + translatedNumber + "?", null, "Cancel", new string[] {"Call"});

				confirmDialog.Clicked += (object sd, UIButtonEventArgs es) => {
					if (es.ButtonIndex == 1) {
						if (!UIApplication.SharedApplication.OpenUrl(url)) {
							var av = new UIAlertView("Not supported",
								        "Scheme 'tel:' is not supported on this device", null, "OK", null);
							av.Show();
						}
					}
				};

				confirmDialog.Show();

			};

			CallHistoryButton.TouchUpInside += (object sender, EventArgs e) => {
				CallHistoryController callHistory = this.Storyboard.InstantiateViewController("CallHistoryController") as CallHistoryController;
				if (callHistory != null) {
					callHistory.PhoneNumbers = PhoneNumbers;
				}
				this.NavigationController.PushViewController(callHistory, true);
			};
		}

		//		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		//		{
		//			base.PrepareForSegue (segue, sender);
		//
		//			var callHistoryController = segue.DestinationViewController as CallHistoryController;
		//
		//			if (callHistoryController != null) {
		//				callHistoryController.PhoneNumbers = PhoneNumbers;
		//			}
		//		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
		}

		#endregion
	}
}

