using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Collections.Generic;
/// <summary>
/// LocationServer
/// </summary>
public class Respond
{

    static Dictionary<string, string> dictionary = new Dictionary<string, string>()
    {
        {"470566", "Fenner-052"}
    };

    /// <summary>
    /// main takes in array of strings from command line
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        runServer();
    }
    /// <summary>
    /// called by the main, creates and runs the server
    /// </summary>
    public static void runServer()
    {
        TcpListener listener;
        Socket connection;
        NetworkStream socketStream;
        try
        {
            //creates tcp socket which listens on port 43 for incoming requests
            listener = new TcpListener(IPAddress.Any, 43);
            listener.Start();
            Console.WriteLine("Server started listening");

            //loop forever
            //handles requests
            while (true)
            {
                //create a socket to handle a recieved request
                connection = listener.AcceptSocket();
                socketStream = new NetworkStream(connection);
                Console.WriteLine("Connection Recieved");
                doRequest(socketStream);

                //close sockets and connection once request is done
                socketStream.Close();
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception" + e.ToString());
        }
    }
    /// <summary>
    /// called after the server recieves a connection on a listener
    /// </summary>
    /// <param name="socketStream"></param>
    public static void doRequest(NetworkStream socketStream)
    {
        try
        {
            //socketStream.ReadTimeout = 1000;
            //socketStream.WriteTimeout = 1000;

            //creates stream reader/writer for socket I/O
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);

            //reading first line of request, trims edges to remove spaces and \n \r
            string line = sr.ReadToEnd();
            Console.WriteLine("LocationServer Received " + line);

            //splits line at first space character into 2 parts
            string[] sections = line.Split(new char[] { ' ' }, 2);
            if (sections.Length == 1)
            {

                if (dictionary.ContainsKey(sections[0]))
                {
                    string location = dictionary[sections[0]];
                    sw.WriteLine(location);
                    sw.Flush();
                }
                else
                {
                    sw.WriteLine("ERROR: no entries found");
                    sw.Flush();
                }
            }
            else if (sections.Length == 2)
            {
                sw.WriteLine("OK");
                sw.Flush();
                if (dictionary.ContainsKey(sections[0]))
                {
                    dictionary[sections[0]] = sections[1];
                }
                else if (!dictionary.ContainsKey(sections[0]))
                {
                    dictionary.Add(sections[0], sections[1]);
                }
            }



        }
        catch
        {
            //catch in case somehting goes really wrong
            Console.WriteLine("Unrecognised Command \nConnection Broken");
        }
    }
}

