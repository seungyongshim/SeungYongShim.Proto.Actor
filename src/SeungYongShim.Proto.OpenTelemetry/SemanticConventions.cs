using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeungYongShim.Proto.OpenTelemetry
{
    static class SemanticConventions
    {
        public const string AttributeExceptionEventName = "exception";
        public const string AttributeExceptionType = "exception.type";
        public const string AttributeExceptionMessage = "exception.message";
        public const string AttributeExceptionStacktrace = "exception.stacktrace";
    }
}
