using System;
using System.Security.Principal;

namespace tcpkiller
{
    class Program
    {
        static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Disconnect IP1 [IP2 [IP3 [IPn]]]");
                    Console.WriteLine("");
                    Console.WriteLine("Connections up now:");
                    string[] cons = Disconnecter.Connections(Disconnecter.State.Established);
                    foreach (string s in cons)
                    {
                        Console.WriteLine(s);
                    }
                }
                else
                {
                    if (!IsElevated)
                    {
                        Console.WriteLine("The application is running non-elevated.");
                        return;
                    }

                    foreach (string arg in args)
                    {
                        var s = arg.Split(':');
                        if (s.Length == 1)
                            Disconnecter.CloseRemoteIP(s[0]);
                        else if (s.Length == 2)
                            Disconnecter.CloseRemoteIP(s[0], int.Parse(s[1]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}