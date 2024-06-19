using System.Diagnostics;
using OpenTelemetry;

namespace ServiceDesk.Gateway;

public class BaggageProcessFilter : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        if (!activity.Baggage.Any(baggage => baggage.Key == "outbox"))
        {
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }
}
