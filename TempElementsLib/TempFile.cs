using System;
using TempElementsLib.Interfaces;
using System.IO;
using System.Text;

namespace TempElementsLib
{
    public class TempFile : ITempFile
    {
        public readonly FileStream Stream;
        public readonly FileInfo File;

        public string FilePath { get; set; }
        public bool IsDestroyed { get; set; }

        string[] bs = { "\\" };

        public TempFile()
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + ".txt";
            File = new FileInfo(Path.Combine(path, fileName));
            Stream = new FileStream(File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            IsDestroyed = false;
        }

        public TempFile(string fileName)
        {

            if (fileName.Contains("\\"))
            {
                if (Uri.IsWellFormedUriString(fileName, UriKind.Absolute))
                    throw new InvalidDataException("Path is incorrect.");

                string[] splitArray = fileName.Split(bs, StringSplitOptions.None);
                for (int i = 0; i < splitArray.Length - 1; i++)
                {
                    if (i == splitArray.Length - 2)
                    {
                        FilePath += splitArray[i];
                    }
                    else
                    {
                        FilePath += splitArray[i] + @"\";
                    }
                }
                fileName = splitArray[splitArray.Length - 1];
            }
            else
            {
                FilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Temp\\";
            }
            var dir = new DirectoryInfo(FilePath);
            File = new FileInfo(Path.Combine(dir.FullName, fileName));
            Stream = new FileStream(
                File.FullName,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite);
            IsDestroyed = false;
        }

        ~TempFile() => Dispose(false);

        public void AddText(string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            Stream.Write(info, 0, info.Length);
            Stream.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
                Stream?.Dispose();
            File?.Delete();
            Console.WriteLine($"File {File.FullName} deleted.");
            IsDestroyed = true;
        }

        public void Close() => Dispose();
    }
}
