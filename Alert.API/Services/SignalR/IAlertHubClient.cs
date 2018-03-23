using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alert.API.Dto;

namespace Alert.API.Services.SignalR
{
    public interface IAlertHubClient
    {
        Task SendAlert(AlertDTO alert);
    }

}
