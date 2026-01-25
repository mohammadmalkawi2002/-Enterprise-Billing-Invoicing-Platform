using BillingInvoicingPlatform.Application.Interfaces;
using BillingInvoicingPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Infrastructure.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;


        public UnitOfWork(ApplicationDbContext context)
        {
            
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if(_transaction != null)
                return;

           _transaction=  await  _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if(_transaction is null) 
                return;
            await _transaction?.CommitAsync();
               Dispose();
        }

       

        public async Task RollbackAsync()
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync();
            Dispose();
        }

        public void Dispose()
        {
            if(_disposed)
                return;

            _transaction?.Dispose();
            _transaction = null;
            _disposed = true;
        }
    }
}
