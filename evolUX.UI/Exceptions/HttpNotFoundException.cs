using Flurl.Http;
using System.Runtime.Serialization;

namespace evolUX.UI.Exceptions
{
    [Serializable]
    internal class HttpNotFoundException : Exception
    {
        public IFlurlResponse response;

        public HttpNotFoundException()
        {
        }

        public HttpNotFoundException(IFlurlResponse response)
        {
            this.response = response;
        }

        public HttpNotFoundException(string? message) : base(message)
        {
        }

        public HttpNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HttpNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}