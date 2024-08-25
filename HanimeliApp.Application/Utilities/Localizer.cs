using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace HanimeliApp.Application.Utilities
{
	public class Localizer
    {
        private static Localizer instance = null!;

        public static Localizer Instance
        {
            get
            {
                return instance ??= new Localizer();
            }
        }

        private readonly ResourceManager _resourceManager;

        public Localizer()
        {
            _resourceManager = new ResourceManager("SizeFinder.Application.Resources.Resource", Assembly.GetAssembly(this.GetType())!);
        }

        public string? Translate(string key, string? culture = null)
        {
			var cultureInfo = string.IsNullOrEmpty(culture) ? CultureInfo.CurrentUICulture : new CultureInfo(culture);
			var message = _resourceManager.GetResourceSet(cultureInfo, true, true)!.GetString(key);

            return message ?? key;
        }

		public string GetLocalizedEnumTexts(Enum value)
        {
            var stringBuilder = new StringBuilder();

            foreach (Enum item in Enum.GetValues(value.GetType()))
            {
                if (value.HasFlag(item) && !item.ToString().Equals("None"))
                    stringBuilder.Append(Translate(item.ToString()) + ",");
            }

            return stringBuilder.ToString().TrimEnd(',');
        }
    }
}
