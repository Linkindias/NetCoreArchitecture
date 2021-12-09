using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
	class Program
	{
		/// <summary>
		/// run OS 需有 adb.exe
		/// </summary>
		static string package = "com.example.kotlinsampleapplication"; //app domain
		static string startActivity = ".MainActivity"; //start activity

		private static string startAppCommand = $"/c adb shell am start -a {package}{startActivity} -n {package}/{package}{startActivity}";
		private static string installAppCommand = $"/c adb shell pm install -r /storage/emulated/0/Download/app-debug.apk";
		private static string unInstallAppCommand = $"/c adb shell pm uninstall {package}";
		private static string checkDeviceCommand = "/c adb devices";
		private static string disconDeviceCommand = "/c adb disconnect";
		private static string conDeviceCommand = "/c adb connect";
		private static string pingCommand = "/c ping";

		static Process process = new Process();

		static void Main(string[] args)
		{
			List<string> connectDevices = new List<string>() { "10.168.18.50" }; 
			List<string> dicconDevices = new List<string>(); 
			List<string> notInstallDevices = new List<string>(); 
			List<string> notStartDevices = new List<string>(); 

			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			bool isDiscon = DisconnectDevices();

			if (isDiscon)
				connectDevices.ForEach(o =>
				{
					if (IsPingDevice(o))
					{
						Console.WriteLine($"Ping Devices :{o}");

						ConnectDevice(o);

						if (IsSameDevice(o))
						{
							Console.WriteLine($"Connected Devices :{o}");

							if (UnInstallApp() == "Success\r\n") Console.WriteLine($"UnInstalled :{package}");

							if (InstallApp() == "Success\r\n")
							{
								Console.WriteLine($"Installed :{package}");

								if (IsStartApp())
								{
									Console.WriteLine($"Start App :{package}");
									return;
								}
								notStartDevices.Add(o);
								Console.WriteLine($"NotStart App :{o}");
							}
							notInstallDevices.Add(o);
							Console.WriteLine($"NotInstall Devices :{o}");
						}
						DisconnectDevices();
					}
					else
					{
						Console.WriteLine($"Discon Device :{o}");
						dicconDevices.Add(o);
					}
				});
			process.WaitForExit();
		}

		private static string InstallApp()
		{
			return ExecCmdSingleResult(installAppCommand);
		}

		private static string UnInstallApp()
		{
			return ExecCmdSingleResult(unInstallAppCommand);
		}

		private static void ConnectDevice(string o)
		{
			ExecCmdSingleResult($"{conDeviceCommand} {o}");
		}

		private static bool DisconnectDevices()
		{
			var outDevices = ExecCmdAllResult(checkDeviceCommand);

			bool isDiscon = true;

			if (IsConnectDevice(outDevices))
			{
				isDiscon = false;

				if (IsDisconnectDevices()) isDiscon = true;
			}

			process.WaitForExit();
			return isDiscon;
		}

		private static string ExecCmdSingleResult(string command)
		{
			process.StartInfo.Arguments = command;
			process.Start();

			string outResult = string.Empty;
			while (!process.StandardOutput.EndOfStream)
			{
				outResult = process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
			return outResult;
		}

		private static string ExecCmdAllResult(string command)
		{
			process.StartInfo.Arguments = command;
			process.Start();

			string outDevices = null;
			while (!process.StandardOutput.EndOfStream)
			{
				outDevices += process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
			return outDevices;
		}

		private static bool IsPingDevice(string o)
		{
			var pingDevice = ExecCmdAllResult($"{pingCommand} {o}").Split("\r\n", StringSplitOptions.RemoveEmptyEntries); ;

			return pingDevice.Length > 1 && pingDevice[1].IndexOf($"回覆自 {o}: 位元組=32 時間<1ms TTL=64") > -1;
		}

		private static bool IsStartApp()
		{
			return ExecCmdSingleResult(startAppCommand) == "Starting: Intent { act=" + package + startActivity + " cmp=" + package + "/" + startActivity + " }\r\n";
		}
	
		private static bool IsSameDevice(string o)
		{
			var connectDevice = ExecCmdAllResult(checkDeviceCommand).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

			return connectDevice.Length > 1 && connectDevice[1].IndexOf(o) > -1 && connectDevice[1].IndexOf("device") > -1;
		}

		private static bool IsConnectDevice(string outDevices)
		{
			return outDevices.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length > 1;
		}

		private static bool IsDisconnectDevices()
		{
			return ExecCmdSingleResult(disconDeviceCommand) == "disconnected everything\r\n";
		}
	}
}
