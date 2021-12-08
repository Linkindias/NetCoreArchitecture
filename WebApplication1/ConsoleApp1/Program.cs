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

		static void Main(string[] args)
		{
			List<string> ips = new List<string>() { "10.168.18.50" }; //devices ip
			
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			var outDevices = QueryDevicesConnect(process);

			bool isDiscon = SetDevicesDisconnect(outDevices, process);

			if (isDiscon) 
				ips.ForEach(o =>
				{
					SetDeviceConnect(process, o);

					var outConnectDevice = QueryDevicesConnect(process);

					var connectDevice = outConnectDevice.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
					if (IsSameDevice(connectDevice, o))
					{
						Console.WriteLine($"Connected Devices :{o}");

						if (SetDeviceUnInstall(process) == "Success\r\n")
						{
							Console.WriteLine($"UnInstalled :{package}");

							if (SetDeviceInstall(process) == "Success\r\n")
							{
								Console.WriteLine($"Installed :{package}");

								if (IsStartApp(process)) 
									Console.WriteLine($"Start App :{package}");
							}
						}
					}

					SetDevicesDisconnect(outConnectDevice, process);
				});
		}

		private static bool IsStartApp(Process process)
		{
			return StartApp(process) == "Starting: Intent { act=" + package + startActivity + " cmp=" + package + "/" + startActivity + " }\r\n";
		}

		private static string StartApp(Process process)
		{
			process.StartInfo.Arguments = $"/c adb shell am start -a {package}{startActivity} -n {package}/{package}{startActivity}";
			process.Start();

			string outStartApp = string.Empty;
			while (!process.StandardOutput.EndOfStream)
			{
				outStartApp = process.StandardOutput.ReadLine() + Environment.NewLine;
			}
			
			process.WaitForExit();
			return outStartApp;
		}

		private static string SetDeviceInstall(Process process)
		{
			process.StartInfo.Arguments = $"/c adb shell pm install -r /storage/emulated/0/Download/app-debug.apk";
			process.Start();

			string outInstall = string.Empty;
			while (!process.StandardOutput.EndOfStream)
			{
				outInstall = process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
			return outInstall;
		}

		private static string SetDeviceUnInstall(Process process)
		{
			process.StartInfo.Arguments = $"/c adb shell pm uninstall {package}";
			process.Start();

			string outUnInstall = string.Empty;
			while (!process.StandardOutput.EndOfStream)
			{
				outUnInstall = process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
			return outUnInstall;
		}

		private static bool IsSameDevice(string[] connectDevice, string o)
		{
			return connectDevice.Length > 1 && connectDevice[1].IndexOf(o) > -1 && connectDevice[1].IndexOf("device") > -1;
		}

		private static void SetDeviceConnect(Process process, string ip)
		{
			process.StartInfo.Arguments = $"/c adb connect {ip}";
			process.Start();

			string outConnect = string.Empty;
			while (!process.StandardOutput.EndOfStream)
			{
				outConnect = process.StandardOutput.ReadLine() + Environment.NewLine;
			}

			process.WaitForExit();
		}

		private static bool SetDevicesDisconnect(string outDevices, Process process)
		{
			bool isDiscon = true;

			if (IsConnectDevice(outDevices))
			{
				isDiscon = false;

				process.StartInfo.Arguments = "/c adb disconnect";
				process.Start();

				string outDisconnect = string.Empty;
				while (!process.StandardOutput.EndOfStream)
				{
					outDisconnect = process.StandardOutput.ReadLine() + Environment.NewLine;
				}

				if (outDisconnect == "disconnected everything\r\n") isDiscon = true;
			}

			process.WaitForExit();
			return isDiscon;
		}

		private static bool IsConnectDevice(string outDevices)
		{
			return outDevices.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length > 1;
		}

		private static string QueryDevicesConnect(Process process)
		{
			process.StartInfo.Arguments = "/c adb devices";
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
