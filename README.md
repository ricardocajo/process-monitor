# process-monitor

A tiny C# utility to monitor Windows processes and kill the processes that work longer than the threshold specified.

This command line utility expects three arguments: a process name, its maximum lifetime (in minutes) and a monitoring frequency (in minutes, aswell). 
When you run the program, it starts monitoring processes with the frequency specified. 
If a process of interest lives longer than the allowed duration, the utility kills the process and adds the corresponding record to the log. 
When no process exists at any given moment, the utility continues monitoring (new processes might appear later). 
The utility stops when a special keyboard button is pressed (say, q).

Here is the example: monitor.exe notepad 5 1 
Every other minute, the program verifies if a notepad process lives longer than 5 minutes, and if it does, the program kills the process.
