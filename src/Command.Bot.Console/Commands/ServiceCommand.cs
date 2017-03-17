using System.Reflection;
using System.Threading.Tasks;
using Command.Bot.Core;
using Command.Bot.Core.Properties;
using log4net;
using System;
using System.Threading;

namespace Command.Bot.Console.Commands
{


	public class ServiceCommand : ServiceCommandBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private SlackService _slackService;
        private Timer _timer;

	    public ServiceCommand()
			: base("Command.BotService")
		{
			IsCommand("service", string.Format("Bot running as service"));
		}

		#region Overrides of ServiceCommandBase

		protected override void StartService()
		{
            _log.Info("Starting service");
            _log.InfoFormat("BaseDirectory: {0}", AppDomain.CurrentDomain.BaseDirectory);

            _slackService = new SlackService(Settings.Default.BotKey);

            _slackService.Connect().ContinueWith(Connected).ContinueWith((obj) =>
            {
                _log.Info("Starting check service");
                _timer = new Timer(PeriodCheckService, null, 5 * 60 * 1000, 5 * 60 * 1000);
            });
        }

	    private void Connected(Task obj)
	    {
	        if (obj.Exception != null)
	        {
                _log.Error(obj.Exception.Message, obj.Exception);
	        }
        }

        private void PeriodCheckService(object state)
        {

            _slackService.Disconnect();
            _slackService.Connect().ContinueWith(Connected);
            _log.Info("Check service connection.");
        }

        protected override void StopService()
		{
            _log.Info("Stop the service");
		}

		#endregion

		
	}
	
}