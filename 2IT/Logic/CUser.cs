using _2IT.Logic.Priorityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2IT.Logic
{
  /// <summary>
  /// Класс пользователя
  /// </summary>
  public class CUser
  {
    /// <summary>
    /// Индивидуальный номер в БД
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Наименование пользователя
    /// </summary>
    private string _name;
    /// <summary>
    /// Наименование пользователя
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
          throw new Exception("Отсутствует наименование пользователя!");
        }
        if (value == "") {
          throw new Exception("Отсутствует наименование пользователя!");
        }

        _name = value;
      }
    }
    /// <summary>
    /// Список направлений с приоритетами
    /// </summary>
    public CPriorityDirection PriorityDirection { get; set; }

    /// <summary>
    /// Конструктор класса при создании и загрузки в БД
    /// </summary>
    /// <param name="name">Именование пользователя</param>
    /// <param name="priorityDirection">Список направлений с приоритетами</param>
    public CUser(string name, CPriorityDirection priorityDirection)
    {
      Name = name;
      PriorityDirection = priorityDirection;
    }

    /// <summary>
    /// Конструктор класса при копировании из БД
    /// </summary>
    /// <param name="id">Индивидуальный номер в БД</param>
    public CUser(int id)
    {
      ID = id;
    }
  }
}
