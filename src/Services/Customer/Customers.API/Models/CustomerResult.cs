using CodeChallenge.DataObjects.Customers;
using CodeChallenge.DataObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services.Customers.Api.Models
{
    public class CustomerAccountResult
    {
        private int _rows = 0;
        private IQueryable<CustomerAccount> _customers;
        public CustomerAccountResult(IQueryable<CustomerAccount> customers, int rows)
        {
            _customers = customers;
            _rows = rows;
        }

        public CustomerAccountResult(IQueryable<CustomerAccount> customers)
        {
            _customers = customers;
        }

        public void ApplySearchFilter(string q)
        {
            _customers = _customers.Where(c =>
                EF.Functions.Like(c.AccountName, $"%{q}%")
            );
        }

        public async Task<ResultSet<CustomerAccount>> GetItemsByPageAsync(int page)
        {
            var totalItems = await _customers.CountAsync();
            var itemsOnPage = _customers.Skip((page - 1) * this._rows).Take(this._rows).ToListAsync();
            var resultset = new ResultSet<CustomerAccount>(page, _rows, totalItems, await itemsOnPage);
            return resultset;
        }

        public async Task<List<CustomerAccount>> GetAllItemsAsync()
        {
            return await _customers.ToListAsync();
        }
    }
}
