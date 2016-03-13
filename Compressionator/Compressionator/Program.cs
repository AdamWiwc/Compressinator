using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
 

namespace Compressinator
{

    public enum ErrorLevel : byte 
    {
        error,
        warning,
        verbose
    };
    public enum Type : byte
    { 
        zip,
        zip7,
        gZip,
        add,
        delete,
        extract
    };
    public enum Filter : byte
    { 
        none,
        jpg,
        png,
        txt
    };

    class Globals
    {
        public ErrorLevel errlvl = ErrorLevel.error;
        public Type cmpType = Type.zip;
        public Filter filter = Filter.none;
        public string logPath = null;
        public bool isLogging = true;
        private System.IO.StreamWriter file;

        private static Globals instance;
        private Globals()
        { }

        public string GetFilter()
        { 
            switch(filter)
            {
                case Filter.jpg:
                    return ".jpg";
                case Filter.png:
                    return ".png";
                case Filter.txt:
                    return ".txt";
                default:
                    return null;
            }
        }

        public static Globals Instance
        {
            get 
            {
                if (instance == null)
                    instance = new Globals();
                return instance;
            }
        }

        ~Globals()
        {
            if(file != null)
                file.Close();
        }
        public void Init()
        { 
            if(isLogging)
            {
                if(logPath != null )
                {
                    if(File.Exists(logPath))
                    {
                        string dir = Path.GetDirectoryName(logPath);
                        string fileName = Path.GetFileNameWithoutExtension(logPath);
                        string fileExt = Path.GetExtension(logPath);

                        fileName += "_";

                        //cycle through each iteration to see if they already exist. 
                        for (uint i = 0; ; ++i)
                        {
                            string newPath = Path.Combine(dir, fileName + " " + i + fileExt);
                            if (!File.Exists(newPath))
                            {
                                logPath = newPath;
                                break;
                            }
                        }
                        file = new System.IO.StreamWriter(logPath);
                    }
                    else
                    {
                        Console.Write("ERROR: logPath error, will not write log.\n");
                        isLogging = false;   
                    }
                }
            }
        }
        public void Log(string message, ErrorLevel lvl)
        {
            if(isLogging && lvl <= errlvl)
            {
                if (logPath == null)
                {
                    Console.Write(message);
                }
                else
                {
                    file.WriteLine(message);
                }
            }
        }
    }
    

    class Program
    {
        static void PrintUsage()
        { 
            Console.Write("Compressionator.exe [Source] [Target] -type -filter -log -detail\n");
            Console.Write("-type: -zip, -7zip, -gzip, -a add, -d delete, -ex extract\n");
            Console.Write("-filter: none, jpg, png, txt\n");
            Console.Write("-log: -t true, -f false\n");
            Console.Write("-detail: -er error, -w warning, -v verbose\n");
            Console.Write("Source is the path to the source,\n");
            Console.Write("Target is the extract/compress destination or the target file to be added/deleted.\n");
            Console.Write("leave Taget blank and specify a filter to delete all of specific filetype.\n");

        }
        static void DoCommand(short command)
        {
            switch (command)
            {
                case 0:
                    Globals.Instance.cmpType = Type.zip;
                    break;
                case 1:
                    Globals.Instance.cmpType = Type.zip7;
                    break;
                case 2:
                    Globals.Instance.cmpType = Type.gZip;
                    break;
                case 3:
                    Globals.Instance.filter = Filter.jpg;
                    break;
                case 4:
                    Globals.Instance.filter = Filter.png;
                    break;
                case 5:
                    Globals.Instance.filter = Filter.txt;
                    break;
                case 6:
                    Globals.Instance.cmpType = Type.add;
                    break;
                case 7:
                    Globals.Instance.cmpType = Type.delete;
                    break;
                case 8:
                    Globals.Instance.cmpType = Type.extract;
                    break;
                case 9:
                    Globals.Instance.isLogging = true;
                    break;
                case 10:
                    Globals.Instance.isLogging = false;
                    break;
                case 11:
                    Globals.Instance.errlvl = ErrorLevel.error;
                    break;
                case 12:
                    Globals.Instance.errlvl = ErrorLevel.warning;
                    break;
                case 13:
                    Globals.Instance.errlvl = ErrorLevel.verbose;
                    break;
                case 14:
                    PrintUsage();
                    break;
                default:
                    Console.Write("ERROR: Default Struck in DoCommand!");
                    break;
            }
        }

        static void Main( string[] args )
        {
            //usage Compressionator.exe [Source] [Target] -type -filter -log -detail
            //-type: -zip, -7zip, -gzip, -a add, -d delete, -ex extract
            //-filter: -jpg, -png, -txt
            //-log -t true, -f false
            //-detail -er error, -w warning, -v verbose
            //Source is the path to the source,
            //Target is the extract/compress destination or the target file to be added/deleted.
            //leave Taget blank and specify a filter to delete all of specific filetype.

            string sourceName = null;
            string targetName = null;
            
            string[] commands = {"-zip", "-7zip", "-gzip", "-jpg", "-png", "-txt", "-a", "-d", "-ex", "-t", "-f", "-er", "-w", "-v", "-?"};


            if (args.Length == 0)
            {
                Console.Write("Please define a source path\n-? for help.\n");
                return;
            }

            sourceName = args[0]; //Some filepaths have spaces in them, which means they will be counted as two seperate args.
                                  //I know how I would fiz this issue with the program but I don't have the time right now. 
            short argOffset = 1;
            if (args.Length > 1 && args[1].Count() > 5)
            {
                targetName = args[1];
                argOffset++;   
            }
            else
                targetName = "";
            for (short i = argOffset; i < args.Length; ++i)
            {
                for(short j = 0; j < commands.Length; ++j)
                    if (string.Compare(args[i], commands[j], true) == 0)
                    {
                        DoCommand(j);
                    }
            }

            if (targetName == null)
                targetName = sourceName;

            Globals.Instance.Init();

            //Do the stuff with the commands!
            switch (Globals.Instance.cmpType)
            { 
                case Type.zip:
                    Zip.SmartZip(sourceName, targetName);
                    break;
                case Type.zip7:
                    //Zip.Zip7Compression(sourceName, targetName);
                    Console.Write("7Zip does not work :(\n");
                    break;
                case Type.gZip:
                    Zip.GZipCompress(sourceName, targetName);
                    break;
                case Type.extract:
                    Zip.SmartUnzip(sourceName, targetName);
                    break;
                case Type.delete:
                    Zip.DeleteFile(sourceName, targetName);
                    break;
                case Type.add:
                    Zip.AddFile(sourceName, targetName);
                    break;
                default:
                    Console.Write("ERROR: Default case hit! \n");
                    break;
            }
        }
    }

    public static class Zip
    {
        public static void SmartZip(string dirToZip, string dirToPlace)
        {
            Globals.Instance.Log("Zipping File...\n", ErrorLevel.verbose);

            if (Directory.Exists(dirToZip))
            {

                if (!Directory.Exists(dirToPlace))
                {
                    dirToPlace = Path.GetFullPath(dirToZip);
                    Globals.Instance.Log("ERROR: " + dirToPlace + " is invalid, placing in source directory.\n", ErrorLevel.error);
                }
                
                dirToPlace += ".zip";
                if (File.Exists(dirToPlace))
                {
                    Globals.Instance.Log("Deleting old File...\n", ErrorLevel.verbose);
                    File.Delete(dirToPlace);
                }
               
                    ZipFile.CreateFromDirectory(dirToZip, dirToPlace);
            }
            else 
            {
                Globals.Instance.Log("ERROR: " + dirToZip + " Could Not be found.\n", ErrorLevel.error);
            }
            
        }

        public static void GZipCompress( string name, string dirToPlace )
        {
            if(!Directory.Exists(name))
            {
                Globals.Instance.Log( name + " Directory not found.\n", ErrorLevel.error);
                return;
            }
            DirectoryInfo directorySelected = new DirectoryInfo(name);

            foreach( FileInfo fileToCompress in directorySelected.GetFiles() )
            { 
                using(FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if(((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) && fileToCompress.Extension != Globals.Instance.GetFilter())
                    {
                        if (dirToPlace == null)
                            dirToPlace = name;
                        using (FileStream CompressedFileStream = File.Create(dirToPlace + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream( CompressedFileStream, CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                    }
                } 
            }
        }

        public static void Zip7Compression( string source, string target)
        {

            if (Directory.Exists(source))
            {
				Process.Start(source);
                ProcessStartInfo p = new ProcessStartInfo();
			    p.FileName = "7zG.exe";
                p.Arguments = "a -tgzip \"" + target + "\" \"" +
				source + "\" -mx=9";
			    p.WindowStyle = ProcessWindowStyle.Normal;
                Process x = Process.Start(p);
			    x.WaitForExit();
            }
            else
            {
                Globals.Instance.Log("Directory " + source + " could not be found.\n", ErrorLevel.error);
            }
        }

        public  static void SmartUnzip(string name, string dir)
        {
            string fileUnzipFullPath;
            string fileUnzipFullName;

            if (File.Exists(name))
            {
                using (ZipArchive archive = ZipFile.OpenRead(name))
                {
                    foreach (ZipArchiveEntry file in archive.Entries)
                    {
                        Globals.Instance.Log("File Name:" + file.Name, ErrorLevel.verbose);
                        Globals.Instance.Log("File Size: " + file.Length + " Bytes", ErrorLevel.verbose);

                        fileUnzipFullName = Path.Combine(dir, file.FullName);
                        if (System.IO.File.Exists(fileUnzipFullName))
                        {
                            File.Delete(fileUnzipFullName);
                        }
                        fileUnzipFullPath = Path.GetDirectoryName(fileUnzipFullName);
                        Directory.CreateDirectory(fileUnzipFullPath);
                        file.ExtractToFile(fileUnzipFullName);
                    }
                }
            }
            else
            {
                Globals.Instance.Log(name + "Zip File Not found.\n", ErrorLevel.error);
            }
        }

        public static void DeleteFile(string name, string fileName)
        {
            if(File.Exists(name))
            {
                using (var archive = ZipFile.Open(name, ZipArchiveMode.Update))
                {
                    string filter = "";
                    if (Globals.Instance.filter != Filter.none)
                    {
                        List<ZipArchiveEntry> deleteList = new List<ZipArchiveEntry>();
                        filter = Globals.Instance.GetFilter();
                        foreach (ZipArchiveEntry file in archive.Entries)
                        {
                            if (Path.GetExtension(file.FullName) == filter)
                            {
                                deleteList.Add(file);
                            }
                        }

                        if (deleteList.Count == 0)
                            Globals.Instance.Log("No files with type " + filter + " were found.", ErrorLevel.warning);

                        for (int i = deleteList.Count - 1; i >= 0; --i)
                        {
                            Globals.Instance.Log("Deleting " + deleteList[i].Name + "\n", ErrorLevel.verbose);
                            deleteList[i].Delete();
                        }
                    }
                    if(fileName != "")
                    {
                        ZipArchiveEntry myEntry = archive.GetEntry(fileName);
                        if (myEntry != null)
                        {
                            myEntry.Delete();
                            Globals.Instance.Log("Deleting " + myEntry.Name + "\n", ErrorLevel.verbose);
                        }
                        else
                            Globals.Instance.Log(fileName + " Not Found", ErrorLevel.error);
                    }
                }
            }
            else
            {
                Globals.Instance.Log(name + " Zip not found.\n", ErrorLevel.error);
            }
        }
        
        public static void AddFile(string name, string target)
        {
            if(File.Exists(name))
            {
                using (var archive = ZipFile.Open(name, ZipArchiveMode.Update))
                {
                     if(File.Exists(target))
                        archive.CreateEntryFromFile(target, Path.GetFileName(target));
                     else
                         Globals.Instance.Log(target + " file not found\n", ErrorLevel.error);
                }
            }
            else
            {
                Globals.Instance.Log(name + " Zip not found.\n", ErrorLevel.error);
            }
        }
    }
}
