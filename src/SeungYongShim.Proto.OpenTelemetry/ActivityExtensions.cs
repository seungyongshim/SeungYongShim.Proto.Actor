using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SeungYongShim.Proto.OpenTelemetry
{
    public static class ActivityExtensions
    {
        public static string ToInvariantString(this Exception exception)
        {
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                return exception.ToString();
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetError(this Activity activity, string description = null)
        {
            activity.SetTag("otel.status_code", "ERROR");

            if (description is null) return;

            activity.SetTag("otel.status_description", description);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RecordException(this Activity activity, Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            var tagsCollection = new ActivityTagsCollection
            {
                { SemanticConventions.AttributeExceptionType, ex.GetType().FullName },
                { SemanticConventions.AttributeExceptionStacktrace, ex.ToInvariantString() },
            };

            if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                tagsCollection.Add(SemanticConventions.AttributeExceptionMessage, ex.Message);
            }

            activity?.AddEvent(new ActivityEvent(SemanticConventions.AttributeExceptionEventName, default, tagsCollection));
        }
    }
}
