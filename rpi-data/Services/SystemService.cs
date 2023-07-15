using System.Diagnostics;

public static class SystemService
{
    public static void Reboot()
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "reboot" });
    }
}