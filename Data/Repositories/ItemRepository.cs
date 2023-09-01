using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CandleApi.Models;

namespace CandleApi.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ItemRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            var items = await _appDbContext.Items.ToListAsync();
            return items.Select(_mapper.Map<ItemDto>);
        }

        public async Task<ItemDto> GetItem(Guid id)
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(i => i.ItemId == id);
            return _mapper.Map<ItemDto>(item);
        }

        public async Task<Item?> GetItemByName(string name) 
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(i => i.Name == name);
            return item;
        }

        public async Task<ItemDto> AddItem(ItemDto itemDto)
        {
            var item = new Item() {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                ImageUrl = itemDto.ImageUrl,
                OrderItems = new List<OrderItem>()
            };

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<ItemDto>(item);
        }

        public async Task<ItemDto> UpdateItem(Guid id, ItemDto newItem) 
        {
            var itemToUpdate = await _appDbContext.Items.FirstOrDefaultAsync(i => i.ItemId == id);
            if (itemToUpdate != null) {
                itemToUpdate.Name = newItem.Name;
                itemToUpdate.Description = newItem.Description;
                itemToUpdate.Price = newItem.Price;
                itemToUpdate.ImageUrl = newItem.ImageUrl;
                _appDbContext.SaveChanges();
            }

            return _mapper.Map<ItemDto>(itemToUpdate);
        }

        public async Task<Item?> DeleteItem(Guid id) 
        {
            var itemToDelete = await _appDbContext.Items.FirstOrDefaultAsync(i => i.ItemId == id);

            if (itemToDelete != null)
            {
                _appDbContext.Items.Remove(itemToDelete);
                _appDbContext.SaveChanges();
                return itemToDelete;
            }
            else
            {
                return null;
            }
        }
    }
}