using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class SysLogs
    {
        public long Id { get; set; }
        public DateTime When { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string Trace { get; set; }
        public string Logger { get; set; }
    }
}
