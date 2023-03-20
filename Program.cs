using System.Diagnostics;
using System.Text.RegularExpressions;
class Program 
{
    static void Main(string[] args)
    {
        if(args.Length == 3)
        {
            // Maximum lifetime of a process
            double process_lifetime;
            // Monitoring frequency
            double frequency;
            // Process name
            string process_name = args[0];
            // Regex of a valid process name
            Regex process_regex = new Regex(".*"); //TODO

            // Validate input
            if(!double.TryParse(args[1], out process_lifetime))
            {
                // 2nd argument is not a number
                Console.WriteLine(args[1] + " is not a valid value for a maximum lifetime (in minutes)");
            }
            else if(!double.TryParse(args[2], out frequency))
            {
                // 3rd argument is not a number
                Console.WriteLine(args[2] + " is not a valid value for a monitoring frequency (in minutes)");
            }
            else if(!process_regex.IsMatch(process_name))
            {
                // Process name is not valid
                Console.WriteLine(process_name + " is not a valid process name");
            }
            else
            {
                // Input is good, start monitoring

                // Create a timer to run the processMonitoring function at the specified frequency 
                System.Timers.Timer frequency_timer = new System.Timers.Timer((int)(frequency * 60000));
                frequency_timer.Elapsed += (sender, e) => { if (sender != null) { processMonitoring(sender, e, process_name, process_lifetime); } };

                // Do one monitoring at start
                processMonitoring(null, null, process_name, process_lifetime);

                frequency_timer.Enabled = true;

                Console.WriteLine("Press \"q\" to exit");
                
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.KeyChar == 'q')
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            // Wrong number of command line arguments
            Console.WriteLine("Wrong command line arguments.\n Three arguments are expected:\n 1 Process name \n 2 Maximum lifetime (in minutes) \n 3 Monitoring frequency (in minutes)");
        }
    }

    static void processMonitoring(Object source, System.Timers.ElapsedEventArgs e, string process_name, double process_lifetime)
    {
        // Get list of processes with given name
        Process[] allProcesses = Process.GetProcessesByName(process_name);
    
        foreach (Process process in allProcesses) 
        {
            if((DateTime.Now - process.StartTime).TotalMinutes > process_lifetime)
            {
                // The process is alive for more than the specified time

                // Log this process (will be killed)    
                Console.WriteLine(process.ProcessName + " :  Closed at " + DateTime.Now);        
                using(StreamWriter sw = File.AppendText("log.txt"))
                {
                    sw.WriteLine(process.ProcessName + " :  Closed at " + DateTime.Now);               
                }

                // Kill the process
                process.Kill();
            }
        }
    }
}
