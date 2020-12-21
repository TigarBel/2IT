using _2IT.Logic;
using _2IT.Logic.Directions;
using _2IT.Logic.Priorityes;
using _2IT.Logic.Profiles;
using _2IT.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _2IT
{
  /// <summary>
  /// Логика взаимодействия для Main.xaml
  /// </summary>
  public partial class Main : Window
  {
    private SQLiteConnection _sqlite = new SQLiteConnection("Data Source=DB2IT.db");

    private CUser _user;

    private List<СBasketUserControl> _basketUserControls = new List<СBasketUserControl>();

    public Main()
    {
      InitializeComponent();

      Load();
    }

    private void Load()
    {
      DataTable dataTableDirection = SelectQuery("SELECT DIRECTION.ID, DIRECTION.NAME, DIRECTION.CODE FROM DIRECTION");
      DataTable dataTableProfile = SelectQuery("SELECT DIRECTION.ID, PROFILE.NAME, FACULTY.NAME, PROFILE.ID  FROM CATALOG " +
        "JOIN DIRECTION ON CATALOG.ID_DIRECTION = DIRECTION.ID " +
        "JOIN PROFILE ON CATALOG.ID_PROFILE = PROFILE.ID " +
        "JOIN FACULTY ON PROFILE.ID_FACULTY = FACULTY.ID");

      CPriorityDirection priorityDirection;
      List<CDirection> directions = new List<CDirection>();
      List<CProfile> profiles = new List<CProfile>();
      foreach (DataRow dataRowDirection in dataTableDirection.Rows) {
        directions.Add(new CDirection((string)dataRowDirection[1], (string)dataRowDirection[2]));
      }
      foreach (DataRow dataRowProfile in dataTableProfile.Rows) {
        profiles.Add(new CProfile((string)dataRowProfile[1], (string)dataRowProfile[2], Convert.ToInt32(dataRowProfile[3])));
      }

      List<CDirectionWithProfile> directionWithProfiles = new List<CDirectionWithProfile>();
      for (int i = 0; i < profiles.Count; i++) {
        if (directionWithProfiles.Count != 0) {
          if (directionWithProfiles[directionWithProfiles.Count - 1].ID == Convert.ToInt32(dataTableProfile.Rows[i].ItemArray[0])) {
            directionWithProfiles[directionWithProfiles.Count - 1].PriorityProfile.AddProfile(profiles[i]);
            continue;
          }
        }
        CDirection direction = directions[Convert.ToInt32(dataTableProfile.Rows[i].ItemArray[0])];
        CProfile profile = profiles[i];
        int id = Convert.ToInt32(dataTableProfile.Rows[i].ItemArray[0]);
        CDirectionWithProfile directionWithProfile = new CDirectionWithProfile(direction, profile, id);
        directionWithProfiles.Add(directionWithProfile);
      }

      priorityDirection = new CPriorityDirection(directionWithProfiles[0]);
      for (int i = 1; i < directionWithProfiles.Count; i++) {
        priorityDirection.AddDirection(directionWithProfiles[i]);
      }

      СBasketUserControl basketUserControl = new СBasketUserControl(priorityDirection, true);
      basketUserControl.HorizontalAlignment = HorizontalAlignment.Left;
      basketUserControl.VerticalAlignment = VerticalAlignment.Top;
      basketUserControl.Margin = new Thickness(0, 79, 0, 0);

      basketUserControl.SetSizeHorizontsly(390);
      basketUserControl.SetSizeVerticals(Height - 150);

      basketUserControl.ButtonAdd += TransferDirectionWithProfile;
      _basketUserControls.Add(basketUserControl);
      ((Grid)this.Content).Children.Add(basketUserControl);

      basketUserControl = new СBasketUserControl();
      basketUserControl.HorizontalAlignment = HorizontalAlignment.Left;
      basketUserControl.VerticalAlignment = VerticalAlignment.Top;
      basketUserControl.Margin = new Thickness(390, 79, 0, 0);

      basketUserControl.SetSizeHorizontsly(390);
      basketUserControl.SetSizeVerticals(Height - 150);

      basketUserControl.ButtonAdd += TransferDirectionWithProfile;
      _basketUserControls.Add(basketUserControl);
      ((Grid)this.Content).Children.Add(basketUserControl);
    }

    private void ButtonClear_Click(object sender, RoutedEventArgs e)
    {
      try {
        _basketUserControls[1].Clear();
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      try {
        if (_basketUserControls[1].PriorityDirection == null) {
          throw new Exception("Корзина пуста!");
        }

        DataTable dataTable;

        if (textBoxName.IsReadOnly) {
          SelectQuery($"DELETE FROM DIRECTION_SELECTION WHERE DIRECTION_SELECTION.ID_USER = {_user.ID}");
          _user.PriorityDirection = _basketUserControls[1].PriorityDirection;
        } else {
          _user = new CUser(textBoxName.Text, _basketUserControls[1].PriorityDirection);
          SelectQuery($"INSERT INTO USER (NAME) VALUES(\"{_user.Name}\");");
          dataTable = SelectQuery("SELECT USER.ID FROM USER");
          _user.ID = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1].ItemArray[0]);
        }

        for (int i = 0; i < _user.PriorityDirection.GetCount(); i++) {
          SelectQuery($"INSERT INTO DIRECTION_SELECTION (ID_USER, ID_DIRECTION, PRIORITY) " +
            $"VALUES({_user.ID}, {_user.PriorityDirection.GetDirectionWithProfile(i + 1).ID}, {i + 1});");
          for(int j=0;j< _user.PriorityDirection.GetDirectionWithProfile(i + 1).PriorityProfile.GetCount(); j++) {
            dataTable = SelectQuery("SELECT DIRECTION_SELECTION.ID FROM DIRECTION_SELECTION");
            int id = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1].ItemArray[0]);
            SelectQuery($"INSERT INTO PROFILE_SELECTION (ID_SELECTION, ID_PROFILE, PRIORITY) " +
              $"VALUES({id}, {_user.PriorityDirection.GetDirectionWithProfile(i + 1).PriorityProfile.GetProfile(j + 1).ID}, {j + 1});");
          }
        }

        MessageBox.Show($"Пользователь под именем: «{_user.Name}», под индивидуальным номером: {_user.ID}, сохранен.");
        textBoxName.IsReadOnly = true;
        textBoxName.Text = $"{_user.ID} {_user.Name}";
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonLoad_Click(object sender, RoutedEventArgs e)
    {
      try {
        CSelectUserWindow selectUserWindow = new CSelectUserWindow(SelectQuery("SELECT * FROM USER"));
        selectUserWindow.ShowDialog();
        if (selectUserWindow.Accept) {
          if (_basketUserControls[1].PriorityDirection != null) {
            _basketUserControls[1].Clear();
          }

          _user = selectUserWindow.GetUser();
          textBoxName.Text = _user.ID + " " + _user.Name;
          textBoxName.IsReadOnly = true;

          DataTable dataTableDirection = SelectQuery($"SELECT DIRECTION_SELECTION.ID, DIRECTION_SELECTION.PRIORITY, DIRECTION.NAME, DIRECTION.CODE, " +
            $"DIRECTION.ID AS DIRECTION_ID FROM DIRECTION_SELECTION, DIRECTION WHERE DIRECTION_SELECTION.ID_USER = { _user.ID} AND " +
            $"DIRECTION.ID = DIRECTION_SELECTION.ID_DIRECTION");
          DataTable dataTableProfile = SelectQuery($"SELECT PROFILE_SELECTION.ID, PROFILE_SELECTION.PRIORITY, PROFILE.NAME AS PROFILE_NAME, " +
            $"FACULTY.NAME AS FACULTY_NAME, PROFILE.ID AS PROFILE_ID, DIRECTION_SELECTION.ID AS DIRECTION_SELECTION_ID  FROM DIRECTION_SELECTION, " +
            $"PROFILE_SELECTION, PROFILE, FACULTY WHERE DIRECTION_SELECTION.ID_USER = { _user.ID} AND DIRECTION_SELECTION.ID = PROFILE_SELECTION.ID_SELECTION AND " +
            $"PROFILE_SELECTION.ID_PROFILE = PROFILE.ID AND PROFILE.ID_FACULTY = FACULTY.ID");

          int iCount = 0;
          foreach (DataRow dataRow in dataTableDirection.Rows) {
            CDirection direction = new CDirection((string)dataRow[2], (string)dataRow[3]);
            DataRow row = dataTableProfile.Rows[iCount];
            CProfile profile = new CProfile((string)row[2], (string)row[3], Convert.ToInt32(row[4]));
            CDirectionWithProfile directionWithProfile = new CDirectionWithProfile(direction, profile, Convert.ToInt32(dataRow[4]));

            while (iCount < dataTableProfile.Rows.Count - 1) {
              if (Convert.ToInt32(dataRow[0]) == Convert.ToInt32(dataTableProfile.Rows[iCount + 1].ItemArray[5])) {
                iCount++;
                row = dataTableProfile.Rows[iCount];
                profile = new CProfile((string)row[2], (string)row[3], Convert.ToInt32(row[4]));
                directionWithProfile.PriorityProfile.AddProfile(profile);
              } else {
                iCount++;
                break;
              }
            }

            _basketUserControls[1].Add(directionWithProfile, false);
          }
        }
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void TransferDirectionWithProfile(CDirectionWithProfile directionWithProfile)
    {
      if (_basketUserControls[1].PriorityDirection != null) {
        for (int i = 0; i < _basketUserControls[1].PriorityDirection.GetCount(); i++) {
          if (_basketUserControls[1].PriorityDirection.GetDirectionWithProfile(i + 1).ID == directionWithProfile.ID) {
            throw new Exception("Данное направление уже лежит в корзине!");
          }
        }
      }

      _basketUserControls[1].Add(directionWithProfile, false);
    }

    /// <summary>
    /// Выполнить SQL запрос и получить таблицу с данными
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <returns>Таблица данных</returns>
    private DataTable SelectQuery(string query)
    {
      SQLiteDataAdapter sQLiteDataAdapter;
      DataTable dataTable = new DataTable();

      try {
        SQLiteCommand command;
        _sqlite.Open();  //Initiate connection to the db
        command = _sqlite.CreateCommand();
        command.CommandText = query;  //set the passed query
        sQLiteDataAdapter = new SQLiteDataAdapter(command);
        sQLiteDataAdapter.Fill(dataTable); //fill the datasource
      } catch (SQLiteException ex) {
        MessageBox.Show(ex.Message);
      }
      _sqlite.Close();
      return dataTable;
    }

    private void TextBoxName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (((TextBox)sender).IsReadOnly) {
        _user = null;
        ((TextBox)sender).Text = "Имя абитуриента";
        ((TextBox)sender).IsReadOnly = false;
      }
    }
  }
}
