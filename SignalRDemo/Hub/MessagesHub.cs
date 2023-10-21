using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Hub{

    public interface IMessageHubClient 
    {
        Task SendOffersToUser(List < string > message);
        Task SendDataToUser(string test);        
    }


    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task SendOffersToUser(List<string> message)
        {
            await Clients.All.SendOffersToUser(message);
        }
        public async Task SendDataToUser(string test)
        {
            await Clients.All.SendDataToUser(test);
        }
    }
    

}