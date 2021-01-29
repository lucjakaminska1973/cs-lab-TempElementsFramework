using System;
using TempElementsLib;

namespace TempElementsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var tempFile5 = new TempFile("test1.txt"))
            {
                tempFile5.AddText("Pierwszy raz dodaję tekst do pliku test1");
                Console.WriteLine("aby kontynuowac wciśnij dolnyy klawisz");
                Console.ReadKey();
            }

            var tempFile2 = new TempFile("test2.txt");
            tempFile2.Dispose();

            try
            {
                tempFile2.AddText("Teraz dodaję tekst do pliku test2");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var file = new TempFile("test1.txt");
                file.Close();

                file.AddText("drugi raz coś dodaję");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            var tempFile = new TempFile(@"C:\Users\Administrator\Desktop\test3.txt");
            tempFile.Close();
            var txtFile = new TempTextFile(@"C:\Users\Administrator\Desktop\testExtension.csv");

            txtFile.Write("Ala ma kota \n Ola ma psa");
            txtFile.WriteLine("Każdy jakieś hobby ma.");

            txtFile.ReadLine();
            txtFile.ReadAllText();

            txtFile.Close();

            var directory = new TempDir(@"C:\Users\Administrator\Pulpit\TestDirectory");
            directory.Dispose();

            TempElementsList temp;

            using (temp = new TempElementsList())
            {
                TempFile toMove = temp.AddElement<TempFile>();
                temp.AddElement<TempFile>();

                TempTextFile tempTextFile = temp.AddElement<TempTextFile>();
                TempDir tempDir = temp.AddElement<TempDir>();
                temp.AddElement<TempDir>();

                TempFile tempFile3 = temp.AddElement<TempFile>();

                temp.DeleteElement(tempDir);

                tempFile3.Dispose();

                temp.RemoveDestroyed();

                temp.MoveElementTo(toMove, @"C:\Users\Administrator\Pulpit\" + toMove.File.Name);

                temp.MoveElementTo(tempTextFile, @"C:\Users\Administrator\Pulpit\" + tempTextFile.File.Name);
            }


        }
    
    }
}
