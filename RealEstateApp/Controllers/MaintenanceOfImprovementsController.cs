using Application.Dtos.Improvement;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Improvement;
using AutoMapper;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.Controllers
{
    public class MaintenanceOfImprovementsController : Controller
    {

        private readonly IImprovementService improvementService;
        private readonly IPropertyImprovement propertyImprovement;
     //   private readonly IPropertyService propertyService;
        private readonly IMapper mapper;
        private readonly UserManager<UserAccount> userManager;
        private readonly IUserAccountServiceForWebApp userAccountServiceForWebApp;

        public MaintenanceOfImprovementsController(IImprovementService improvementService, IPropertyImprovement propertyImprovement, 
            IMapper mapper, UserManager<UserAccount> userManager, IUserAccountServiceForWebApp userAccountServiceForWebApp)
        {
            this.improvementService = improvementService;
            this.propertyImprovement = propertyImprovement;
          //  this.propertyService = propertyService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.userAccountServiceForWebApp = userAccountServiceForWebApp;
        }

        // GET: Listado de mejoras
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtoList = await improvementService.GetAllList();
            var viewModelList = mapper.Map<List<ImprovementViewModel>>(dtoList);
            return View(viewModelList);
        }

        // GET: Crear mejora
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear mejora
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImprovementCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = mapper.Map<ImprovementCreateDto>(viewModel);
            await improvementService.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Editar mejora
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await improvementService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<ImprovementEditViewModel>(dto);
            return View(viewModel);
        }

        // POST: Editar mejora
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImprovementEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = mapper.Map<ImprovementUpdateDto>(viewModel);
            await improvementService.UpdateAsync(id, dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Confirmar eliminación
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await improvementService.GetById(id);
            if (dto == null)
                return NotFound();

            var viewModel = mapper.Map<ImprovementDeleteViewModel>(dto);
            return View(viewModel);
        }

        // POST: Eliminar mejora
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await improvementService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
