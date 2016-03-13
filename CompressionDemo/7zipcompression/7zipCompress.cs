using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace _7zipCompression
{
	class Program
	{
		static void Main(string[] args)
		{
			string sourceName = args[0];
			string targetName = args[1];
		//	if (File.Exists(sourceName))
		//		Process.Start(sourceName);
			// 1
			// Initialize process information.
			//
			ProcessStartInfo p = new ProcessStartInfo();
			p.FileName = "7zG.exe";

			// 2
			// Use 7-zip
			// specify a=archive and -tgzip=gzip
			// and then target file in quotes followed by source file in quotes
			//
			p.Arguments = "a -tgzip \"" + targetName + "\" \"" +
				sourceName + "\" -mx=9";
			p.WindowStyle = ProcessWindowStyle.Normal;

			// 3.
			// Start process and wait for it to exit
			//
			
		}
	}
}
