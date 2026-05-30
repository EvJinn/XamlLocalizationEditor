using System.Globalization;

namespace XamlLocalizationEditor.Tools
{
    public static class CultureTools
    {
        public static bool IsValidLanguageCode(string code)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(code);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}
