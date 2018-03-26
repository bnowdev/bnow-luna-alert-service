using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alert.API.Data;
using Alert.API.Models;
using Alert.API.Repositories.Helpers;
using Alert.API.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace Alert.API.Repositories
{
    public class AlertsRepository: IRepository<Models.Alert>
    {
        private readonly AlertDbContext _ctx;

        public AlertsRepository(AlertDbContext dbContext)
        {
            _ctx = dbContext;
        }

        public void Add(Models.Alert item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Alert> GetAll()
        {
            return _ctx.Alert.Include(alert => alert.AlertConclusion)
                .Include(alert => alert.AlertExplanation)
                .Include(alert => alert.AlertSolution)
                .Include(alert => alert.MonitoredDevice).AsNoTracking();
        }

        public Models.Alert GetSingle()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Models.Alert> GetFiltered(string query)
        {
            try
            {
                var alerts = from allAlerts in _ctx.Alert select allAlerts;

                if (!String.IsNullOrWhiteSpace(query))
                {
                    alerts = AlertFilterBuilder.GetFilteredQueryable(alerts, query);
                }

                return alerts;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }

        #region IDisposable Support
        private bool disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}
