namespace shared_data.Models
{
	public class TimeEvent
	{
		public string Id;
		public DateTime ExecuteAt;
		public TimeSpan ExecuteTimeSpan;
		public bool Repeat;
		public readonly Action Method;

		public TimeEvent(TimeSpan executeAt, Action method, bool repeat = true)
		{
			TimeSpan timeToGo = executeAt - DateTime.Now.TimeOfDay;
			if (timeToGo < TimeSpan.Zero)
			{
				timeToGo += new TimeSpan(24, 0, 0); // assign it as "+" since the time to go will be negative
			}

			ExecuteAt = DateTime.Now + timeToGo;
			ExecuteTimeSpan = executeAt;
			Repeat = repeat;
			Id = Guid.NewGuid().ToString();
			Method = method;
		}

		public void Execute()
		{
			Method();
		}
	}
}