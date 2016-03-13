using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

//namespace CompressionDemo
//{
//    //https://msdn.microsoft.com/library/dd576348(v=vs.100).aspx
//    class Program
//    {
//        static void Main( string[] args )
//        {
//        }
//    }

//    class gZip
//    {
//        public static void DirCompress( DirectoryInfo directorySelected, string filter )
//        {
//            foreach( FileInfo fileToCompress in directorySelected.GetFiles() )
//            { 
//                using(FileStream originalFileStream = fileToCompress.OpenRead())
//                {
//                    if((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden) && fileToCompress.Extension != filter)
//                    {
//                        using (FileStream CompressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
//                        {
//                            using (GZipStream compressionStream = new GZipStream( CompressedFileStream, CompressionMode.Compress))
//                            {
//                                originalFileStream.CopyTo(compressionStream);
//                            }
//                        }
//                    }
//                } 
//            }
//        }
//    }

//    class Zip
//    {
//        static void SimpleZip( string dirToZip, string zipName )
//        {
//            ZipFile.CreateFromDirectory(dirToZip, zipName);
//        }

//        static void SimpleZip(string dirToZip, string zipName, CompressionLevel compressLvl, bool includeRoot)
//        {
//            ZipFile.CreateFromDirectory( dirToZip, zipName, compressLvl, includeRoot );
        
//        }
//        static void SimpleUnzip( string zipName, string dir )
//        {
//            ZipFile.ExtractToDirectory( zipName, dir );
//        }
//        static voi SmartUnzip( string name, string dir )
//        {
//            string fileUnzipFullPath;
//            string fileUnzipFullName;

//            using( ZipArchive archive = ZipFile.OpenRead( name ) )
//            {
//                foreach( ZipArchiveEntry file in archive.Entries )
//                {
//                    Console.WriteLine( "File Name: {0}", file.Name );
//                    Console.WriteLine( "File Size: {0} Bytes", file.Length );
//                    Console.WriteLine( "Compression Ratio: {0}");

//                    fileUnzipFullName = Path.Combine( dir, file.FullName );
//                    if( !System.IO.File.Exists( fileUnzipFullName ) )
//                    {
//                        fileUnzipFullPath = Path.GetDirectoryName( fileUnzipFullName );
//                        Directory.CreateDirectory( fileUnzipFullPath );
//                        file.ExtractToFile( fileUnzipFullName)
                    
//                    }

                
//                }
            
//            }
//        }

//        public enum Overwrite
//        {
//            Always,
//            IfNewer,
//            Never
//        }
//        public enum ArchiveAction
//        {
//            Merge,
//            Replace,
//            Error,
//            Ignore
//        }
//        static void improvedExtractToFile( ZipArchiveEntry file, string destPath, Overwrite owMethod = Overwrite.IfNewer )
//        {
//            string destFilename = Path.Combine( destPath, file.FullName );
//            string destFilePath = Path.GetDirectoryName( destFilename );
//            Directory.CreateDirectory( destFilePath );
//            switch( owMethod )
//            { 
//            case Overwrite.Always:
//            file.ExtractToFile( destinationFileName, true );
//            break;
//            case Overwrite.IfNewer:
//            if( !File.Exists( destFilename ) || File.GetLastWriteTime(destFilename) < file.LastWriteTime)
//            {
//                file.ExtractToFile( destFilename, true );
//            }
//            break;
//            case Overwrite.Never:
//            if( !File.Exists( destFilename ) )
//            {
//                file.ExtractToFile( destinationFileName );  
//            }
//            break;
//            default:
//            break;
            
//            }
//        }
//    }
//}
