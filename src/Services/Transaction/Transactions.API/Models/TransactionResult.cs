using CodeChallenge.DataObjects.Transactions;
using CodeChallenge.DataObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services.Transactions.Api.Models
{
    public class TransactionResult
    {
        private int _rows = 0;
        private IQueryable<Transaction> _transactions;
        public TransactionResult(IQueryable<Transaction> transactions, int rows)
        {
            _transactions = transactions;
            _rows = rows;
        }

        public TransactionResult(IQueryable<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public void ApplyTypeFilter(int typeId)
        {
            _transactions = _transactions.Where(t => t.TransactionTypeId == typeId);
        }

        public void ApplyAccountFilter(string accountNumber)
        {
            _transactions = _transactions.Where( t =>
                t.AccountNumber == accountNumber ||
                t.RecieverAccountNumber == accountNumber
            );
        }

        public async Task<ResultSet<Transaction>> GetItemsByPageAsync(int page)
        {
            var totalItems = await _transactions.CountAsync();
            var itemsOnPage = _transactions.Skip((page - 1) * this._rows).Take(this._rows).ToListAsync();
            var resultset = new ResultSet<Transaction>(page, _rows, totalItems, await itemsOnPage);
            return resultset;
        }

        public async Task<List<Transaction>> GetAllItemsAsync()
        {
            return await _transactions.ToListAsync();
        }
    }
}
