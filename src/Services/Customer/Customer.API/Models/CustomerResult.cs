using CodeChallenge.DataObjects.Customers;
using CodeChallenge.DataObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services.Customers.Api.Models
{
    public class CustomerResult
    {
        private int _rows = 0;
        private IQueryable<Customer> _customers;
        public CustomerResult(IQueryable<Customer> customers, int rows)
        {
            _customers = customers;
            _rows = rows;
        }

        public CustomerResult(IQueryable<Customer> customers)
        {
            _customers = customers;
        }

        public void ApplySearchFilter(string q)
        {
            _customers = _customers.Where(c =>
                EF.Functions.Like(c.Name, $"%{q}%")
                || EF.Functions.Like(c.Lastname, $"%{q}%")
            );
        }

        public async Task<ResultSet<Customer>> GetItemsByPageAsync(int page)
        {
            var totalItems = await _customers.CountAsync();
            var itemsOnPage = _customers.Skip((page - 1) * this._rows).Take(this._rows).ToListAsync();
            var resultset = new ResultSet<Customer>(page, _rows, totalItems, await itemsOnPage);
            return resultset;
        }

        public async Task<List<Customer>> GetAllItemsAsync()
        {
            return await _customers.ToListAsync();
        }
    }
}
