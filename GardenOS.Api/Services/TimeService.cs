using Shared.Models;

namespace MainService.Services
{
	public class TimeService
	{
		public List<TimeEvent> _Events = new();
		private bool _IsTimerRunning = false;

		public void SetUpDailyTimer(TimeSpan executeAt, Action method)
		{
			if (!_IsTimerRunning)
				Task.Factory.StartNew(Start);

			_Events.Add(new TimeEvent(executeAt, method));
		}

		private void Start()
		{
			_IsTimerRunning = true;
			while (_IsTimerRunning)
			{
				List<TimeEvent> toBeExecuted = _Events.Where(e => e.ExecuteAt < DateTime.Now).ToList();
				foreach (TimeEvent timeEvent in toBeExecuted)
				{
					Console.WriteLine(timeEvent);
					timeEvent.Execute();
					_Events.Remove(timeEvent);
					_Events.Add(new TimeEvent(timeEvent.ExecuteTimeSpan, timeEvent.Method));
				}
				// wait 30 sec
				Thread.Sleep(5000);
			}
		}
	}
}