using System.Text;
using System.Text.Json;

namespace Services;

public class PreservePropertyNames : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return name.Replace('_', '.');
    }
}
