using System;
using System.Collections.Generic;
using TempElementsLib.Interfaces;
using System.IO;
using System.Reflection;

namespace TempElementsLib
{
    public class TempDir : ITempDir
    {
        public string DirPath { get; set; }

        public bool IsEmpty =>
            Directory.GetFiles(DirPath, "*", SearchOption.TopDirectoryOnly).Length == 0;

        public bool IsDestroyed { get; set; }

        public TempDir()
        {
            DirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Temp\\";
            var folderName = Guid.NewGuid().ToString();
            DirPath += folderName;
            Console.WriteLine(folderName);
            Directory.CreateDirectory(DirPath);
            IsDestroyed = false;
        }

        public TempDir(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                if (Uri.IsWellFormedUriString(fileName, UriKind.Absolute))
                    throw new InvalidDataException("Path is incorrect.");

                DirPath = fileName;
                if (Directory.Exists(DirPath))
                {
                    Console.WriteLine("Given directory already exist!");
                    return;
                }
            }
            else
            {
                DirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Temp\\";
            }
            Directory.CreateDirectory(DirPath);
            IsDestroyed = false;
        }

        ~TempDir() => Dispose();

        public void Dispose()
        {
            Directory.Delete(DirPath);
            System.GC.SuppressFinalize(this);
        }

        public void Empty()
        {
            DirectoryInfo dir = new DirectoryInfo(DirPath);

            foreach (FileInfo file in dir.GetFiles())
                file.Delete();

            foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
                directoryInfo.Delete(true);
        }
    }
}
