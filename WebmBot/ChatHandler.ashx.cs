using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using System.Text;
using System.Web.UI;

namespace WebmBot
{
    /// <summary>
    /// Сводное описание для ChatHandler
    /// </summary>
    public class ChatHandler : IHttpHandler
    {
        private static readonly List<WebSocket> Clients = new List<WebSocket>();
        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        static List<ArraySegment<byte>> HistoryResult = new List<ArraySegment<byte>>();
        public static string MSG="";

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(WebSocketRequest);
        }
        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            // Получаем сокет клиента из контекста запроса
            var socket = context.WebSocket;

            // Добавляем его в список клиентов
            Locker.EnterWriteLock();
            try
            {
                Clients.Add(socket);
                foreach (ArraySegment<byte> historyBuffer in HistoryResult)
                {
                    string messageHistory = Encoding.UTF8.GetString(historyBuffer.Array);
                    string ntmps = "";
                    for (int i = 0; i < messageHistory.Length; i++)
                    {
                        if (messageHistory[i].ToString() != "\0" && messageHistory[i].ToString() != "/0")
                        {
                            ntmps += messageHistory[i].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(ntmps)&&!string.IsNullOrWhiteSpace(ntmps))
                    {
                        var msgBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(ntmps));
                        try
                        {
                            if (socket.State == WebSocketState.Open)
                            {
                                await socket.SendAsync(msgBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }

                        catch (ObjectDisposedException)
                        {
                            Locker.EnterWriteLock();
                            try
                            {
                                Clients.Remove(socket);
                            }
                            finally
                            {
                                Locker.ExitWriteLock();
                            }
                        }
                    }
                }
            }
            finally
            {
                Locker.ExitWriteLock();
            }

            // Слушаем его
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);
              
                

                // Ожидаем данные от него
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer.Array);
                if (HistoryResult.Count<100)
                {
                    HistoryResult.Add(buffer);
                }
                else
                {
                    HistoryResult.Clear();
                    HistoryResult.Add(buffer);
                }
                
                

                //Передаём сообщение всем клиентам
                for (int i = 0; i < Clients.Count; i++)
                {

                    WebSocket client = Clients[i];
                    try
                    {
                        if (client.State == WebSocketState.Open)
                        {
                            string ntmps = "";
                            for (int j = 0; j < message.Length; j++)
                            {
                                if (message[j].ToString() != "\0" && message[j].ToString() != "/0")
                                {
                                    ntmps += message[j].ToString();
                                }
                            }

                            var msgBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(ntmps));

                            await client.SendAsync(msgBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }

                    catch (ObjectDisposedException)
                    {
                        Locker.EnterWriteLock();
                        try
                        {
                            Clients.Remove(client);
                            i--;
                        }
                        finally
                        {
                            Locker.ExitWriteLock();
                        }
                    }
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}