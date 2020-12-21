using _2IT.Logic;
using System;
using System.Collections.Generic;
using System.Data;
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
  /// Логика взаимодействия для CSelectUserWindow.xaml
  /// </summary>
  public partial class CSelectUserWindow : Window
  {
    public bool Accept = false;

    public CSelectUserWindow(DataTable dataTable)
    {
      InitializeComponent();
      Fill(dataTable);
    }

    public CUser GetUser()
    {
      if (!Accept) {
        throw new Exception("Пользователь не был выбран!");
      } else {
        DataRowView row = (DataRowView)dataGrid.SelectedItem;
        CUser user = new CUser(Convert.ToInt32(row[0]));
        user.Name = (string)row[1];
        return user;
      }
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void Fill(DataTable dataTable)
    {
      dataGrid.ItemsSource = dataTable.DefaultView;
    }

    private void ButtonAccept_Click(object sender, RoutedEventArgs e)
    {
      try {
        if(dataGrid.SelectedIndex == -1) {
          throw new Exception("Не выбрана строка с пользователем!");
        }

        Accept = true;
        Close();
      }
      catch(Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
