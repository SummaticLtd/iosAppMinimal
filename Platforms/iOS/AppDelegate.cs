using Foundation;
using UIKit;
using System;
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
            LogToDevice("Starting FinishedLaunching");
            try
            {
                window.RootViewController = new UIViewController();
                window.MakeKeyAndVisible();

                LogToDevice("Ending FinishedLaunching");
                return true;
            }
            catch (System.Exception ex)
            {
                LogToDevice($"EXCEPTION {ex.Message} ST:{ex.StackTrace}");
                return true;
            }
        }

        public static void LogToDevice(string message)
        {
            Console.WriteLine($"ѦSUMMATIC CWL: {message}");
        }
    }
}
