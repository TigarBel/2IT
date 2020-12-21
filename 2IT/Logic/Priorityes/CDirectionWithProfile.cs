using _2IT.Logic.Directions;
using _2IT.Logic.Profiles;
using System.Collections.Generic;

namespace _2IT.Logic.Priorityes
{
  /// <summary>
  /// Направление с профилями
  /// </summary>
  public class CDirectionWithProfile
  {
    /// <summary>
    /// Направление
    /// </summary>
    private CDirection _direction;
    /// <summary>
    /// Направление
    /// </summary>
    public CDirection Direction
    {
      get
      {
        return new CDirection(_direction.Name, _direction.Code);
      }
      set
      {
        _direction = new CDirection(value.Name, value.Code);
      }
    }
    /// <summary>
    /// Список профилей с приоритетами
    /// </summary>
    private CPriorityProfile _priorityProfile;
    /// <summary>
    /// Список профилей с приоритетами
    /// </summary>
    public CPriorityProfile PriorityProfile
    {
      get
      {
        return _priorityProfile;
      }
    }
    /// <summary>
    /// Номер индификации
    /// </summary>
    private int _id;
    /// <summary>
    /// Номер индификации
    /// </summary>
    public int ID
    {
      get { return _id; }
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="direction">Направление</param>
    /// <param name="profile">Профиль</param>
    /// <param name="id">Номер индификации</param>
    public CDirectionWithProfile(CDirection direction, CProfile profile, int id)
    {
      _direction = direction;
      _priorityProfile = new CPriorityProfile(profile);
      _id = id;
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="direction">Направление</param>
    /// <param name="profiles">Список профилей</param>
    public CDirectionWithProfile(CDirection direction, CPriorityProfile priorityProfile, int id)
    {
      _direction = direction;
      _priorityProfile = priorityProfile;
      _id = id;
    }
  }
}
