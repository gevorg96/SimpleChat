using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleChat.Contracts
{
    public static class Extensions
    {
        public static IEnumerable<T> Unnullable<T>(this IEnumerable<T> collection) =>
            collection ?? Enumerable.Empty<T>();

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection.Unnullable())
                action(item);
        }

        public static (int port, string host) GetPortHostPair(Assembly assembly)
        {
            var host = "127.0.0.1";
            var port = 8888;

            var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()[0]);
            var appSettings = JObject.Load(new JsonTextReader(new StreamReader(stream)));

            try
            {
                host = (string)appSettings["Host"];
                port = Int32.Parse((string)appSettings["Port"]);
            }
            catch (Exception)
            { }

            return (port, host);
        }
    }
}
