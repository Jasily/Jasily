using System.Text.RegularExpressions;

namespace Jasily.Converters.StringValueConverters
{
    public class RegexStringConverter : BaseStringConverter<Regex>
    {
        protected override Regex ConvertCore(string value)
        {
            return new Regex(value);
        }
    }
}