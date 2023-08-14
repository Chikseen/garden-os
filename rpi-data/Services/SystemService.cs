using System.Diagnostics;

public static class SystemService
{
    public static void Reboot()
    {
        Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "reboot" });
    }
}