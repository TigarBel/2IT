using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2IT.Logic.Priorityes
{
  /// <summary>
  /// Перечисление направления
  /// </summary>
  public enum EStep
  {
    /// <summary>
    /// Отсутствие направления
    /// </summary>
    Null = -1,
    /// <summary>
    /// Поднять по приоритету
    /// </summary>
    Up,
    /// <summary>
    /// Опустить по приоритету
    /// </summary>
    Down
  }
}
