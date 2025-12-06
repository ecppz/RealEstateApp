using Application.Dtos.Offer;
using Application.Dtos.Property;
using Application.Dtos.User;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Offer;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RealEstateApp.Controllers
{
    public class OfferController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IOfferService offerService;
        private readonly IBaseAccountService baseAccountService;
        private readonly IMessageService messageService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;

        public OfferController(IPropertyService propertyService, IMessageService messageService, IOfferService offerService, IBaseAccountService baseAccountService, IMapper mapper, UserManager<UserAccount> userManager)
        {
            this.propertyService = propertyService;
            this.offerService = offerService;
            this.baseAccountService = baseAccountService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.messageService = messageService;
        }

        // Listado de ofertas de una propiedad (vista del agente)
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> PropertyOffers(int propertyId)
        {
            var offers = await offerService.GetOffersByPropertyAsync(propertyId);
            var vm = mapper.Map<List<OfferViewModel>>(offers);
            return View(vm);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClientOffers()
        {
            var clientId = userManager.GetUserId(User); // 🔹 ya es string
            var offers = await offerService.GetOffersByClientAsync(clientId); // 🔹 método espera string
            var vm = mapper.Map<List<OfferViewModel>>(offers);
            return View(vm);
        }

        // Crear nueva oferta (cliente)
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(CreateOfferViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientId = userManager.GetUserId(User);

            var dto = new CreateOfferDto
            {
                PropertyId = model.PropertyId,
                ClientId = clientId,
                Amount = model.Amount
            };

            var created = await offerService.CreateOfferAsync(dto);

            if (created == null)
                return BadRequest("No se pudo crear la oferta.");

            return RedirectToAction("ClientOffers");
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Accept(int offerId, int propertyId)
        {
            if (offerId <= 0 || propertyId <= 0)
                return BadRequest("Parámetros inválidos.");

            var result = await offerService.AcceptOfferAsync(offerId);
            if (!result)
            {
                TempData["ErrorMessage"] = "No se pudo aceptar la oferta. Verifique que esté pendiente y que la propiedad esté disponible.";
                return RedirectToAction("PropertyOffers", new { propertyId });
            }

            TempData["SuccessMessage"] = "Oferta aceptada correctamente. La propiedad ha sido marcada como vendida y las demás ofertas pendientes fueron rechazadas.";
            return RedirectToAction("PropertyOffers", new { propertyId });
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Reject(int offerId, int propertyId)
        {
            if (offerId <= 0 || propertyId <= 0)
                return BadRequest("Parámetros inválidos.");

            var result = await offerService.RejectOfferAsync(offerId);
            if (!result)
            {
                TempData["ErrorMessage"] = "No se pudo rechazar la oferta. Verifique que esté pendiente.";
                return RedirectToAction("PropertyOffers", new { propertyId });
            }

            TempData["SuccessMessage"] = "Oferta rechazada correctamente.";
            return RedirectToAction("PropertyOffers", new { propertyId });
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> PropertyOffersByClient(int propertyId, string clientId)
        {
            // Traer todas las ofertas de la propiedad
            var offers = await offerService.GetOffersByPropertyAsync(propertyId);

            // Filtrar solo las ofertas del cliente indicado
            var clientOffers = offers.Where(o => o.ClientId == clientId).ToList();

            if (clientOffers == null || !clientOffers.Any())
                return NotFound();

            var vm = mapper.Map<List<OfferViewModel>>(clientOffers);

            ViewBag.PropertyId = propertyId;
            ViewBag.ClientId = clientId;

            return View(vm);
        }



        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> PropertyDetailAgent(int? propertyId)
        {
            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Si no viene propertyId, resolvemos destino
            if (propertyId is null)
            {
                var properties = await propertyService.GetProperties(agentId, onlyAvailable: false);

                if (properties == null || properties.Count == 0)
                    return RedirectToAction("Index", "Property"); // sin propiedades → listado (vacío o aviso)

                // Si hay una sola propiedad, usamos esa; si hay varias, llevamos al listado
                if (properties.Count == 1)
                    return RedirectToAction(nameof(PropertyDetailAgent), new { propertyId = properties[0].Id });

                return RedirectToAction("Index", "Property"); // múltiples → el agente elige
            }

            // Traer y validar pertenencia
            var property = await propertyService.GetPropertyById(propertyId.Value);
            if (property == null || property.AgentId != agentId)
                return NotFound();

            var vm = new PropertyDetailAgentViewModel
            {
                Id = property.Id,
                Code = property.Code,
                Price = property.Price,
                Description = property.Description,
                SizeInMeters = property.SizeInMeters,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Status = property.Status
            };

            // Mensajes
            // Mensajes
            var clientsWithMessages = await messageService.GetClientsByPropertyAsync(property.Id, agentId);
            vm.ClientsWithMessages = clientsWithMessages.Select(c => new ClientSummaryViewModel
            {
                Id = c.ClientId,
                Name = c.ClientName
            }).ToList();

            // Ofertas
            var offers = await offerService.GetOffersByPropertyAsync(property.Id);
            var clientsWithOffers = new List<ClientSummaryViewModel>();
            foreach (var offer in offers)
            {
                var user = await baseAccountService.GetUserById<UserDto>(offer.ClientId.ToString());
                clientsWithOffers.Add(new ClientSummaryViewModel
                {
                    Id = offer.ClientId.ToString(),
                    Name = user?.UserName ?? "Cliente"
                });
            }

            vm.ClientsWithOffers = clientsWithOffers
                .GroupBy(c => c.Id)
                .Select(g => g.First())
                .ToList();

            return View(vm);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PropertyOffersClient(int propertyId)
        {
            var clientId = userManager.GetUserId(User);
            var offers = await offerService.GetOffersByClientAsync(clientId);

            // Filtrar solo las ofertas de este cliente para la propiedad indicada
            var propertyOffers = offers.Where(o => o.PropertyId == propertyId).ToList();

            var vm = mapper.Map<List<OfferViewModel>>(propertyOffers);
            ViewBag.PropertyId = propertyId;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOffer(CreateOfferViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("PropertyOffersClient", new { propertyId = model.PropertyId });

            var clientId = userManager.GetUserId(User);

            var dto = new CreateOfferDto
            {
                PropertyId = model.PropertyId,
                ClientId = clientId,
                Amount = model.Amount
            };

            var created = await offerService.CreateOfferAsync(dto);

            if (created == null)
                return BadRequest("No se pudo crear la oferta.");

            return RedirectToAction("PropertyOffersClient", new { propertyId = model.PropertyId });
        }



    }
}
