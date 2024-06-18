using System.Diagnostics;
using OpenTelemetry;
using ServiceDesk.Util;

namespace ServiceDesk.Gateway;

public class BaggageExporter(QueueService queue) : BaseExporter<Activity>
{
    public override ExportResult Export(in Batch<Activity> batch)
    {
        using var scope = SuppressInstrumentationScope.Begin();

        foreach (var activity in batch)
        {
            var id = activity.GetBaggageItem("outbox");

            queue.SendMessageAsync(id);
        }

        return ExportResult.Success;
    }
}
