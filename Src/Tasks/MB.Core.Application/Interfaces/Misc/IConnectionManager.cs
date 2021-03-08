using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Interfaces.Misc
{
    public interface IConnectionManager
    {
        public void RegisterConnection(string connectionId);
        public void RemoveConnection(string connectionId);
        public void AddConnectionToRoom(string connectionId, string roomName);
        public void RemoveConnectionFromRoom(string connectionId, string roomName);
        public bool ClearRoomsOfConnection(string connectionId);

        public HashSet<string> GetRoomsOfConnection(string connectionId);

    }
}
