using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alert.API.Dto;
using Alert.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Alert.API.Models;
using Alert.API.Repositories;
using Alert.API.Services.SignalR;
using Alert.API.Testing;
using Alert.API.ViewModel;
using Microsoft.AspNetCore.SignalR;

namespace Alert.API.Controllers
{
    [Produces("application/json")]
    [Route("api/alerts")]
    public class AlertsController : Controller
    {
        //private readonly LunaAlertsContext _context;
        private readonly IRepository<Models.Alert> _alertsRepo;
        private readonly IHubContext<AlertHub, IAlertHubClient> _alertHubContext;

        public AlertsController(IRepository<Models.Alert> alertsRepository, IHubContext<AlertHub, IAlertHubClient> alertHubContext)
        {
            _alertsRepo = alertsRepository;
            _alertHubContext = alertHubContext;
        }

        // GET api/Alerts[?query=""&sortBy=timeGeneratedDESC&pageSize=5&pageIndex=0]
        [HttpGet]
        public async Task<IActionResult> GetAlerts([FromQuery] string query="", [FromQuery] string sortBy="timeGeneratedDESC", [FromQuery] int pageSize = 5, [FromQuery] int pageIndex = 0)
        {

            // TODO add validation for the request like BadRequest
            if (pageSize > 50 || pageSize < 1)
            {
                return BadRequest();
            }

            if (pageIndex < 0)
            {
                return BadRequest();
            }

            


            // get the filtered alerts from Repository
            var alerts =  _alertsRepo.GetFiltered(query);

            var alertsCount = await alerts.LongCountAsync();

            // Ordering filtered alerts.
            // Get the value provided in sortBy and try to
            // orderBy the alerts.
            if (sortBy.EndsWith("DESC"))
            {
                var property = sortBy.Remove(sortBy.Length - 4).ToPascalCase();
                alerts = alerts.OrderByDescending(a => EF.Property<object>(a, property));
            }
            else
            {
                var property = sortBy.ToPascalCase();
                alerts = alerts.OrderBy(a => EF.Property<object>(a, property));
            } 

            // Set the page size of the returned alerts 
            alerts = alerts
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            // Transform the alert list into Dto object list.
            // Includes the AlertConclusion, AlertExplanation, AlertSolution, MonitoredDevice.
            // Avoids json ReferenceLoopHandling error
            // and provideds more control over the returned data.
            var alertList = await alerts.Include(alert => alert.AlertConclusion)
                .Include(alert => alert.AlertExplanation)
                .Include(alert => alert.AlertSolution)
                .Include(alert => alert.MonitoredDevice)
                .Select(a => new AlertDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Severity = a.Severity,
                    Description = a.Description,
                    Priority = a.Priority,
                    Source = a.Source,
                    TimeGenerated = a.TimeGenerated,
                    AlertSolution = new AlertSolutionDTO
                    {
                        Id = a.AlertSolution.Id,
                        Text = a.AlertSolution.Text
                    },
                    AlertExplanation = new AlertExplanationDTO()
                    {
                        Id = a.AlertExplanation.Id,
                        Text = a.AlertExplanation.Text
                    },
                    AlertConclusion = new AlertConclusionDTO()
                    {
                        Id = a.AlertConclusion.Id,
                        Text = a.AlertConclusion.Text
                    },
                    MonitoredDevice = new MonitoredDeviceDTO()
                    {
                        Id = a.MonitoredDevice.Id,
                        Name = a.MonitoredDevice.Name,
                        CompanyId = a.MonitoredDevice.CompanyId

                    },
                })
                .AsNoTracking()
                .ToListAsync();



            var model = new PaginatedItemsViewModel<AlertDTO>(
                pageIndex, pageSize, alertsCount, alertList
            );

            
            return Ok(model);
        }



        [Route("message")]
        [HttpPost]
        public async Task<IActionResult> PostMessageToClient([FromBody] Message message)
        {

            //await AlertHub.Send(message.Text);

           // var timestamp = DateTime.Now.ToShortTimeString();
            var msg = message.Text;

            var testAlert = new AlertDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Alert Signalr 1",
                Severity = 1,
                Description = "Lorem ipsum dolor sit amet,consectetur adipiscing elit.Curabitur tristique sit amet felis ac semper.Morbi luctus metus nibh.Pellentesque sodales mattis urna.Aliquam in nisl id quam aliquam malesuada in in velit.Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec vitae libero id quam ultricies volutpat.Fusce gravida est ante. Pellentesque non nisi vel nisl sagittis varius sed semper purus. Maecenas fringilla massa in nisl tincidunt, eget accumsan eros ultrices.Sed quis lorem pharetra, vulputate orci et, vehicula arcu.Etiam eget feugiat libero. Proin ullamcorper non dolor in dignissim.Integer ultrices leo arcu.",
                Priority = 1,
                Source = "signalr computer.local",
                TimeGenerated = DateTime.Now,
                AlertSolution = new AlertSolutionDTO
                {
                    Id = Guid.NewGuid(),
                    Text = "solution Lorem ipsum dolor sit amet,consectetur adipiscing elit"
                },
                AlertExplanation = new AlertExplanationDTO()
                {
                    Id = Guid.NewGuid(),
                    Text = "explanation Lorem ipsum dolor sit amet,consectetur adipiscing elit"
                },
                AlertConclusion = new AlertConclusionDTO()
                {
                    Id = Guid.NewGuid(),
                    Text = "conclusion Lorem ipsum dolor sit amet,consectetur adipiscing elit"
                },
                MonitoredDevice = new MonitoredDeviceDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "pawel signalr server",
                    CompanyId = Guid.NewGuid()

                }
            };

            await _alertHubContext.Clients.All.SendAlert(testAlert);

            return Ok(testAlert);

        }

      
    }
}