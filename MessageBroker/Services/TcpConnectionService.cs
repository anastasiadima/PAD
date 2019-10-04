using MessageBroker.Interfaces;
using MessageBroker.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MessageBroker.Services
{
     public class TcpConnectionService : IConnectionService
     {
          private Socket Socket { get; set; }
          private ArrayList SocketList = ArrayList.Synchronized(new ArrayList());
          private int bufferSize = 1024;
          byte[] _buffer = new byte[1024];
          private Publisher publisher;

          private static DatabaseContext context = new DatabaseContext();
          private MessageRepository messageRepository = new MessageRepository(context);
          private ClientRoomRepository clientRoomRepository = new ClientRoomRepository(context);
          private SocketRepository socketRepository = new SocketRepository(context);
          ManualResetEvent allDone = new ManualResetEvent(false);
          private IMessageHub messageHub;
          private MessageHandler messageHandler;

          public void Connect()
          {
               IPAddress ipAddress = IPAddress.Any;
               IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);
               Socket listener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
               listener.Bind(localEndPoint);
               listener.Listen(10);
               messageHub = new MessageHub();
                messageHandler = new MessageHandler(clientRoomRepository, socketRepository, new TcpConnectionService(), messageRepository);
               messageHub.RegisterGlobalHandler((type, message) => { messageHandler.Publish(type, message); });
               publisher = new Publisher(messageHub);
               this.Socket = listener;
               while (true)
               {
                    //  The threads are put into waiting state 
                    allDone.Reset();
                    Console.WriteLine("Waiting for a connection...");

                    //start accepting new connections
                    //AcceptCallback is called when a new connection request is received 
                    //Listener as object to pass state information to callback
                    this.Socket.BeginAccept(new AsyncCallback(AcceptCallback), this.Socket);

                    //block theards 
                    allDone.WaitOne();
               }

          }

          private void AcceptCallback(IAsyncResult asyncResult)
          {
               allDone.Set();
               Socket socket = (Socket)asyncResult.AsyncState;
               Socket handler = socket.EndAccept(asyncResult);
               string ipAddress = handler.RemoteEndPoint.ToString();

               SocketList.Add(handler);
               Console.WriteLine("Connected {0}", handler);
               handler.BeginReceive(_buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), handler);
          }

          private void ReadCallback(IAsyncResult asyncResult)
          {
               Socket handler = (Socket)asyncResult.AsyncState;
               int bytesToRead = handler.EndReceive(asyncResult); 
               byte[] receiveBuffer = new byte[bytesToRead];
               Array.Copy(_buffer, receiveBuffer, bytesToRead);
               string text = Encoding.ASCII.GetString(receiveBuffer);
               Console.WriteLine(text);

               ChooseScopeofMessage(text, handler);
               //handler.Close();
               handler.BeginReceive(_buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), handler);
          }

          private void ChooseScopeofMessage(string text, Socket socket)
          {
               if (text.Length >= 9 && text.Substring(0, 9) == "Subscribe")
               {
                    RoomType roomType = getRoomType(text[9]);
                    SubscribeToRoom(roomType, socket);
               }
               else if (text == "GetRooms")
               {
                    SendSubscribedRooms(socket);
               }
               else if (text.Length >= 11 && text.Substring(0, 11) == "UnSubscribe")
               {
                    RoomType roomType = getRoomType(text[11]);
                    UnsubscribeRoom(roomType, socket);
               }
               else if (text.Length > 9 && text.Substring(0, 9)== "Publisher")
               {
                    PublisherMessage(text);
               }
               else if (text == "GetInfo")
               {
                    SendCorespondingMessages(socket);
               }
          }

          private void SendCorespondingMessages(Socket socket)
          {
               RoomSubscriber roomSubscriber = new RoomSubscriber(messageHub);
               roomSubscriber.GetMessages();
          }

          public void PublisherMessage(string txt)
          {
               var length = txt.Length - 9;
               var messageJson = txt.Substring(9, length );
               Console.WriteLine(messageJson);

               var message = new Message();
               try
               {
                    Message messageObject = JsonConvert.DeserializeObject<Message>(messageJson);
                    messageObject = messageHandler.SaveMessage(messageObject);
                    publisher.Publish(messageObject);
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
          }

          public void UnsubscribeRoom(RoomType roomType, Socket socket)
          {

               SocketModel socketModel = generateSocketModel(roomType, socket);
               var id = socketRepository.GetSocketId(socketModel.Port, socketModel.IpAddress);
               if (id != 0)
               {
                    var clientRoom = new ClientRoom();
                    clientRoom.Room = (int)roomType;
                    clientRoom.SocketId = id;
                    var clientroomid = clientRoomRepository.GetClientRoomId(clientRoom);
                    if (clientroomid != 0)
                    {
                         clientRoomRepository.Delete(clientroomid);
                    }
               }
          }
          public SocketModel generateSocketModel(RoomType roomType, Socket socket)
          {
               SocketModel socketModel = new SocketModel();

               Tuple<int, string> getDetails = GetIpAddress(socket);
               socketModel.IpAddress = getDetails.Item2;
               socketModel.Port = getDetails.Item1;

               return socketModel;
          }
          public void SendMessage(string ipAddress, int port, string message)
          {
               IPEndPoint remoteService = new IPEndPoint(IPAddress.Parse(ipAddress), port);
               Socket socket = new Socket(remoteService.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
               try
               {
                    socket.BeginConnect(remoteService, new AsyncCallback(ConnectCallback), socket);
                    if (socket.Connected)
                    {
                         var buffer = Encoding.ASCII.GetBytes(message);
                         socket.Send(buffer);
                    }
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
          }

          private static void ConnectCallback(IAsyncResult ar)
          {
               try
               {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.  
                    client.EndConnect(ar);

                    Console.WriteLine("Socket connected to {0}",
                        client.RemoteEndPoint.ToString());

                    // Signal that the connection has been made.  
                    //connectDone.Set();
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.ToString());
               }
          }

          private static RoomType getRoomType(char c)
          {
               RoomType roomType = RoomType.None;

               switch (c)
               {
                    case '1':
                         roomType = RoomType.Entertaiment;
                         break;
                    case '2':
                         roomType = RoomType.Politic;
                         break;
                    case '3':
                         roomType = RoomType.Sport;
                         break;
                    default:
                         break;
               }

               return roomType;
          }

          public static Tuple<int, string> GetIpAddress(Socket socket)
          {
               var endPoint = socket.RemoteEndPoint.ToString();
               string[] address;
               address = endPoint.Split(':');
               if (address != null && address.Length == 2)
               {
                    var ip = address[0];
                    var port = address[1];
                    if (int.TryParse(port, out var intPort))
                    {
                         return new Tuple<int, string>(intPort, ip);
                    }
               }

               return new Tuple<int, string>(0, "");
          }
          public void SubscribeToRoom(RoomType roomType, Socket socket)
          {
               SocketModel socketModel = generateSocketModel(roomType, socket);
               var modelId = socketRepository.GetSocketId(socketModel.Port, socketModel.IpAddress);
               if (modelId == 0)
               {
                    socketModel = socketRepository.Insert(socketModel);
               }
               var clientRoom = new ClientRoom();
               clientRoom.Room = (int)roomType;
               clientRoom.SocketId = modelId;
               clientRoomRepository.Insert(clientRoom);
          }

          public void SendSubscribedRooms(Socket socket)
          {
               Tuple<int, string> socketDetails = GetIpAddress(socket);
               var socketModelId = socketRepository.GetSocketId(socketDetails.Item1, socketDetails.Item2);
               string result = "";
               if (socketModelId == 0)
               {
                    var model = new SocketModel();
                    model.IpAddress = socketDetails.Item2;
                    model.Port = socketDetails.Item1;
                    model = socketRepository.Insert(model);
                    socketModelId = model.Id;
                    result = "No subscribed rooms";
               }

               if (socketModelId != 0)
               {
                    IList<RoomType> listOfRooms = clientRoomRepository.GetRoomsForClientId(socketModelId);
                    if (listOfRooms != null && listOfRooms.Count > 0)
                    {
                         result = "You are subscribed to the rooms: ";

                         foreach (var item in listOfRooms)
                         {
                              result += item.ToString();
                              result += " ";
                         }
                    }
               }
               var buffer = Encoding.ASCII.GetBytes(result);
               //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
               var sendBytes = socket.Send(buffer);
               byte[] _buffer = new byte[bufferSize]; 

               if (sendBytes > 0)
               {
                    socket.BeginReceive(_buffer, 0, bufferSize, 0, new AsyncCallback(ReadCallback), socket);
               }
          }
     }
}
