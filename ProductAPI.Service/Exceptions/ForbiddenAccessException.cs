using System.Runtime.Serialization;

namespace ProductAPI.Service.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message)
        {

        }

        protected ForbiddenAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
