using Flurl.Http;
using System.Runtime.Serialization;

namespace evolUX.UI.Exceptions
{
    [Serializable]
    internal class HttpUnauthorizedException : Exception
    {
        public IFlurlResponse response;

        public HttpUnauthorizedException()
        {
        }

        public HttpUnauthorizedException(IFlurlResponse response)
        {
            this.response = response;
        }

        public HttpUnauthorizedException(string? message) : base(message)
        {
        }

        public HttpUnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HttpUnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}