using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alert.API.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Sockets;


namespace Alert.API.Services.SignalR
{
    public class AlertHub : Hub<IAlertHubClient>
    {

        public async void Send(AlertDTO alert)
        {
            await Clients.All.SendAlert(alert);
        }




//  private static HubContext<AlertHub> hubContext = new HubContext<AlertHub>(new DefaultHubLifetimeManager<AlertHub>());

//        public Task Send(string message)
//        {
//            string timestamp = DateTime.Now.ToShortTimeString();
//            return Clients.All.SendAsync(timestamp, message);
//
//        }

//        public static async Task Send(string message)
//        {
//            string timestamp = DateTime.Now.ToShortTimeString();
//           // await hubContext.Clients.All.SendAsync(timestamp, message);
//        }

    }
}
