using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Hub;
using System.Timers;

namespace SignalRDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOfferController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHubClient> msghub;
        private System.Timers.Timer _timer;

        public ProductOfferController(IHubContext<MessageHub, IMessageHubClient> _messageHub)
        {
            msghub = _messageHub;
        }
        [HttpPost]
        [Route("productoffers")]
        public string Get()
        {
            List<string> offers = new List<string>();
            offers.Add("20% Off on IPhone 12");
            offers.Add("15% Off on HP Pavillion");
            offers.Add("25% Off on Samsung Smart TV");
            msghub.Clients.All.SendOffersToUser(offers);
            new TimerManager(() => msghub.Clients.All.SendDataToUser("Data is Data"));

            _timer = new System.Timers.Timer(interval:2000);
            _timer.Elapsed += Test;
            _timer.Enabled = true;
            _timer.Start();

            return "Offers sent successfully to all users!";
        }

        /// <summary>
        /// This Function is called using System.Timers.Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("Timer event raised {0}", e.SignalTime);
            msghub.Clients.All.SendDataToUser("Timer Event Raised");
        }
        [HttpGet]
        [Route("TestEnd")]
        private string TestEnd()
        {
            _timer.Stop();
            return "Ok";
        }
    }

    /// <summary>
    /// This class is called using System.Thread.Timer
    /// </summary>
    public class TimerManager
    {
        private System.Threading.Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        public DateTime TimerStarted { get; set; }

        public TimerManager(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new System.Threading.Timer(Execute, _autoResetEvent, 1000, 10000);
            //TimerStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            System.Console.WriteLine("Thread Timer event raised {0}", DateTime.Now);
            
            _action();
        }
    }
}