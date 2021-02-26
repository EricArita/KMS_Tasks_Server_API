using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Helper.Exceptions.Project
{
    public class ProjectServiceException : BaseServiceException
    {
        public Dictionary<string, object> ExtraData { get; private set; }

        public ProjectServiceException(string message) : base(message)
        {
            ExtraData = new Dictionary<string, object>();
        }

        public void AddExtraData(string dataName, object data)
        {
            if (!ExtraData.ContainsKey(dataName))
            {
                ExtraData.Add(dataName, data);
            } else
            {
                ExtraData[dataName] = data;
            }
        }
    }
}
