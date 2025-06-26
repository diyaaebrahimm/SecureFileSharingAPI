using System;
using System.Collections.Generic;

namespace SecureFileSharingAPI
{
    /// <summary>
    /// Represents a client identified by IP address or token and
    /// keeps track of recent request timestamps per endpoint.
    /// </summary>
    public class ClientProfile
    {
        private readonly Dictionary<string, List<DateTimeOffset>> _requestLog = new();

        /// <summary>
        /// Records a timestamp for the specified endpoint.
        /// </summary>
        public void LogRequest(string path, DateTimeOffset timestamp)
        {
            if (!_requestLog.TryGetValue(path, out var list))
            {
                list = new List<DateTimeOffset>();
                _requestLog[path] = list;
            }
            list.Add(timestamp);
        }

        /// <summary>
        /// Gets the recorded timestamps for the specified endpoint.
        /// </summary>
        public List<DateTimeOffset> GetRequests(string path)
        {
            return _requestLog.TryGetValue(path, out var list) ? list : new List<DateTimeOffset>();
        }
    }
}
