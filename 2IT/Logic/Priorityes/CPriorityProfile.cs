using _2IT.Logic.Profiles;
using System;
using System.Collections.Generic;

namespace _2IT.Logic.Priorityes
{
  /// <summary>
  /// Класс со списком профилей по приоритету
  /// </summary>
  public class CPriorityProfile
  {
    /// <summary>
    /// Список профилей
    /// </summary>
    private List<CProfile> _profiles;
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="profile">Профиль направления</param>
    public CPriorityProfile(CProfile profile)
    {
      _profiles = new List<CProfile>();
      _profiles.Add(profile);
    }
    /// <summary>
    /// Получить информацию профиля
    /// </summary>
    /// <param name="index">Приоритетный номер</param>
    /// <returns>Профиль</returns>
    public CProfile GetProfile(int index)
    {
      CProfile profile = _profiles[index - 1];
      return new CProfile(profile.Name, profile.Faculty, profile.ID);
    }
    /// <summary>
    /// Получить количество профилей
    /// </summary>
    /// <returns>Количество</returns>
    public int GetCount()
    {
      return _profiles.Count;
    }
    /// <summary>
    /// Добавить в список приоритетов профиль
    /// </summary>
    /// <param name="profile">Профиль направления</param>
    public void AddProfile(CProfile profile)
    {
      _profiles.Add(profile);
    }
    /// <summary>
    /// Удаляет выбранный профиль направления
    /// </summary>
    /// <param name="index">Приоритетный номер</param>
    public void RemoveProfile(int index)
    {
      if(_profiles.Count == 0) {
        throw new Exception("Невозможно удалить выбранный профиль из пустого списка!");
      }
      if(index < 1 || index > _profiles.Count) {
        throw new Exception("В списке нет выбранного профиля с данным приоритетом!");
      }

      _profiles.RemoveAt(index - 1);
    }
    /// <summary>
    /// Поменять приоритет профиля
    /// </summary>
    /// <param name="step">Шаг в направление</param>
    /// <param name="index">Приоритетный номер</param>
    public void ChangePriority(EStep step, int index)
    {
      if(index < 1 || index > _profiles.Count) {
        throw new Exception("Приоритетный номер выходит за границы списка!");
      }

      List<CProfile> localProfiles = new List<CProfile>();
      switch (step) {
        case EStep.Up: {
            if (index == 1) {
              return;
            }

            for (int i = 0; i < _profiles.Count; i++) {
              if (i == index - 2) {
                localProfiles.Add(_profiles[i + 1]);
                localProfiles.Add(_profiles[i]);
                i++;
              } else {
                localProfiles.Add(_profiles[i]);
              }
            }
            break;
          }
        case EStep.Down: {
            if (index == _profiles.Count) {
              return;
            }

            for (int i = 0; i < _profiles.Count; i++) {
              if (i == index - 1) {
                localProfiles.Add(_profiles[i + 1]);
                localProfiles.Add(_profiles[i]);
                i++;
              } else {
                localProfiles.Add(_profiles[i]);
              }
            }
            break;
          }
      }

      _profiles = localProfiles;
    }
  }
}
