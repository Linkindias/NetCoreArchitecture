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

		static Process process = new Process();

		static void Main(string[] args)
		{
			List<string> ips = new List<string>() { "10.168.18.50" }; //devices ip
			
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			var outDevices = QueryDevicesConnect();

			bool isDiscon = SetDevicesDisconnect(outDevices);

			if (isDiscon) 
				ips.ForEach(o =>
				{
					executeProcess($"{conDeviceCommand} {o}");

					var outConnectDevice = QueryDevicesConnect();

					var connectDevice = outConnectDevice.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
					if (IsSameDevice(connectDevice, o))
					{
						Console.WriteLine($"Connected Devices :{o}");

						if (executeProcess(unInstallAppCommand) == "Success\r\n")
						{
							Console.WriteLine($"UnInstalled :{package}");

							if (executeProcess(installAppCommand) == "Success\r\n")
							{
								Console.WriteLine($"Installed :{package}");

								if (IsStartApp()) 
									Console.WriteLine($"Start App :{package}");
							}
						}
					}

					SetDevicesDisconnect(outConnectDevice);
				});
		}

		private static bool IsStartApp()
		{
			return executeProcess(startAppCommand) == "Starting: Intent { act=" + package + startActivity + " cmp=" + package + "/" + startActivity + " }\r\n";
		}

		private static string executeProcess(string command)
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

		private static bool IsSameDevice(string[] connectDevice, string o)
		{
			return connectDevice.Length > 1 && connectDevice[1].IndexOf(o) > -1 && connectDevice[1].IndexOf("device") > -1;
		}

		private static bool SetDevicesDisconnect(string outDevices)
		{
			bool isDiscon = true;

			if (IsConnectDevice(outDevices))
			{
				isDiscon = false;

				if (executeProcess(disconDeviceCommand) == "disconnected everything\r\n") isDiscon = true;
			}

			process.WaitForExit();
			return isDiscon;
		}

		private static bool IsConnectDevice(string outDevices)
		{
			return outDevices.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length > 1;
		}

		private static string QueryDevicesConnect()
		{
			process.StartInfo.Arguments = checkDeviceCommand;
			process.Start();

			string outDevices = null;
			while (!process.StandardOutput.EndOfStream)
			{
				outDevices += process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
			return outDevices;
		}
	}
}
