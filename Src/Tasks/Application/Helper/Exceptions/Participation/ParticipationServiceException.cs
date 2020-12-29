using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helper.Exceptions.Participation
{
    public class ParticipationServiceException : BaseServiceException
    {
        public ParticipationServiceException(string message) : base(message)
        {
        }
    }
}
