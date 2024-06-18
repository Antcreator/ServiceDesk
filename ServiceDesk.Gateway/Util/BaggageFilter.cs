using System.Diagnostics;
using OpenTelemetry;

namespace ServiceDesk.Gateway;

public class BaggageFilter : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        var id = activity.GetBaggageItem("outbox");

        if (string.IsNullOrWhiteSpace(id))
        {
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }
}
