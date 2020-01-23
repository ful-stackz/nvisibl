using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Nvisibl.Cloud.WebSockets.Messages.Client.Base;
using Nvisibl.Cloud.WebSockets.Messages.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages.Server.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nvisibl.Cloud.WebSockets.Messages
{
    public class MessageParser : IMessageParser
    {
        private const string PropNameType = "type";
        private const string PropNamePayload = "payload";

        private readonly Dictionary<Type, string> _serverMessageTypes;
        private readonly Dictionary<string, Type> _clientMessageTypes;

        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public MessageParser()
        {
            var serverMessageType = typeof(ServerMessageBase);
            var clientMessageType = typeof(ClientMessageBase);
            var messageInfos = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(serverMessageType) || t.IsSubclassOf(clientMessageType))
                .Select(t => (Type: t, MessageInfo: t.GetCustomAttribute<MessageInfoAttribute>()))
                .Where(t => t.MessageInfo is { })
                .ToList();

            _serverMessageTypes = messageInfos
                .Where(ti => ti.Type.IsSubclassOf(serverMessageType))
                .ToDictionary(mi => mi.Type, mi => mi.MessageInfo!.TypeName.ToUpperInvariant());

            _clientMessageTypes = messageInfos
                .Where(ti => ti.Type.IsSubclassOf(clientMessageType))
                .ToDictionary(mi => mi.MessageInfo!.TypeName.ToUpperInvariant(), mi => mi.Type);
        }

        public string SerializeServerMessage(ServerMessageBase message)
        {
            if (message is null || !_serverMessageTypes.TryGetValue(message.GetType(), out string? typeName))
            {
                return string.Empty;
            }

            var json = new JObject(
                new JProperty(PropNameType, typeName),
                new JProperty(PropNamePayload, JObject.FromObject(message, Serializer)));
            return json.ToString(Formatting.None);
        }

        public ClientMessageBase DeserializeClientMessage(string message)
        {
            JObject? json = null;
            try
            {
                json = JToken.Parse(message) as JObject;
            }
            catch
            {
                // Ignore
            }

            if (json is null)
            {
                return ClientMessageBase.Empty;
            }

            string typeName = json.Property(PropNameType)?.Value?.ToString().ToUpperInvariant() ?? string.Empty;
            JToken payload = json.Property(PropNamePayload)?.Value ?? new JObject();

            return _clientMessageTypes.TryGetValue(typeName, out Type? messageType) && messageType is { }
                ? payload.ToObject(messageType, Serializer) as ClientMessageBase ?? ClientMessageBase.Empty
                : ClientMessageBase.Empty;
        }
    }
}
