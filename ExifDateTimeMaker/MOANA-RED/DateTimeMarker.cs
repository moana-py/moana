using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ExifLibrary;

namespace ExifDateTimeMaker
{
    public class DateTimeMarker
    {
        string rootDirectory = "";
        string outputDirectory = "";
        string dateTimeMode = "";
        bool recursive = false;

        public DateTimeMarker(string d, string m, bool r, string od)
        {
            rootDirectory = d;
            dateTimeMode = m;
            recursive = r;
            outputDirectory = od;
        }
 
        public void Execute()
        {
            SetExifDateTimdOriginal(rootDirectory);
        }

        private void SetExifDateTimdOriginal(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine(directory + " - directory is not exist.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Console.WriteLine(outputDirectory + " - directory is not exist. ignore /to option.");
                outputDirectory = "";
            }

            Console.WriteLine("\nDirectory = " + directory);

            try
            {
                foreach (var f in Directory.GetFiles(directory))
                {
                    try
                    {
                        ImageFile image = ImageFile.FromFile(f);
                        FileInfo info = new FileInfo(f);
                        if (image.Format == ImageFileFormat.JPEG)
                        {
                            var original = image.Properties[ExifTag.DateTimeOriginal];
                            if (original == null)
                            {
                                DateTime fileTime = GetFileTime(info, dateTimeMode);
                                if (fileTime != DateTime.MinValue)
                                {
                                    image.Properties.Set(ExifTag.DateTimeOriginal, fileTime);
                                    
                                    Console.WriteLine(info.Name + " - is successful");
                                    image.Save(f);
                                }
                            }
                            else
                            {
                                Console.WriteLine(info.Name + " is skipped - Exif is exists.");
                            }
     
                            if (!string.IsNullOrWhiteSpace(outputDirectory))
                            {
                                CopyToFolder(outputDirectory, (DateTime)image.Properties[ExifTag.DateTimeOriginal].Value, info);
                            }
                        }
                        else
                        {
                            Console.WriteLine(info.Name + " is skipped - Type is not jpg");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("failed - " + e.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("finding file is failed - " + e.Message);
            }
            
            if (recursive)
            {
                try
                {
                    foreach (var d in Directory.GetDirectories(directory))
                    {
                        SetExifDateTimdOriginal(d);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("finding directory is failed - " + e.Message);
                }
            }
        }

        private DateTime GetFileTime(FileInfo info, string dateTimeMode)
        {
            DateTime fileTime = new DateTime();

            try
            {
                switch (dateTimeMode)
                {
                    case "CT":
                        fileTime = info.CreationTime;
                        break;
                    case "MT":
                        fileTime = info.LastWriteTime;
                        break;
                    case "AT":
                        fileTime = info.LastAccessTime;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("reading file DateTime is failed - " + e.Message);
            }

            return fileTime;
        }

        private void CopyToFolder(string outputDirectory, DateTime fileTime, FileInfo info)
        {
            if (Directory.Exists(outputDirectory) == false)
                return;
            try
            {
                string dir = string.Format(@"{0}\{1}년\{1}년 {2}월", outputDirectory, fileTime.Year, fileTime.Month);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                int count = 1;
                string fileName = Path.GetFileNameWithoutExtension(info.FullName);

                while (File.Exists($"{dir}\\{fileName}{info.Extension}"))
                {
                    fileName = string.Format("{0} ({1})", fileName, count++);
                }
                if (count > 1)
                    Console.WriteLine(string.Format("{0} - already exists. Copy by replacing it with a {1}.", info.Name, fileName + info.Extension));
                else
                    Console.WriteLine(string.Format("{0} - copy is successful.", info.Name));

                File.Copy(info.FullName, Path.Combine(dir, fileName + info.Extension), true);
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to copy file - " + e.Message);
            }
        }
    }
}
