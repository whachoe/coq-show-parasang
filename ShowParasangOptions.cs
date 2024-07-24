namespace XRL.UI
{
    [HasModSensitiveStaticCache]
    public class ShowParasangOptions
    {
        public static bool showParasangMessage => GetOption("OptionsShowParasangShowMessage").EqualsNoCase("Yes");

        public static string GetOption(string ID, string Default = "")
        {
            return Options.GetOption(ID, Default);
        }
    }
}