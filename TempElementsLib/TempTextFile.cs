using System;
using TempElementsLib.Interfaces;
using System.IO;
using System.Reflection;
using System.Text;

namespace TempElementsLib
{
    public class TempTextFile : TempFile
    {
        public StreamReader streamReader;
        public StreamWriter streamWriter;
        string[] bs = {"\n"};

        public TempTextFile() : base()
        {

        }

        public TempTextFile(string fileName) : base(fileName)
        {
            string[] vs = fileName.Split('.');
            if (vs[vs.Length - 1] != "txt")
            {
                Stream.Close();
                File.MoveTo(Path.ChangeExtension(File.FullName, ".txt"));
            }
        }

        ~TempTextFile() => Dispose(false);

        public void ReadLine()
        {
            using (streamReader = new StreamReader(File.FullName))
            {
                Console.WriteLine(streamReader.ReadLine());
            }
        }

        public void ReadAllText()
        {
            using (streamReader = new StreamReader(File.FullName))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public void Write(string text)
        {
            if (text != null)
            {
                using (streamWriter = new StreamWriter(File.FullName, true))
                {
                    if (text.Contains("\n"))
                    {
                        string[] vs = text.Split(bs, StringSplitOptions.None);
                        foreach (string line in vs)
                        {
                            streamWriter.WriteLine(line);
                        }
                        streamWriter.Flush();
                    }
                    else
                    {
                        throw new ArgumentException("Use instead of 'void WriteLine()'");
                    }
                }
            }
        }

        public void WriteLine(string line)
        {
            if (line != null)
            {
                using (streamWriter = new StreamWriter(File.FullName, true))
                {
                    streamWriter.WriteLine("\n" + line);
                    streamWriter.Flush();
                }
            }
        }
    }
}
