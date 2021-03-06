using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XSockets.ClientPortableW8.Common.Event.Arguments;
using XSockets.ClientPortableW8.Model;

namespace XSockets.ClientPortableW8.Common.Interfaces
{
    public interface IController
    {
        IClientInfo ClientInfo { get; set; }
        IXSocketClient XSocketClient { get; }

        event EventHandler<Message> OnMessage;
        event EventHandler<OnClientConnectArgs> OnOpen;
        event EventHandler<OnClientDisconnectArgs> OnClose;
        event EventHandler<OnErrorArgs> OnError;
        
        void Close();        

        void BindUnboundSubscriptions();

        void FireOnMessage(IMessage message);
        void FireOnBlob(IMessage message);
        void FireClosed();       

        IMessage AsMessage(string topic, object o);

        //Properties
        Task SetEnum(string propertyName, string value);
        Task SetProperty(string propertyName, object value);

        //STORAGE
        T StorageGet<T>(string key);
        Task StorageSet<T>(string key, T value); //where T : class;
        IListener StorageOnSet<T>(string key, Action<T> action); // where T : class;
        IListener StorageOnRemove<T>(string key, Action<T> action); // where T : class;
        Task StorageRemove(string key);
        Task StorageClear();


        //PUBSUB
        Task One(string topic, Action<IMessage> callback);
        Task One<T>(string topic, Action<T> callback); //where T : class;
        Task One(string topic, Action<IMessage> callback, Action<IMessage> confirmCallback);
        Task One<T>(string topic, Action<T> callback, Action<IMessage> confirmCallback); //where T : class;
        Task Many(string topic, uint limit, Action<IMessage> callback);
        Task Many<T>(string topic, uint limit, Action<T> callback);// where T : class;
        Task Many(string topic, uint limit, Action<IMessage> callback, Action<IMessage> confirmCallback);
        Task Many<T>(string topic, uint limit, Action<T> callback, Action<IMessage> confirmCallback);// where T : class;

        /// <summary>
        /// Remove the subscription from the list
        /// </summary>
        /// <param name="topic"></param>
        Task Unsubscribe(string topic);

        Task Subscribe(string topic);
        Task Subscribe(string topic, SubscriptionType subscriptionType, uint limit = 0);
        Task Subscribe(string topic, Action<IMessage> callback, SubscriptionType subscriptionType = SubscriptionType.All, uint limit = 0);
        Task Subscribe(string topic, Action<IMessage> callback, Action<IMessage> confirmCallback, SubscriptionType subscriptionType = SubscriptionType.All, uint limit = 0);

        Task Subscribe<T>(string topic, Action<T> callback, SubscriptionType subscriptionType = SubscriptionType.All,
            uint limit = 0);// where T : class;

        Task Subscribe<T>(string topic, Action<T> callback, Action<IMessage> confirmCallback,
            SubscriptionType subscriptionType = SubscriptionType.All, uint limit = 0);// where T : class;

        /// <summary>
        ///     Send message
        /// </summary>
        /// <param name="payload">IMessage</param>
        /// <param name="callback"> </param>
        Task Publish(IMessage payload, Action callback);

        /// <summary>
        /// Send a binary message with metadata
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        /// <param name="metadata"></param>                
        Task Publish(string topic, List<byte> data, object metadata);
        Task Publish(string topic, byte[] data, object metadata);
        Task Publish(string topic, List<byte> data);
        Task Publish(string topic, byte[] data);

        //void Publish(string payload);
        Task Publish(string payload, Action callback);
        Task Publish(IMessage payload);
        Task Publish(string topic);
        Task Publish(string topic, object obj);
        Task Publish(string topic, object obj, Action callback);        

        //RPC
        Task Invoke(IMessage payload);
        Task Invoke(string target);
        Task Invoke(string target, object data);
        Task Invoke(string target, IList<byte> data);
        Task Invoke(string target, byte[] data);
        Task Invoke(string target, IList<byte> data, object metadata);
        Task<T> Invoke<T>(string target, int timeoutMilliseconds = 30000);
        Task<T> Invoke<T>(string target, object data, int timeoutMilliseconds = 30000);
        Task<T> Invoke<T>(string target, IList<byte> data, int timeoutMilliseconds = 30000);
        Task<T> Invoke<T>(string target, byte[] data, int timeoutMilliseconds = 30000);
        Task<T> Invoke<T>(string target, IList<byte> blob, object metadata, int timeoutMilliseconds = 30000);
        Task<T> Invoke<T>(string target, byte[] blob, object metadata, int timeoutMilliseconds = 30000);
        IListener On<T>(string target, Action<T> action);
        //IListener On(string target, Action<dynamic> action);
        IListener On(string target, Action action);
        void DisposeListener(IListener listener);
    }
}