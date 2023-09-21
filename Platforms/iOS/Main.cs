using UIKit;
using System.Globalization;

namespace Summatic.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            AppDelegate.LogToDevice("Starting MAIN");
            UIApplication.Main(args, null, typeof(AppDelegate));
            AppDelegate.LogToDevice("Ending MAIN");
        }
    }
}
