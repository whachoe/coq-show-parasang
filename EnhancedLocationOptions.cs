namespace XRL.UI
{
    [HasModSensitiveStaticCache]
    public class EnhancedLocationOptions
    {
        public static bool showParasangMessage => GetOption("OptionsEnhancedLocationShowMessage").EqualsNoCase("Yes");

        public static string GetOption(string ID, string Default = "")
        {
            return Options.GetOption(ID, Default);
        }
    }
}