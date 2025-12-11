using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Integration.Tests.Persistence.Repositories
{
    public class MessageTests
    {
        private readonly DbContextOptions<RealEstateAppContext> _dbOptions;

        public MessageTests()
        {
            // Cada test usa una base en memoria única para evitar contaminación entre pruebas
            _dbOptions = new DbContextOptionsBuilder<RealEstateAppContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Messages_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task SendMessageAsync_Should_Add_Message_To_Database()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var repository = new MessageRepository(context);
            var message = new Message
            {
                PropertyId = 1,
                SenderId = "client1",
                ReceiverId = "agent1",
                Content = "Estoy interesado en la propiedad"
            };

            // Act
            var result = await repository.SendMessageAsync(message);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            var messages = await context.Messages.ToListAsync();
            messages.Should().ContainSingle();
        }

        [Fact]
        public async Task GetClientsByPropertyAsync_Should_Return_Distinct_Senders()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var messages = new List<Message>
        {
            new Message { PropertyId = 1, SenderId = "client1", ReceiverId = "agent1", Content = "Hola" },
            new Message { PropertyId = 1, SenderId = "client2", ReceiverId = "agent1", Content = "Buenas" },
            new Message { PropertyId = 1, SenderId = "client1", ReceiverId = "agent1", Content = "Otra vez yo" }
        };
            context.Messages.AddRange(messages);
            await context.SaveChangesAsync();
            var repository = new MessageRepository(context);

            // Act
            var result = await repository.GetClientsByPropertyAsync(1);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(new[] { "client1", "client2" });
        }

        [Fact]
        public async Task GetConversationAsync_Should_Return_Messages_In_Order()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var messages = new List<Message>
        {
            new Message { PropertyId = 1, SenderId = "agent1", ReceiverId = "client1", Content = "Hola cliente", SentAt = DateTime.UtcNow },
            new Message { PropertyId = 1, SenderId = "client1", ReceiverId = "agent1", Content = "Hola agente", SentAt = DateTime.UtcNow.AddMinutes(1) }
        };
            context.Messages.AddRange(messages);
            await context.SaveChangesAsync();
            var repository = new MessageRepository(context);

            // Act
            var result = await repository.GetConversationAsync(1, "agent1", "client1");

            // Assert
            result.Should().HaveCount(2);
            result.First().Content.Should().Be("Hola cliente");
            result.Last().Content.Should().Be("Hola agente");
        }

        [Fact]
        public async Task GetMessagesByAgentAsync_Should_Return_Agent_Messages()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var messages = new List<Message>
        {
            new Message { PropertyId = 1, SenderId = "agent1", ReceiverId = "client1", Content = "Mensaje del agente" },
            new Message { PropertyId = 1, SenderId = "client1", ReceiverId = "agent1", Content = "Mensaje del cliente" }
        };
            context.Messages.AddRange(messages);
            await context.SaveChangesAsync();
            var repository = new MessageRepository(context);

            // Act
            var result = await repository.GetMessagesByAgentAsync(1, "agent1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Content.Should().Be("Mensaje del agente");
        }

        [Fact]
        public async Task GetMessagesByClientAsync_Should_Return_Client_Messages()
        {
            // Arrange
            using var context = new RealEstateAppContext(_dbOptions);
            var messages = new List<Message>
        {
            new Message { PropertyId = 1, SenderId = "client1", ReceiverId = "agent1", Content = "Mensaje del cliente" },
            new Message { PropertyId = 1, SenderId = "agent1", ReceiverId = "client1", Content = "Mensaje del agente" }
        };
            context.Messages.AddRange(messages);
            await context.SaveChangesAsync();
            var repository = new MessageRepository(context);

            // Act
            var result = await repository.GetMessagesByClientAsync(1, "client1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Content.Should().Be("Mensaje del cliente");
        }
    }
}
