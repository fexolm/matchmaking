using System.Collections.Generic;
using System.Net;

namespace matchmaking
{
    public class ConnectionManager
    {
        private readonly Dictionary<string, IPEndPoint> _connectionFromToken = new Dictionary<string, IPEndPoint>();
        private readonly Dictionary<IPEndPoint, string> _tokenFromConnection = new Dictionary<IPEndPoint, string>();

        public string this[IPEndPoint address] {
            get {
                lock (_tokenFromConnection) {
                    return _tokenFromConnection[address];
                }
            }
        }

        public IPEndPoint this[string token] {
            get {
                lock (_connectionFromToken) {
                    return _connectionFromToken[token];
                }
            }
        }
        
        private void Add(string token, IPEndPoint connection) {
            _connectionFromToken[token] = connection;
            _tokenFromConnection[connection] = token;
        }

        public void Update(string token, IPEndPoint connection) {
            lock (_connectionFromToken)
            lock (_tokenFromConnection) {
                bool tokenExitst = _connectionFromToken.ContainsKey(token);
                bool connectionExist = _tokenFromConnection.ContainsKey(connection);
                if (!tokenExitst && connectionExist) {
                    Add(token, connection);
                }
            }
        }

        public void Remove(string token) {
            lock (_connectionFromToken)
            lock (_tokenFromConnection) {
                var connection = _connectionFromToken[token];
                _connectionFromToken.Remove(token);
                _tokenFromConnection.Remove(connection);
            }
        }
    }
}