using System;
using System.Collections.Generic;

namespace _2IT.Logic.Priorityes
{
  /// <summary>
  /// Класс списка направлений с приоритетами
  /// </summary>
  public class CPriorityDirection 
  {
    /// <summary>
    /// Список с направлениями с профилями
    /// </summary>
    private List<CDirectionWithProfile> _directionWithProfiles;
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="directionWithProfile">Направление с профилями</param>
    public CPriorityDirection(CDirectionWithProfile directionWithProfile)
    {
      _directionWithProfiles = new List<CDirectionWithProfile>();
      _directionWithProfiles.Add(directionWithProfile);
    }

    /// <summary>
    /// Получить информацию направления
    /// </summary>
    /// <param name="index">Приоритетный номер</param>
    /// <returns>Направление</returns>
    public CDirectionWithProfile GetDirectionWithProfile(int index)
    {
      CDirectionWithProfile directionWithProfile = _directionWithProfiles[index - 1];
      return new CDirectionWithProfile(directionWithProfile.Direction, directionWithProfile.PriorityProfile, directionWithProfile.ID);
    }
    /// <summary>
    /// Получить количество направлений
    /// </summary>
    /// <returns>Количество</returns>
    public int GetCount()
    {
      return _directionWithProfiles.Count;
    }
    /// <summary>
    /// Добавить в список приоритетов направлений
    /// </summary>
    /// <param name="directionWithProfile">Профиль направления</param>
    public void AddDirection(CDirectionWithProfile directionWithProfile)
    {
      _directionWithProfiles.Add(directionWithProfile);
    }
    /// <summary>
    /// Удаляет выбранное направление
    /// </summary>
    /// <param name="index">Приоритетный номер</param>
    public void RemoveProfile(int index)
    {
      if (_directionWithProfiles.Count == 0) {
        throw new Exception("Невозможно удалить выбранное направление из пустого списка!");
      }
      if (index < 1 || index > _directionWithProfiles.Count) {
        throw new Exception("В списке нет выбранного направления с данным приоритетом!");
      }

      _directionWithProfiles.RemoveAt(index - 1);
    }
    /// <summary>
    /// Поменять приоритет направления
    /// </summary>
    /// <param name="step">Шаг в направление</param>
    /// <param name="index">Приоритетный номер</param>
    public void ChangePriority(EStep step, int index)
    {
      if (index < 1 || index > _directionWithProfiles.Count) {
        throw new Exception("Приоритетный номер выходит за границы списка!");
      }

      List<CDirectionWithProfile> localDirectionWithProfiles = new List<CDirectionWithProfile>();
      switch (step) {
        case EStep.Up: {
            if (index == 1) {
              return;
            }

            for (int i = 0; i < _directionWithProfiles.Count; i++) {
              if (i == index - 2) {
                localDirectionWithProfiles.Add(_directionWithProfiles[i + 1]);
                localDirectionWithProfiles.Add(_directionWithProfiles[i]);
                i++;
              } else {
                localDirectionWithProfiles.Add(_directionWithProfiles[i]);
              }
            }
            break;
          }
        case EStep.Down: {
            if (index == _directionWithProfiles.Count) {
              return;
            }

            for (int i = 0; i < _directionWithProfiles.Count; i++) {
              if (i == index - 1) {
                localDirectionWithProfiles.Add(_directionWithProfiles[i + 1]);
                localDirectionWithProfiles.Add(_directionWithProfiles[i]);
                i++;
              } else {
                localDirectionWithProfiles.Add(_directionWithProfiles[i]);
              }
            }
            break;
          }
      }

      _directionWithProfiles = localDirectionWithProfiles;
    }
  }
}
