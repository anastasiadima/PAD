using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageBroker
{
     public class DatabaseContext : DbContext 
     {
          protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
             optionsBuilder.UseSqlServer(@"Server=DESKTOP-HH3PAF5;Database=MessageBroker;Trusted_Connection=True;");
         }

          public DbSet<Message> Messages { get; set; }
          public DbSet<ClientRoom> ClientRooms { get; set; }
          public DbSet<SocketModel> SocketModels { get; set; }

     }
}
