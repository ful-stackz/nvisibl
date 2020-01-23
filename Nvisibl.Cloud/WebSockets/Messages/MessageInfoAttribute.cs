using System;

namespace Nvisibl.Cloud.WebSockets.Messages
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class MessageInfoAttribute : Attribute
    {
        public MessageInfoAttribute(string typeName)
        {
            TypeName = !string.IsNullOrEmpty(typeName)
                ? typeName
                : throw new ArgumentException("Argument was null or empty.", nameof(typeName));
        }

        public string TypeName { get; set; }
    }
}
