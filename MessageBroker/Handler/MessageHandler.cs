using MessageBroker.Interfaces;
using MessageBroker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MessageBroker
{
     public class MessageHandler
     {
          private string Message { get; set; }
          private readonly IClientRoomRepository clientRoomRepository;
          private readonly ISocketRepository socketRepository;
          private readonly IMessageRepository messageRepository;
          private readonly IConnectionService connectionService;

          public MessageHandler(IClientRoomRepository  clientRoomRepository, ISocketRepository socketRepository, IConnectionService connectionService , IMessageRepository messageRepository)
          {
               //this.Message = message;
               this.clientRoomRepository = clientRoomRepository;
               this.socketRepository = socketRepository;
               this.connectionService = connectionService;
               this.messageRepository = messageRepository;
          }

          internal void Publish(Type type, object message)
          {
               if (type == message.GetType())
               { 
                    System.Reflection.PropertyInfo pi = message.GetType().GetProperty("RoomType");
                    int roomType = (int)(pi.GetValue(message, null));

                    System.Reflection.PropertyInfo pt = message.GetType().GetProperty("Text");
                    string text = (string)(pt.GetValue(message, null));

                    var clientsId = clientRoomRepository.GetSocketsIdListForRoomType(roomType);

                    var listOfSubscribers = new List<SocketModel>();
                     foreach (int id in clientsId)
                      {
                          var subscriber = socketRepository.GetById(id);
                         if (subscriber != null)
                         {
                         listOfSubscribers.Add(subscriber);
                         }
                    }

                    if (listOfSubscribers != null && listOfSubscribers.Count > 0)
                    {
                         foreach (var client in listOfSubscribers)
                         {
                              text = FormatText(text);
                              connectionService.SendMessage(client.IpAddress, client.Port, text);
                         }
                    }
               }
          }

          internal void Subscribe(RoomType roomType, SocketModel socketModel)
          {

          }

          private string FormatText(string input)
          {
               string[] input1vec = input.Split(' ');

               string[,] dicCorect = new string[,]
               {
                {@"\bsi\b","?i" },
                {@"\bgasim\b","gasim" },
                {@"\bcand\b","cand" }
               };

               for (int j = 0; j < input1vec.Length; j++)
               {
                    for (int i = 0; i < dicCorect.GetLength(0); i++)
                    {
                         input1vec[j] = Regex.Replace(input1vec[j], dicCorect[i, 0], dicCorect[i, 1]);
                    }
               }

               string joined = String.Join(" ", input1vec);
               /// Outputul 
               return joined;
          }

          internal Message SaveMessage(Message messageObject)
          {
                return this.messageRepository.Insert(messageObject);
          }

          public void GetSubscribedRooms(Tuple<int, string> socketDetails)
          {
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
              // var sendBytes = socket.Send(buffer);
          }

          public void SubscribeToRoom()
          {

          }
     }
}
