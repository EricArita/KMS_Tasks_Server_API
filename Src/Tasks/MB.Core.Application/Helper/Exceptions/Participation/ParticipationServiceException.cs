using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Helper.Exceptions.Participation
{
    public class ParticipationServiceException : BaseServiceException
    {
        public ParticipationServiceException(string message) : base(message)
        {
        }
    }
}
