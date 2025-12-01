using Application.Dtos.SaleType;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.SaleType;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Controllers
{
    public class SalesTypeMaintenanceController : Controller
    {
        private readonly IPropertyService PropertyService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;
        private readonly ISaleTypeService saleTypeService;


        public SalesTypeMaintenanceController(IPropertyService propertyService, IMapper mapper, UserManager<UserAccount> userManager, IUserAccountServiceForWebApp userAccountServiceForWebApp, ISaleTypeService saleTypeService)
        {
            PropertyService = propertyService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
            this.saleTypeService = saleTypeService;
        }

        // GET: Listado de tipos de venta
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var saleTypes = await saleTypeService.GetAllList();
            var viewModel = mapper.Map<List<SaleTypeListViewModel>>(saleTypes);
            return View(viewModel);
        }

        // GET: Mostrar formulario de creación de tipo de venta
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SaleTypeCreateViewModel());
        }

        // POST: Crear un nuevo tipo de venta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleTypeCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = mapper.Map<SaleTypeCreateDto>(model);
            await saleTypeService.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Mostrar formulario de edición de tipo de venta
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await saleTypeService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<SaleTypeEditViewModel>(dto);
            return View(viewModel);
        }

        // POST: Guardar cambios en un tipo de venta existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaleTypeEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = mapper.Map<SaleTypeUpdateDto>(model);
            await saleTypeService.UpdateAsync(dto, model.Id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Mostrar confirmación de eliminación de tipo de venta
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await saleTypeService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<SaleTypeListViewModel>(dto);
            return View(viewModel);
        }

        // POST: Confirmar y ejecutar la eliminación de tipo de venta
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await saleTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
