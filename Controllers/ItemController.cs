using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CandleApi.Data;
using CandleApi.Data.Repositories;
using CandleApi.Models;

namespace CandleApi.Controllers
{
    [Route("[controller]")]
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IItemRepository _itemRepository;

        public ItemController(ILogger<ItemController> logger, AppDbContext appDbContext, IItemRepository itemRepository)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetItems() 
        {
            try {
                return Ok(await _itemRepository.GetItems());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id)
        {
            try
            {
                var result = await _itemRepository.GetItem(id);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ItemDto>> CreateItem(ItemDto itemDto) 
        {
            try
            {
                if (itemDto == null) {
                    return BadRequest("Bad data");
                }

                var item = await _itemRepository.GetItemByName(itemDto.Name);

                if (item != null)
                {
                    ModelState.AddModelError("name", "Item already exists");
                    return BadRequest(ModelState);
                }

                var createdItem = await _itemRepository.AddItem(itemDto);

                return Created(nameof(GetItem), createdItem);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ItemDto>> UpdateItem(Guid id, ItemDto itemDto)
        {
            try
            {
                if (itemDto == null) {
                    return BadRequest("Bad data");
                }

                var itemToUpdate = await _itemRepository.GetItem(id);

                if(itemToUpdate == null)
                {
                    return NotFound($"Item with Id = {id} not found");
                }

                return await _itemRepository.UpdateItem(id, itemDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpDelete("/Item/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Item>> DeleteItem(Guid id)
        {
            try
            {
                var itemToDelete = await _itemRepository.GetItem(id);

                if (itemToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                return await _itemRepository.DeleteItem(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}