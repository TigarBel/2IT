using System;

namespace _2IT.Logic.Directions
{
  /// <summary>
  /// Класс направления
  /// </summary>
  public class CDirection
  {
    /// <summary>
    /// Наименование направления
    /// </summary>
    private string _name;
    /// <summary>
    /// Наименование направления
    /// </summary>
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        if (value == null) {
          throw new Exception("Отсутствует наименование направления!");
        }
        if (value == "") {
          throw new Exception("Отсутствует наименование направления!");
        }

        _name = value;
      }
    }
    /// <summary>
    /// Код направления
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="name">Наименование направления</param>
    /// <param name="code">Код направления</param>
    public CDirection(string name, string code)
    {
      Name = name;
      Code = code;
    }
  }
}
