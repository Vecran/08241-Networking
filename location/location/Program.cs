using System;
using System.Net.Sockets;
using System.IO;
using System.Net;

/// <summary>
/// Location
/// </summary>
public class Connect
{
    static string Hosting(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-h")
            {
                return args[i + 1];
            }
        }
        return "localhost";
    }
    static int RequestFormat(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-h9")
            {
                return 1;
            }
            else if (args[i] == "-h0")
            {
                return 2;
            }
            else if (args[i] == "h1")
            {
                return 3;
            }
        }
        return 0;
    }
    static string Formatting(int format, string[] args, string host)
    {
        //whois style requests
        if (format == 0)
        {
            if (args.Length == 3 || args.Length == 1)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h")
                    {
                        return args[i];
                    }
                }
                return "FALSE";
            }
            else if (args.Length == 2 || args.Length == 4)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h")
                    {
                        return (args[i] + " " + args[i + 1]);
                    }
                }
                return "FALSE";
            }
            else return "FALSE";
        }
        else if (format == 1) // http 0.9 style requests
        {
            if (args.Length == 2 || args.Length == 4)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h9")
                    {
                        return "GET /" + args[i];
                    }
                }
                return "FALSE";
            }
            else if (args.Length == 3 || args.Length == 5)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h9")
                    {
                        return ("PUT /" + args[i] + "\r\n\r\n" + args[i + 1]);
                    }
                }
                return "FALSE";
            }
            else return "FALSE";
        }
        else if (format == 2) // http 1.0
        {
            if (args.Length == 2 || args.Length == 4)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h0")
                    {
                        return args[i];
                    }
                }
                return "FALSE";
            }
            else if (args.Length == 3 || args.Length == 5)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h0")
                    {
                        return (args[i] + " " + args[i + 1]);
                    }
                }
                return "FALSE";
            }
            else return "FALSE";
        }
        else if (format == 3) //http 1.1
        {
            if (args.Length == 2 || args.Length == 4)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h1")
                    {
                        return args[i];
                    }
                }
                return "FALSE";
            }
            else if (args.Length == 3 || args.Length == 5)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != host && args[i] != "-h" && args[i] != "-h1")
                    {
                        return (args[i] + " " + args[i + 1]);
                    }
                }
                return "FALSE";
            }
            else return "FALSE";
        }

        else return "FALSE";
    }
    /// <summary>
    /// Main program
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        try
        {
            //opens connection to server on localhost at port 43
            //sets up readers and writers
            TcpClient client = new TcpClient();
            string host = Hosting(args);
            client.Connect(host, 43);

            //client.ReceiveTimeout = 1000;
            //client.SendTimeout = 1000;
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());
            sw.WriteLine(Formatting(RequestFormat(args), args, host));
            //flush server commands and read reply
            sw.Flush();
            sw.Close();
            string response = sr.ReadToEnd().Trim();

            //outputs reply to console
            if (response == " ")
            {
                Console.WriteLine("No Response");
            }
            else if (response == "OK")
            {
                Console.WriteLine(response);
            }
            else
            {
                Console.WriteLine(response);
            }
        }
        catch
        {
            Console.Write("ya fucked up");
            Console.Write("ERROR: no entries found \r\n");
        }
    }
}

