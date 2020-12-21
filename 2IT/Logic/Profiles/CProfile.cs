using System;

namespace _2IT.Logic.Profiles
{
  /// <summary>
  /// Профиль направления
  /// </summary>
  public class CProfile
  {
    /// <summary>
    /// Наименование профиля
    /// </summary>
    private string _name;
    /// <summary>
    /// Наименование профиля
    /// </summary>
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        if(value == null) {
          throw new Exception("Отсутствует наименование профиля!");
        }
        if(value == "") {
          throw new Exception("Отсутствует наименование профиля!");
        }

        _name = value;
      }
    }
    /// <summary>
    /// Факультет
    /// </summary>
    private string _faculty;
    /// <summary>
    /// Факультета
    /// </summary>
    public string Faculty
    {
      get
      {
        return _faculty;
      }
      set
      {
        if (value == null) {
          throw new Exception("Отсутствует наименование факультета!");
        }
        if (value == "") {
          throw new Exception("Отсутствует наименование факультета!");
        }

        _faculty = value;
      }
    }
    /// <summary>
    /// Индификационный номер
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="name">Именование профиля</param>
    /// <param name="faculty">Именование факультета</param>
    /// <param name="id">Индификационный номер</param>
    public CProfile(string name, string faculty, int id)
    {
      Name = name;
      Faculty = faculty;
      ID = id;
    }
  }
}
