using System;
using System.Collections.Generic;
using TempElementsLib.Interfaces;
using System.IO;
using System.Reflection;

namespace TempElementsLib
{
    public class TempElementsList : ITempElements
    {
        private bool disposed;
        private readonly List<ITempElement> elements = new List<ITempElement>();
        string[] bs = { "\\" };

        public IReadOnlyCollection<ITempElement> Elements => elements;

        ~TempElementsList() => throw new NotImplementedException();

        public T AddElement<T>() where T : ITempElement, new()
        {
            ITempElement newFile = new T();
            elements.Add(newFile);
            return (T)newFile;
        }

        public void DeleteElement<T>(T element) where T : ITempElement, new()
        {
            ITempElement foundObject = elements.Find(x => x.Equals(element));
            elements.Remove(foundObject);
            foundObject.Dispose();
        }

        public void MoveElementTo<T>(T element, string newPath) where T : ITempElement, new()
        {
            Type type = element.GetType();

            PropertyInfo propertyInfo;
            if (newPath.Contains("."))
                propertyInfo = type.GetProperty("FilePath");
            else
                propertyInfo = type.GetProperty("DirPath");

            propertyInfo.SetValue(element, newPath);

            if (element is TempDir)
            {
                element.Dispose();
                Directory.CreateDirectory(newPath);
            }
            else
            {
                string oldPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\AppData\\Local\\Temp\\";

                string[] vs = newPath.Split(bs, StringSplitOptions.None);
                string lastItem = vs[vs.Length - 1];

                oldPath += lastItem;

                FieldInfo streamField = type.GetField("Stream");
                object Stream = streamField.GetValue(element);

                MethodInfo close = typeof(FileStream).GetMethod("Close", new Type[] { });
                close.Invoke(Stream, new object[] { });

                MethodInfo moveTo = typeof(File)
                    .GetMethod("Move", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(string) }, null); ;

                moveTo.Invoke(element, new object[] {
                    oldPath,
                    newPath
                });
            }
        }


        public void RemoveDestroyed()
        {
            foreach (var x in elements)
            {
                if (x.IsDestroyed == true) elements.Remove(x); 
            }
            GC.Collect(0);
        }

        public bool IsEmpty() => Elements.Count == 0;

        // public bool IsEmpty => ((ITempElements)this).IsEmpty;


        #region Dispose section ==============================================
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    foreach (ITempElement tmp in elements)
                    {
                        if (tmp is TempFile)
                        {
                            TempFile tempFile = (TempFile)tmp;
                            tempFile.Stream?.Dispose();
                        }
                        else if (tmp is TempTextFile)
                        {
                            TempTextFile tmpTextFile = (TempTextFile)tmp;
                            tmpTextFile.Stream?.Close();
                            tmpTextFile.streamReader?.Close();
                            tmpTextFile.streamWriter?.Close();
                        }
                    }
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                foreach (ITempElement tmp in elements)
                {
                    if (tmp is TempFile)
                    {
                        TempFile tempFile = (TempFile)tmp;
                        tempFile.File?.Delete();
                    }
                    else if (tmp is TempDir)
                    {
                        TempDir tempDir = (TempDir)tmp;
                        tempDir.Dispose();
                    }
                    else
                    {
                        TempTextFile tmpTextFile = (TempTextFile)tmp;
                        tmpTextFile.File?.Delete();
                    }
                }
                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i] = null;
                }
                elements.Clear();
                disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
