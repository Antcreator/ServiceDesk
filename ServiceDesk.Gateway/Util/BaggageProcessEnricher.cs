using System.Diagnostics;
using Microsoft.Net.Http.Headers;

namespace ServiceDesk.Gateway;

public static class BaggageProcessEnricher
{
    public static void LoadBaggage(Activity activity, HttpResponseMessage response)
    {
        var headers = response.Headers
            .TryGetValues(HeaderNames.Baggage, out var list) switch
            { true => list, _ => [] };

        foreach (var header in headers)
        {
            var baggage = header.Split("=");
            var key = baggage[0];
            var value = baggage[1];

            activity.AddBaggage(key, value);
        }
    }
}
