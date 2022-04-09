using BaGet.Protocol;
using BlazModular.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazModular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleController : ControllerBase
    {

        private readonly IModuleBusiness moduleBusiness;

        public ModuleController(IModuleBusiness moduleBusiness)
        {
            this.moduleBusiness = moduleBusiness;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetModulesActivatedAsync([FromQuery] string? path = null)
        {
            var packages = await moduleBusiness.GetLoadedAndInstalledModules(path);
            return Ok(packages);
        }

        [HttpGet("loaded")]
        public async Task<IActionResult> GetModulesInstalledAsync([FromQuery] string? path = null)
        {
            var packages = await moduleBusiness.GetLoadedAndInstalledModules(path);
            return Ok(packages);
        }

        [HttpGet]
        public async Task<IActionResult> GetModulesAsync(
            [FromQuery] bool allowBetaChannel = false,
            [FromQuery] bool allowAlphaChannel = false)
        {
            var packages = await moduleBusiness.GetModulesAsync(allowBetaChannel, allowAlphaChannel);
            return Ok(packages);
        }

        [HttpGet("install")]
        public async Task<IActionResult> InstallModuleAsync([FromQuery] string packageName, [FromQuery] string version)
        {
            if (await moduleBusiness.InstallModuleAsync(packageName, version))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
