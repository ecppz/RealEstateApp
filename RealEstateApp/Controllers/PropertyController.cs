using Application.Dtos.Property;
using Application.Services;
using Application.ViewModels.Message;
using Application.ViewModels.Offer;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RealEstateApp.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IMessageService messageService;
        private readonly IOfferService offerService;
        private readonly IMapper mapper;
        public PropertyController(IPropertyService propertyService, IMapper mapper, IOfferService offerService, IMessageService messageService)
        {
            this.propertyService = propertyService;
            this.messageService = messageService;
            this.offerService = offerService;
            this.mapper = mapper;
        }


        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Index()
        {
            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var properties = await propertyService.GetProperties(agentId, onlyAvailable: false);
            return View(properties); // IEnumerable<PropertyDto>
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> IndexClient()
        {
            // agentId = null porque no estamos filtrando por agente
            var properties = await propertyService.GetAvailablePropertiesAsync();
            return View(properties);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PropertyDetailClient(int propertyId)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var property = await propertyService.GetPropertyById(propertyId);
            if (property == null)
                return NotFound();

            // Conversación con el agente
            var conversation = await messageService.GetConversationAsync(propertyId, property.AgentId, clientId);

            // Ofertas del cliente
            var offers = await offerService.GetOffersByClientAsync(clientId);
            var propertyOffers = offers.Where(o => o.PropertyId == propertyId).ToList();

            var vm = new PropertyDetailClientViewModel
            {
                Id = property.Id,
                Code = property.Code,
                Price = property.Price,
                Description = property.Description,
                SizeInMeters = property.SizeInMeters,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Status = property.Status,
                AgentId = property.AgentId,
                Messages = conversation?.Messages.Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    PropertyId = m.PropertyId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsFromAgent = m.SenderId == property.AgentId
                }).ToList() ?? new List<MessageViewModel>(),
                Offers = mapper.Map<List<OfferViewModel>>(propertyOffers)
            };

            return View(vm);
        }


    }
}
