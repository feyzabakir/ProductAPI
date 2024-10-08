﻿using System.Runtime.Serialization;

namespace ProductAPI.Service.Exceptions
{
    [Serializable]
    public class ClientSideException : Exception
    {
        public ClientSideException(string message) : base(message)
        {

        }
        protected ClientSideException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
