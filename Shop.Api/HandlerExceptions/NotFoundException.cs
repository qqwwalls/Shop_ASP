using System;

namespace Shop.Api.HandlerExceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string messsage): base (messsage)
        {
        }
    }
}
