using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiMay.Service.Core
{
    public class GlobalMessageBus
    {
        private static IDictionary<string, Action<string, object>> _subscribeDictionary = new Dictionary<string, Action<string, object>>();

        public static void SubScribe(string topic, Action<string, object> action)
        {
            if (_subscribeDictionary.ContainsKey(topic) && _subscribeDictionary.TryGetValue(topic, out var outAction))
                outAction += action;
            else
                _subscribeDictionary[topic] = new Action<string, object>(action);
        }

        public static void UnSubScribe(string topic, Action<string, object> action)
        {
            if (_subscribeDictionary.ContainsKey(topic) && _subscribeDictionary.TryGetValue(topic, out var outAction))
                outAction -= action;
        }

        public static void Publish(string topic, object message)
        {
            if (_subscribeDictionary.ContainsKey(topic) && _subscribeDictionary.TryGetValue(topic, out var outAction))
                outAction?.Invoke(topic, message);
        }
    }

    public class BusTopic
    {
        public const string CREATE_SERVICE_POST_TO_SEQUENCE = "CREATE_SERVICE_POST_TO_SEQUENCE";
    }
}
