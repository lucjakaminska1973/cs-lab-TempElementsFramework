using System;


namespace TempElementsLib.Interfaces
{
    /// <summary>
    /// Reprezentuje tymczasowy plik
    /// </summary>
    /// <remarks>
    /// konstruktor bezargumentowy        -> tworzenie pliku w domyslnej dla systemu/użytkownika lokalizacji i o losowej nazwie
    ///                                      Path.GetTempFileName
    /// konstruktor z argumentem `string` -> tworzenie pliku we wskazanym miejscu i o wskazanej nazwie
    /// </remarks>
    interface ITempFile : ITempElement
    {
        // property zwracające pełną ścieżkę dostępu do pliku
        string FilePath { get; }
    }
}
