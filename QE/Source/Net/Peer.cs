using System;
using System.Collections.Generic;
using System.Net;
using Lidgren.Network;

namespace QE.Net {

    [Serializable]
    public enum DeliveryMethod {
        Default = NetDeliveryMethod.Unreliable,
        Reliable = NetDeliveryMethod.ReliableOrdered,
    }

    public class Peer {
        const string SERVER_NICK = "QE_SERVER";
        const bool PLAIN_TEXT = false;

        NetPeer netPeer;

        public bool Server { get; private set; }
        public string Nick { get; private set; }

        bool _connected = false;
        public bool Connected {
            get {
                PeekMessage();
                return _connected;
            }
        }

        public Peer(string app, string nick, string host, int port) {
            Nick = nick;
            var conf = new NetPeerConfiguration(app);
            netPeer = new NetClient(conf);
            netPeer.Start();
            connections[SERVER_NICK] = netPeer.Connect(host, port);
            Send(SERVER_NICK, new ConnectMessage(), DeliveryMethod.Reliable);
        }

        public Peer(string app, int port) {
            Server = true;
            Nick = SERVER_NICK;
            var conf = new NetPeerConfiguration(app);
            conf.Port = port;
            netPeer = new NetServer(conf);
            netPeer.Start();
        }

        public void Send(object msg, DeliveryMethod method) {
            Log.Info("Sending to everyone: " + string.Join(", ", connections.Keys));
            foreach (var connection in connections.Values)
                Send(connection, msg, method);
        }
        public void Send(string to, object msg, DeliveryMethod method) {
            if (!connections.ContainsKey(to))
                return;
            Send(connections[to], msg, method);
        }
        public void SendToServer(object msg, DeliveryMethod method) {
            Send(SERVER_NICK, msg, method);
        }
        void Send(NetConnection to, object msg, DeliveryMethod method) {
            var message = netPeer.CreateMessage();
            message.Write(Nick);
            if (PLAIN_TEXT) {
                var data = Serializer.ToJson(msg);
                Log.Info("SENT " + data);
                message.Write(data);
            } else {
                byte[] data = Serializer.ToBytes(msg);
                message.Write(data.Length);
                message.Write(data);
            }
            netPeer.SendMessage(message, to, (NetDeliveryMethod)method);
        }

        Message peekMessage = null;
        Message PeekMessage() {
            if (peekMessage == null)
                peekMessage = GetMessage();
            return peekMessage;
        }
        void SkipMessage() {
            peekMessage = null;
        }

        void SendPlayersInfo() {
            var info = new PlayersInfo();
            foreach (var conn in connections)
                info.players.Add(conn.Key);
            Send(info, DeliveryMethod.Reliable);
        }

        Message GetMessage() {
            NetIncomingMessage message;
            while ((message = netPeer.ReadMessage()) != null) {
                switch (message.MessageType) {
                case NetIncomingMessageType.Data:
                    string sender = message.ReadString();
                    object msg;
                    if (PLAIN_TEXT) {
                        var data = message.ReadString();
                        Log.Info(Nick + " RECEIVED " + data);
                        msg = Serializer.FromJson<object>(data);
                    } else {
                        int size = message.ReadInt32();
                        msg = Serializer.FromBytes<object>(message.ReadBytes(size));
                    }
                    if (msg is ConnectMessage && Server) {
                        var connectMsg = (ConnectMessage)msg;
                        if (connections.ContainsKey(sender)) {
                            // TODO GET OUT
                            break;
                        }
                        connections[sender] = message.SenderConnection;
                        SendPlayersInfo();
                        break;
                    }
                    if (msg is PlayersInfo && !Server) {
                        var info = (PlayersInfo)msg;
                        players = new HashSet<string>(info.players);
                        _connected = true;
                        break;
                    }
                    return new Message(sender, msg);
                case NetIncomingMessageType.StatusChanged:
                    if (Server) {
                        CheckBrokenConnections();
                        SendPlayersInfo();
                    }
                    break;
                default:
                    Log.Info("Got unrecognized message " + message);
                    break;
                }
            }
            return null;
        }

        List<Action<string, object>> handlers = new List<Action<string, object>>();
        public void AddHandler<T>(Action<string, T> handler) {
            handlers.Add((string sender, object msg) => {
                if (msg is T)
                    handler?.Invoke(sender, (T)msg);
            });
        }

        public void CheckMessages() {
            Message m;
            while ((m = PeekMessage()) != null) {
                foreach (var handler in handlers)
                    handler?.Invoke(m.sender, m.data);
                SkipMessage();
            }
            CheckBrokenConnections();
        }

        void CheckBrokenConnections() {
            List<string> brokenConnections = new List<string>();
            foreach (var conn in connections)
                if (conn.Value == null || conn.Value.Status == NetConnectionStatus.Disconnected) {
                    brokenConnections.Add(conn.Key);
                }
            foreach (var nick in brokenConnections)
                connections.Remove(nick);
        }

        Dictionary<string, NetConnection> connections = new Dictionary<string, NetConnection>();
        HashSet<string> players = new HashSet<string>();
        public IEnumerable<string> ConnectedPlayers {
            get { return players; }
        }

        [Serializable]
        class Message {
            public string sender;
            public object data;
            public Message(string sender, object data) {
                this.sender = sender;
                this.data = data;
            }
        }

        [Serializable]
        class ConnectMessage {
        }

        [Serializable]
        class PlayersInfo {
            public List<string> players = new List<string>();
        }

    }

}