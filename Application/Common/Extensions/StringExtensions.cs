namespace Contacts.Application.Common.Extensions;

public static class StringExtensions
{
    public static string BuildWithQueryParams(this string str, IReadOnlyList<string> queryParamNames, Dictionary<string, object> queryParams)
    {
        var url = str;

        for (int i = 0; i < queryParamNames.Count; i++)
        {
            string param = queryParamNames[i];

            if (i == 0)
            {
                if (queryParams.TryGetValue(param, out var firstParamValue))
                {
                    url += $"?{param}={firstParamValue}";
                }

                continue;
            }

            if (queryParams.TryGetValue(param, out var value))
            {
                url += $"&{param}={value}";
            }
        }

        return url;
    }

    public static string BuildWithQueryParams(this string str, IReadOnlyList<string> queryParamNames, params object[] args)
    {
        var url = str;

        for (int i = 0; i < queryParamNames.Count; i++)
        {
            string param = queryParamNames[i];
            object value = args[i];

            if (i == 0)
            {
                {
                    url += $"?{param}={value}";
                }

                continue;
            }

            url += $"&{param}={value}";
        }

        return url;
    }
}