using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibFTP;
using static System.Console;
using static ConsoleApp.Utility.ConsoleUtility;

namespace ConsoleApp.Class
{
    public static class Menu
    {
        public static async Task InitialPoint()
        {
            var selection = MainMenu();

            switch (selection)
            {
                case "1":
                    await ConnectoToServer();
                    break;

                case "2":
                    await RegisterNewServer();
                    break;
                    
                case "3":
                    await ListAllServer();
                    break;

                case "0":
                    ClearScreen();
                    break;

                case "Q":
                    return;

                default:
                    await RepeatOptions(InitialPoint);
                    break;
            }
        }

        private static DbFileHandler Handler { get; set; } = new DbFileHandler();
        public static string MainMenu()
        {
            WriteLine("\t \t ..Menu..");
            WriteLine("Options\n");
            WriteLine("1. Connect to a server.");
            WriteLine("2. Register new server.");
            WriteLine("3. List all the servers.");
            WriteLine("0. Clear the screen.");
            WriteLine("Q. Exit.");


            WriteLine("-----------------------");
            InsertBlankLine();

            var selection = ReadOption();

            return selection;
        }
        public static async Task ConnectoToServer()
        {
            var output = false;

            do
            {
                var credetials = MenuConnection();
                var clientFtp = Connection(credetials);
                output = clientFtp.IsConnected;

                if (output)
                {
                    Write("Connected Successfully, hit enter...");
                    WaitAndClearScreen();
                    await MenuFtpOptions(clientFtp); //TODO Monitoring difference between  and await;
                }
                else
                {
                    InsertBlankLine();
                    WriteLine("Was a problem connecting with the server, try again");
                    WaitAndClearScreen();
                }

            } while (!output);
        }
        static DtoConnectioSever MenuConnection()
        {
            var connection = new DtoConnectioSever();

            InsertBlankLine();
            WriteLine("* Please fullfill with the correct FTP Information Server");
            InsertBlankLine();

            Write("\t1.Hostname Server\t :");
            var hostname = ReadLine();

            Write("\t2.Username Server\t :");
            var username = ReadLine();

            Write("\t3.Password Server\t :");
            var passwrod = ReadLine();

            Write("\t4.Port Server\t :");
            var port = ReadLine();
            InsertBlankLine();

            connection.Assing(hostname, username, passwrod, port);

            return connection;
        }
        static FTP Connection(DtoConnectioSever credentials)
        {
            var client = new FTP(credentials.HostName, credentials.UserName, credentials.Password, credentials.Port);
            client.Connect();

            return client;
        }
        public static async Task MenuFtpOptions(FTP client)
        {
            InsertBlankLine();
            WriteLine("\t \t ..MENU..");
            InsertBlankLine();

            WriteLine("- FTP Options");
            InsertBlankLine();

            WriteLine("\t1.Display all the files.");
            WriteLine("\t2.Display all the folders.");
            WriteLine("\t3.Upload a file.");
            WriteLine("\t4.Download a file.");
            WriteLine("\t0.Clear screen.");
            InsertBlankLine();

            var option = ReadOption();

            switch (option)
            {
                case "1":
                    InsertBlankLine();
                    await client.ListItems();

                    InsertBlankLine();
                    InsertBlankLine();
                    WriteLine("Hit a key to return to the menu.");
                    ReadKey();
                    await MenuFtpOptions(client);
                    break;

                case "2":
                    InsertBlankLine();

                    Write("Insert the path to look up or leave empty for the root: ");
                    var output = ReadLine();
                    await client.ListItemsFiles(output);

                    WriteLine("Hit a key to return to the menu.");
                    ReadKey();
                    await MenuFtpOptions(client);
                    break;

                case "3":
                    InsertBlankLine();

                    Write("Insert the local path :");
                    var localPath = ReadLine();
                    InsertBlankLine();

                    Write("Insert the remote path :");
                    var remotePath = ReadLine();
                    InsertBlankLine();

                    var ouput = await client.UploadFile(localPath, remotePath);

                    if (ouput)
                        WriteLine("File successfully uploaded.");
                    else
                        WriteLine("File not successfully uploaded.");

                    InsertBlankLine();
                    WriteLine("Hit a key to return to the menu.");
                    ReadKey();
                    await MenuFtpOptions(client);
                    break;

                case "0":
                    Clear();
                    await MenuFtpOptions(client);
                    break;

                default:
                    await RepeatOptions(MenuFtpOptions, client);
                    break;
            }
        }
        public static async Task RegisterNewServer()
        {
            var credentials = MenuConnection();

            var response = await Handler.Add(credentials);

            if (response.status)
                Write($"The Ftp server [{credentials.HostName}] was saved it");
            else
                Write("Ocurred a problem saving the credetials, try again.");

            InsertBlankLine();
            WaitAndClearScreen();
            MainMenu();
        }
        public static async Task ListAllServer()
        {
            WriteLine("\t \t ..MENU..");
            InsertBlankLine();
            var response = await Handler.ReadAll();
            var FtpSevers = new List<DtoConnectioSever>();
            int counter = 0;

            if (!response.Data.Equals(null))
            {
                WriteLine("Listing all the available servers :");

                foreach (var line in response.Data as string[])
                {
                    counter++;
                    var item = DtoConnectioSever.Map(line);
                    WriteLine($"{counter}.{item.HostName}");
                    FtpSevers.Add(item);
                }
            }

            InsertBlankLine();
            Write("Do you want to return to the menu press a Non-numeric key OR If want to connect to one server from the list above, digit the number. ? :");
            var anserw = ReadLine().ToUpper();
            int ServerPosition;
            var IsCorrect = int.TryParse(anserw, out ServerPosition);

            if (!IsCorrect)
            {
                MainMenu();
            }
            else
            {
                DtoConnectioSever server = null;

                try
                {
                    server = FtpSevers[ServerPosition - 1];
                }
                catch (Exception ex)
                {
                    InsertBlankLine();
                    WriteLine("The option is not available, returning to the menu...");
                    WaitAndClearScreen();
                    MainMenu();
                }

                var client = Connection(server);

                if (client.IsConnected)
                {
                    Write("Connected Successfully, hit enter...");
                    WaitAndClearScreen();
                    await MenuFtpOptions(client);
                }
                else
                {
                    WriteLine("there Was a problem connecting with the server, try Later.");
                    Clear();
                    MainMenu();
                }
            }
        }
        public static void ClearScreen()
        {
            Clear();
            MainMenu();
        }
    }
}