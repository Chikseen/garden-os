using System.Text.Json.Serialization;

public class TimeFrame
{
  [JsonInclude]
  [JsonPropertyName("start")]
  public DateTime Start;

  [JsonInclude]
  [JsonPropertyName("end")]
  public DateTime End;

  public TimeFrame()
  {
    this.Start = StartOfDay(DateTime.Now);
    this.End = EndOfDay(DateTime.Now);
  }

  public TimeFrame(DateTime start, DateTime end)
  {
    this.Start = start;
    this.End = end;
  }

  private DateTime StartOfDay(DateTime theDate)
  {
    return theDate.Date;
  }

  private DateTime EndOfDay(DateTime theDate)
  {
    return theDate.Date.AddDays(1).AddTicks(-1);
  }
}

public static class DateExtensions
{
  public static String ConvertToPGString(this DateTime date)
  {
    return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
  }
}