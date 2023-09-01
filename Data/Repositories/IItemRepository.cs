using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandleApi.Models;

namespace CandleApi.Data.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemDto>> GetItems();
        Task<ItemDto> GetItem(Guid id);
        Task<Item?> GetItemByName(string name);
        Task<ItemDto> AddItem(ItemDto itemDto);
        Task<ItemDto> UpdateItem(Guid id, ItemDto item);
        Task<Item?> DeleteItem(Guid id);
    }
}