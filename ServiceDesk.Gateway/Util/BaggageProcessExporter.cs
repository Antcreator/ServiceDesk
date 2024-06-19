using System.Diagnostics;
using OpenTelemetry;
using ServiceDesk.Util;

namespace ServiceDesk.Gateway;

public class BaggageProcessExporter(QueueService queue) : BaseExporter<Activity>
{
    public override ExportResult Export(in Batch<Activity> batch)
    {
        using var scope = SuppressInstrumentationScope.Begin();

        foreach (var activity in batch)
        {
            var baggage = activity.Baggage
                .FirstOrDefault(baggage => baggage.Key == "outbox");
            
            queue.SendMessageAsync(baggage.Value);
        }

        return ExportResult.Success;
    }
}
