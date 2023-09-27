using UIKit;
using System.Globalization;
using FSLibrary;

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
            Log.MessageToConsole("Starting MAIN");
            UIApplication.Main(args, null, typeof(AppDelegate));
            Log.MessageToConsole("Ending MAIN");
        }
    }
}
