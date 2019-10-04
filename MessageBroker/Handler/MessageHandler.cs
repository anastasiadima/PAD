using MessageBroker.Interfaces;
using MessageBroker.Models;
using System;
using System.Collections.Generic;
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
                              connectionService.SendMessage(client.IpAddress, client.Port, text);
                         }
                    }
               }
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
                         Console.WriteLine(input1vec[j]);
                         input1vec[j] = Regex.Replace(input1vec[j], dicCorect[i, 0], dicCorect[i, 1]);
                         Console.WriteLine(input1vec[j]);
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
     }
}
