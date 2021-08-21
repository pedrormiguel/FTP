using System.Collections.Generic;
using System.Threading.Tasks;
using FTPConsole.Class.Common;
using FTPConsole.Class.Dto;
using FTPLib.Class;
using FTPLib.Class.Common;
using static System.Console;
using static FTPConsole.Utility.ConsoleUtility;
using BaseCredentials = FTPConsole.Class.Common.BaseCredentials;

namespace FTPConsole.Class
{
    public static class Menu
    {
        private static DbFileHandler Handler { get; set; } = new DbFileHandler();

        public static async Task InitialPoint()
        {
            var selection = MainMenu();

            switch (selection)
            {
                case "1":
                    await ConnectToServer();
                    break;

                case "2":
                    await RegisterNewServer();
                    break;

                case "3":
                    await ListAllServer();
                    break;

                case "4":
                    await DeleteServer();
                    break;

                case "5":
                    await UpdateServer();
                    break;

                case "0":
                    await ClearScreen();
                    break;

                case "Q":
                    return;

                default:
                    await RepeatOptions(InitialPoint);
                    break;
            }
        }

        private static string MainMenu()
        {
            WriteLine("\t \t ..Menu..");
            WriteLine("Options\n");
            WriteLine("1. Connect to a server.");
            WriteLine("2. Register new server.");
            WriteLine("3. List all the servers.");
            WriteLine("4. Delete server.");
            WriteLine("5. Update server.");
            WriteLine("0. Clear the screen.");
            WriteLine("Q. Exit.");


            WriteLine("-----------------------");
            InsertBlankLine();

            var selection = ReadOption();

            return selection;
        }

        static DtoConnectioSever MenuConnection()
        {
            var connection = new DtoConnectioSever();
            var IsValidPort = false;
            int port = 21;

            InsertBlankLine();
            WriteLine("* Please fullfill with the correct FTP Information Server");
            InsertBlankLine();

            Write("\t1.\tHostname Server\t :");
            var hostname = ReadLine();

            Write("\t2.\tUsername Server\t :");
            var username = ReadLine();

            Write("\t3.\tPassword Server\t :");
            var passwrod = ReadLine();

            do
            {
                Write("\t4.\tPort Server\t :");
                var input = ReadLine();

                IsValidPort = int.TryParse(input, out port);
                InsertBlankLine();

                if (IsValidPort == false)
                    WriteLine("Digit a valid Number Port");

                InsertBlankLine();


            } while (!IsValidPort);

            connection.Assing(hostname, username, passwrod, port);

            return connection;
        }

        public static string MenuEditServer()
        {
            WriteLine("\t \t ..Menu..");
            WriteLine("Options\n");
            WriteLine("\t1.Hostname Server\t :");
            WriteLine("\t2.Username Server\t :");
            WriteLine("\t3.Password Server\t :");
            WriteLine("\t4.Port Server\t :");

            WriteLine("-----------------------");
            InsertBlankLine();

            var selection = ReadOption();

            return selection;
        }

        private static async Task MenuFtpOptions(Ftp client)
        {
            InsertBlankLine();
            WriteLine("\t \t ..MENU..");
            InsertBlankLine();

            WriteLine("- FTP Options");
            InsertBlankLine();

            WriteLine("\t1. Display all the files.");
            WriteLine("\t2. Display all the folders.");
            WriteLine("\t3. Upload a file.");
            WriteLine("\t4. Download a file.");
            WriteLine("\t0. Clear screen.");
            WriteLine("\tM. Return to the menu.");
            WriteLine("\tQ. Quit.");

            InsertBlankLine();

            var option = ReadOption();

            switch (option.ToUpper())
            {
                case "1":
                    await DisplayAllTheFiles(client);
                    break;

                case "2":
                    await DisplayAllTheFilesCustomPath(client);
                    break;

                case "3":
                    await UploadFile(client);
                    break;

                case "4":
                    await DownloadFile(client);
                    break;

                case "0":
                    Clear();
                    await MenuFtpOptions(client);
                    break;

                case "Q":
                    return;

                case "M":
                    await InitialPoint();
                    break;

                default:
                    await RepeatOptions(MenuFtpOptions, client);
                    break;
            }
        }

        private static async Task DownloadFile(Ftp client)
        {
            InsertBlankLine();

            Write("Insert the local path to download the file :");
            var localPathOfFile = ReadLine();
            InsertBlankLine();

            Write("Insert the remote path where the file is :");
            var remotePathOfFile = ReadLine();
            InsertBlankLine();

            var downloadFileResponse = await client.DownloadFile(localPathOfFile, remotePathOfFile);

            WriteLine(downloadFileResponse.Data);
            InsertBlankLine();
            WriteLine("Hit a key to return to the menu.");
            ReadKey();
            await MenuFtpOptions(client);
        }

        private static async Task UploadFile(Ftp client)
        {
            InsertBlankLine();

            Write("Insert the local path :");
            var localPath = ReadLine();
            InsertBlankLine();

            Write("Insert the remote path :");
            var remotePath = ReadLine();
            InsertBlankLine();
            WriteLine("Uploading File....");

            var responseUploadFile = await client.UploadFile(localPath, remotePath);

            if (!responseUploadFile.Status)
                WriteLine($"{responseUploadFile.Error}");

            WriteLine(responseUploadFile.Data);
            InsertBlankLine();
            WriteLine("Hit a key to return to the menu.");
            ReadKey();
            await MenuFtpOptions(client);
        }

        private static async Task DisplayAllTheFilesCustomPath(Ftp client)
        {
            InsertBlankLine();

            Write("Insert the path to look up or leave empty for the root: ");
            var output = ReadLine();
            var response = await client.GetListItemsFiles(output);

            if (response.Status)
            {
                WriteLine("Files on the remote server : \n");

                foreach (var item in response.Data)
                {
                    WriteLine($"- {item.FullName} {item.Size} {item.OwnerPermissions}");
                }
            }
            else
            {
                WriteLine($"{response.Error}");
            }

            InsertBlankLine();
            WriteLine("Hit a key to return to the menu.");
            ReadKey();
            await MenuFtpOptions(client);
        }

        private static async Task DisplayAllTheFiles(Ftp client)
        {
            InsertBlankLine();
            var response = await client.GetListItems();

            if (response.Status)
            {

                WriteLine("Files on the remote server : \n");

                foreach (var item in response.Data)
                {
                    WriteLine($"- {item}");
                }
            }
            else
            {
                WriteLine($"{response.Error}");
            }

            InsertBlankLine();
            InsertBlankLine();
            WriteLine("Hit a key to return to the menu.");
            ReadKey();
            await MenuFtpOptions(client);
        }

        private static async Task ConnectToServer()
        {
            bool output;

            do
            {
                var credetials = MenuConnection();

                var clientFtp = Connection(credetials);

                output = clientFtp.IsConnected;

                if (output)
                {
                    Write("Connected Successfully, hit enter...");
                    WaitAndClearScreen();
                    await MenuFtpOptions(clientFtp);
                }
                else
                {
                    InsertBlankLine();
                    Write("Was a problem connecting with the server, try again. Y/N : ");
                    var input = ReadLine();

                    if (input.ToUpper().Equals("N"))
                    {
                        Clear();
                        output = true;
                        await InitialPoint();
                    }

                    Clear();
                }

            } while (!output);
        }

        public static Ftp Connection(DtoConnectioSever credentials)
        {
            Response<bool> response;

            var client = new Ftp(credentials.HostName, credentials.UserName, credentials.Password, credentials.Port);
            response = client.Connect();

            if (!response.Data)
            {
                WriteLine("It's not Connected to the server");
                WriteLine();
            }

            return client;
        }

        public static async Task RegisterNewServer()
        {
            var credentials = MenuConnection();

            var response = await Handler.Add(credentials);

            if (response.Status)
                Write($"The Ftp server [{credentials.HostName}] was saved it");
            else
                Write("Ocurred a problem saving the credetials, try again.");

            InsertBlankLine();
            WaitAndClearScreen();
            await InitialPoint();
        }

        private static async Task<List<DtoConnectioSever>> GetAllServer()
        {
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
            return FtpSevers;
        }

        public static async Task ListAllServer()
        {
            InsertBlankLine();
            WriteLine("\t \t ..MENU..");
            InsertBlankLine();
            List<DtoConnectioSever> FtpSevers = await GetAllServer();

            Write("If you want to return to the menu press a Non-numeric key OR Press the number to connect to one server from the list above, digit the number. ? :");

            var anserw = ReadLine().ToUpper();
            int ServerPosition;
            var IsCorrect = int.TryParse(anserw, out ServerPosition);

            if (IsCorrect)
            {
                DtoConnectioSever server = null;

                try
                {
                    server = FtpSevers[ServerPosition - 1];
                }
                catch
                {
                    InsertBlankLine();
                    InsertBlankLine();

                    WriteLine("The option is not available, returning to the menu...");
                    WaitAndClearScreen();
                    await InitialPoint();
                }

                InsertBlankLine();
                var client = Connection(server);

                if (client.IsConnected)
                {

                    InsertBlankLine();
                    Write("Connected Successfully, hit enter...");
                    WaitAndClearScreen();
                    await MenuFtpOptions(client);
                }
                else
                {
                    InsertBlankLine();
                    Write("there Was a problem connecting with the server, try Later....");
                    WaitAndClearScreen();
                    await InitialPoint();
                }

            }
            else
            {
                await InitialPoint();
            }
        }

        public static async Task DeleteServer()
        {
            var servers = await GetAllServer();

            Write("Select the number of the server to delete ? :");
            var input = ReadLine();

            var response = await Handler.Delete(servers[int.Parse(input) - 1]);

            if (response.Status)
            {
                WriteLine("The server was deleted successfully");
            }
            else
            {
                WriteLine($"{response.Error}");
            }

            await InitialPoint();
        }

        public static async Task UpdateServer()
        {
            var servers = await GetAllServer();

            Write("Select the number of the server to delete ? :");

            var input = ReadLine();
            var server = servers[int.Parse(input) - 1];


            var editResponse = EditServer(input, ref server);
            Response<string> updatedResponse = new Response<string>();

            if (editResponse.Status)
                updatedResponse = await Handler.Update(editResponse.Data as BaseCredentials);

            if (editResponse.Status && updatedResponse.Status)
            {
                WriteLine("The server was updated successfully");
            }
            else
            {
                WriteLine($"-- {editResponse.Error} -- \n \n -- {updatedResponse.Error} --");
            }

            await InitialPoint();
        }

        public static Response<DtoConnectioSever> EditServer(string option, ref DtoConnectioSever dto)
        {
            var response = new Response<DtoConnectioSever>();

            option = MenuEditServer();

            switch (option)
            {
                case "1":
                    Write("\t1.Hostname Server\t :");
                    dto.HostName = ReadLine();
                    response.Status = true;
                    break;

                case "2":
                    Write("\t2.Username Server\t :");
                    dto.UserName = ReadLine();
                    response.Status = true;
                    break;

                case "3":
                    Write("\t3.Password Server\t :");
                    dto.Password = ReadLine();
                    response.Status = true;
                    break;

                case "4":
                    int port = 21;
                    var IsValidPort = false;
                    do
                    {
                        Write("\t4.Port Server\t :");
                        var input = ReadLine();

                        IsValidPort = int.TryParse(input, out port);
                        InsertBlankLine();

                        if (IsValidPort == false)
                            WriteLine("Digit a valid Number Port");

                        InsertBlankLine();

                    } while (!IsValidPort);
                    dto.Port = port;
                    response.Status = true;
                    break;

                default:
                    response.Error = "Not Valid option";
                    break;
            }

            response.Data = dto;

            return response;
        }

        public static async Task ClearScreen()
        {
            Clear();
            await InitialPoint();
        }
    }

}