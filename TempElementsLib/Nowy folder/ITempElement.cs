using System;
using System.Collections.Generic;
using System.Text;

namespace TempElementsLib.Interfaces
{
    /// <summary>
    /// Reprezentuje element o charakterze tymczasowym, usuwany wtedy, gdy przestaje być potrzebny (`Dispose`)
    /// </summary>
    public interface ITempElement : IDisposable
    {
        bool IsDestroyed { get; } //true, jeśli element skutecznie usunięty
    }
}
