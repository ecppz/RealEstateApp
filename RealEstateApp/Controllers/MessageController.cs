using Application.Dtos.Message;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Message;
using Application.ViewModels.Property;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IMessageService messageService;
        private readonly IBaseAccountService baseAccountService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public MessageController(IPropertyService propertyService, IMessageService messageService, IBaseAccountService baseAccountService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.propertyService = propertyService;
            this.messageService = messageService;
            this.baseAccountService = baseAccountService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // Listado de clientes que han conversado sobre una propiedad
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Clients(int propertyId)
        {
            var agentId = userManager.GetUserId(User); // 🔹 obtener el GUID del agente logueado

            var clients = await messageService.GetClientsByPropertyAsync(propertyId, agentId); // 🔹 pasar agentId

            // Mapeo manual a ViewModel
            var vm = clients.Select(c => new ChatClientViewModel
            {
                ClientId = c.ClientId,
                ClientName = c.ClientName,
                PropertyId = c.PropertyId,
                LastMessageAt = c.LastMessageAt
            }).ToList();

            ViewBag.PropertyId = propertyId;
            return View(vm);
        }

        // 🔹 Ver conversación entre agente y cliente
        public async Task<IActionResult> Conversation(int propertyId, string clientId)
        {
            var agentId = userManager.GetUserId(User); // agente logueado
            var conversation = await messageService.GetConversationAsync(propertyId, agentId, clientId);

            if (conversation == null)
                return NotFound();

            // Mapeo manual a ViewModel
            var vm = new ConversationViewModel
            {
                PropertyId = conversation.PropertyId,
                AgentId = conversation.AgentId,
                ClientId = conversation.ClientId,
                ClientName = conversation.ClientName,
                Messages = conversation.Messages.Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    PropertyId = m.PropertyId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    Property = mapper.Map<PropertyViewModel>(m.Property),
                    IsFromAgent = m.SenderId == agentId
                }).ToList()
            };

            return View(vm);
        }

        // 🔹 Enviar mensaje en la conversación
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Conversation", new { propertyId = model.PropertyId, clientId = model.ReceiverId });

            var agentId = userManager.GetUserId(User);

            var dto = new SendMessageDto
            {
                PropertyId = model.PropertyId,
                SenderId = agentId,
                ReceiverId = model.ReceiverId,
                Content = model.Content
            };

            await messageService.SendMessageAsync(dto);

            return RedirectToAction("Conversation", new { propertyId = model.PropertyId, clientId = model.ReceiverId });
        }


        //CLIENTE:

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConversationClient(int propertyId)
        {
            var clientId = userManager.GetUserId(User); // cliente logueado
            var property = await propertyService.GetPropertyById(propertyId);
            if (property == null)
                return NotFound();

            var agentId = property.AgentId; // agente dueño de la propiedad
            var conversation = await messageService.GetConversationAsync(propertyId, agentId, clientId);

            if (conversation == null)
                return NotFound();

            var vm = new ConversationViewModel
            {
                PropertyId = conversation.PropertyId,
                AgentId = conversation.AgentId,
                ClientId = conversation.ClientId,
                ClientName = conversation.ClientName,
                Messages = conversation.Messages.Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    PropertyId = m.PropertyId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    Property = mapper.Map<PropertyViewModel>(m.Property),
                    IsFromAgent = m.SenderId == agentId
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SendMessageClient(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("ConversationClient", new { propertyId = model.PropertyId });

            var clientId = userManager.GetUserId(User); // cliente logueado
            var property = await propertyService.GetPropertyById(model.PropertyId);
            if (property == null)
                return NotFound();

            var agentId = property.AgentId; // receptor es el agente dueño de la propiedad

            var dto = new SendMessageDto
            {
                PropertyId = model.PropertyId,
                SenderId = clientId,
                ReceiverId = agentId,
                Content = model.Content
            };

            await messageService.SendMessageAsync(dto);

            return RedirectToAction("ConversationClient", new { propertyId = model.PropertyId });
        }

    }
}
