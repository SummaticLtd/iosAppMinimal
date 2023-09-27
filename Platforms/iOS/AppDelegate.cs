using Foundation;
using UIKit;
using System;
using FSLibrary;

namespace Summatic.iOS
{

    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : UIApplicationDelegate
    {
        private readonly UIWindow window = new UIWindow(UIScreen.MainScreen.Bounds);
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Log.MessageToConsole("Starting FinishedLaunching");
            try
            {
                window.RootViewController = new UIViewController() { View = new UIView() };
                window.MakeKeyAndVisible();

                Log.MessageToConsole("Getting PlatformData");
                var pd = new PlatformData();
                Log.MessageToConsole("Getting OS");
                var os = pd.OS;
                Log.MessageToConsole("Logging OS");
                Log.MessageToConsole("OS: " + os.ToString());

                Log.MessageToConsole("Passing in");

                var pi = new PassedIn(pd);

                Log.MessageToConsole("Ending FinishedLaunching");
                return true;
            }
            catch (System.Exception ex)
            {
                Log.MessageToConsole($"EXCEPTION {ex.Message} ST:{ex.StackTrace}");
                return true;
            }
        }
    }
}
