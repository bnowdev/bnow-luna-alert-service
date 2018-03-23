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

//
//        public IEnumerable<AlertDTO> GetAlerts()
//        {
//            try
//            {
//                return _ctx.Alert
//                    .Include(alert => alert.AlertConclusion)
//                    .Include(alert => alert.AlertExplanation)
//                    .Include(alert => alert.AlertSolution)
//                    .Include(alert => alert.MonitoredDevice)
//                    .Select(a => new AlertDTO
//                    {
//                       Id = a.Id,
//                        Name = a.Name,
//                        Severity = a.Severity,
//                        Description = a.Description,
//                        Priority = a.Priority,
//                        Source = a.Source,
//                        TimeGenerated = a.TimeGenerated,
//                        AlertSolution = new AlertSolutionDTO
//                        {
//                            Id = a.AlertSolution.Id,
//                            Text = a.AlertSolution.Text
//                        },
//                        AlertExplanation = new AlertExplanationDTO()
//                        {
//                            Id = a.AlertExplanation.Id,
//                            Text = a.AlertExplanation.Text
//                        },
//                        AlertConclusion = new AlertConclusionDTO()
//                        {
//                            Id = a.AlertConclusion.Id,
//                            Text = a.AlertConclusion.Text
//                        },
//                        MonitoredDevice = new MonitoredDeviceDTO()
//                        {
//                            Id = a.MonitoredDevice.Id,
//                            Name = a.MonitoredDevice.Name,
//                            CompanyId = a.MonitoredDevice.CompanyId
//
//                        },
//                    })
//                    .AsNoTracking();
//
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }
//        }

//        public async Task<IEnumerable<AlertDTO>> GetAlertsAsync(string query, string sortBy, int pageSize, int pageIndex)
//        {
//            var alerts = from allAlerts in _ctx.Alert select allAlerts;
//
//
//            // filtering alerts
//            if (!String.IsNullOrWhiteSpace(query))
//            {
//                alerts = AlertFilterBuilder.GetFilteredQueryable(alerts, query);
//            }
//
//            var filteredAlertsCount = alerts.Count();
//
//
//            // Ordering filtered alerts
//            // TODO add other properties
//            if (sortBy.EndsWith("DESC"))
//            {
//                alerts = alerts.OrderByDescending(a => a.TimeGenerated);
//            }
//            else
//            {
//                alerts = alerts.OrderBy(a => a.TimeGenerated);
//            }
//
//            // Limit the amount of returned alerts 
//            alerts = alerts
//                .Skip(pageSize * pageIndex)
//                .Take(pageSize);
//
//
//
//            // Cast to Alerts to AlertDTOs
//            var alertDtos = alerts
//                .Include(alert => alert.AlertConclusion)
//                .Include(alert => alert.AlertExplanation)
//                .Include(alert => alert.AlertSolution)
//                .Include(alert => alert.MonitoredDevice)
//                .Select(a => new AlertDTO
//                {
//                    Id = a.Id,
//                    Name = a.Name,
//                    Severity = a.Severity,
//                    Description = a.Description,
//                    Priority = a.Priority,
//                    Source = a.Source,
//                    TimeGenerated = a.TimeGenerated,
//                    AlertSolution = new AlertSolutionDTO
//                    {
//                        Id = a.AlertSolution.Id,
//                        Text = a.AlertSolution.Text
//                    },
//                    AlertExplanation = new AlertExplanationDTO()
//                    {
//                        Id = a.AlertExplanation.Id,
//                        Text = a.AlertExplanation.Text
//                    },
//                    AlertConclusion = new AlertConclusionDTO()
//                    {
//                        Id = a.AlertConclusion.Id,
//                        Text = a.AlertConclusion.Text
//                    },
//                    MonitoredDevice = new MonitoredDeviceDTO()
//                    {
//                        Id = a.MonitoredDevice.Id,
//                        Name = a.MonitoredDevice.Name,
//                        CompanyId = a.MonitoredDevice.CompanyId
//
//                    },
//                })
//                .AsNoTracking();
//
//            await alertDtos.ToListAsync();
//
//            var model = new PaginatedItemsViewModel<AlertDTO>(
//                pageIndex, pageSize, filteredAlertsCount, alertDtos
//            );
//
//            return model;
//
//        }

  

      

  

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
