using OpenTelemetry;
using OpenTelemetry.Trace;
using ServiceDesk.Util;

namespace ServiceDesk.Gateway;

public static class BaggageProcessExtension
{
    public static TracerProviderBuilder AddBaggageProcessor(this TracerProviderBuilder trace)
    {
        trace.AddProcessor(new BaggageProcessFilter());
        trace.AddProcessor(services =>
        {
            var queue = services.GetRequiredService<QueueService>();
            var exporter = new BaggageProcessExporter(queue);

            return new SimpleActivityExportProcessor(exporter);
        });

        return trace;
    }
}
