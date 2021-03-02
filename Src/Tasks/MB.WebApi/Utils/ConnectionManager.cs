using MB.Core.Application.Interfaces.Misc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MB.WebApi.Utils
{
    public class ConnectionManager : IConnectionManager
    {
        private HashSet<string> allClients;

        private Dictionary<string, HashSet<string>> connection_to_rooms;
        private ILogger<ConnectionManager> _logger;

        public ConnectionManager(ILogger<ConnectionManager> logger)
        {
            allClients = new HashSet<string>();

            connection_to_rooms = new Dictionary<string, HashSet<string>>();
            _logger = logger;
        }

        public void RegisterConnection(string connectionId)
        {
            allClients.Add(connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            allClients.Remove(connectionId);

            bool connectionAvailable = connection_to_rooms.ContainsKey(connectionId);
            if (connectionAvailable)
            {
                connection_to_rooms.Remove(connectionId);
            }
        }

        public void AddConnectionToRoom(string connectionId, string roomName)
        {
            bool connectionAvailable = connection_to_rooms.ContainsKey(connectionId);
            HashSet<string> roomsOfConnection;
            if (connectionAvailable)
            {
                roomsOfConnection = connection_to_rooms[connectionId];
            }
            else
            {
                roomsOfConnection = new HashSet<string>();
                connection_to_rooms.Add(connectionId, roomsOfConnection);
            }
            roomsOfConnection.Add(roomName);
        }

        public void RemoveConnectionFromRoom(string connectionId, string roomName)
        {
            bool connectionAvailable = connection_to_rooms.ContainsKey(connectionId);
            if (connectionAvailable)
            {
                connection_to_rooms[connectionId].Remove(roomName);
            }
        }
    }
}
