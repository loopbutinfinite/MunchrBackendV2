using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;
using MunchrBackendV2.Services;

namespace MunchrBackendV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemServices _menuItemServices;

        public MenuItemController(MenuItemServices menuItemServices)
        {
            _menuItemServices = menuItemServices;
        }

        [HttpPost("CreateMenuItem")]
        public async Task<ActionResult> CreateMenuItem([FromBody] CreateMenuItemDTO newItem)
        {
            var result = await _menuItemServices.CreateMenuItem(newItem);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(result.Item);
        }

        [HttpGet("GetMenuItemsByBusinessId/{businessId}")]
        public async Task<ActionResult<IEnumerable<MenuItemModel>>> GetMenuItemsByBusinessId(int businessId)
        {
            var items = await _menuItemServices.GetMenuItemsByBusinessId(businessId);
            return Ok(items);
        }

        [HttpPut("UpdateMenuItem")]
        public async Task<ActionResult> UpdateMenuItem([FromBody] MenuItemDTO item)
        {
            var result = await _menuItemServices.UpdateMenuItem(item);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        [HttpDelete("DeleteMenuItem/{id}")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            var success = await _menuItemServices.DeleteMenuItem(id);
            if (!success)
                return NotFound(new { message = "Menu item not found." });

            return Ok(new { success = true });
        }
    }
}
