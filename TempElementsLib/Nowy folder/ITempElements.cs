using System;
using System.Collections.Generic;
using System.Text;

namespace TempElementsLib.Interfaces
{
    /// <summary>
    /// Reprezentuje kolekcję elementów tymczasowych
    /// </summary>
    /// <remarks>
    /// Utworzenie i zarejestrowanie elementu tymczasowego poprzez użycie 
    /// generycznej metody `AddElement<T>()`. Jako typ `T` należy określić
    /// konkretną implementację klasy `ITempElements`. Warunki nałożone na typ `T`
    /// wymuszają zastosowanie konstruktorów bezargumentowych dla referencyjnego typu `T`.
    /// 
    /// Zarejestrowane elementy mogą mieć zwalniane zasoby "bez wiedzy" kolekcji.
    /// Do usuwania z listy elementów, które zwolniły już zasoby służy metoda `RemoveDestroyed()`
    /// 
    /// Wywołanie `Dispose` powoduje zwolnienie wszystkich zasobów powiązanych
    /// z zapamiętanymi elementami w kolekcji i opróżnienie kolekcji.
    /// </remarks>
    public interface ITempElements : IDisposable
    {
        // dostęp do kolekcji elementów
        IReadOnlyCollection<ITempElement> Elements { get; }

        // tworzy nowy element tymczasowy (domyślnie) i rejestruje go w kolekcji,
        // zwraca referencję do utworzonego elementu
        T AddElement<T>() where T : ITempElement, new();

        // usuwa z kolekcji elementy, które już zwolniły zasoby (dla nich `IsDestroyed` zwraca true)
        // i kompaktuje kolekcję
        void RemoveDestroyed();

        // `true` jeśli kolekcja jest pusta, `false` w przeciwnym przypadku
        // default implementation C# 8
        bool IsEmpty => (Elements.Count == 0);
    }
}
