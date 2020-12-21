using _2IT.Logic.Profiles;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2IT.UserControls
{
  /// <summary>
  /// Логика взаимодействия для CProfileUserControl.xaml
  /// </summary>
  public partial class CProfileUserControl : UserControl
  {
    private CProfile _profile;

    public delegate void ClickButtonMove(int priority);

    public event ClickButtonMove ButtonMoveUp;

    public event ClickButtonMove ButtonMoveDown;

    private EMove _move;

    public EMove Move
    {
      get
      {
        return _move;
      }
      set
      {
        _move = value;
        switch (value) {
          case EMove.Null: {
              buttonUp.Visibility = Visibility.Hidden;
              buttonDown.Visibility = Visibility.Hidden;
              textBoxName.Width = this.Width - textBoxFaculty.Width;
              textBoxName.Margin = new Thickness(0, 0, 0, 0);
              break;
            }
          case EMove.Down: {
              buttonUp.Visibility = Visibility.Hidden;
              buttonDown.Visibility = Visibility.Visible;
              textBoxName.Width = this.Width - textBoxFaculty.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(buttonDown.Width, 0, 0, 0);
              break;
            }
          case EMove.DownUp: {
              buttonUp.Visibility = Visibility.Visible;
              buttonDown.Visibility = Visibility.Visible;
              textBoxName.Width = this.Width - textBoxFaculty.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(buttonDown.Width, 0, 0, 0);
              break;
            }
          case EMove.Up: {
              buttonUp.Visibility = Visibility.Visible;
              buttonDown.Visibility = Visibility.Hidden;
              textBoxName.Width = this.Width - textBoxFaculty.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(buttonDown.Width, 0, 0, 0);
              break;
            }
        }
      }
    }

    public int Priority { get; set; }

    public CProfileUserControl(CProfile profile, EMove move, int priority)
    {
      InitializeComponent();
      
      textBoxName.Text = profile.Name;
      textBoxFaculty.Text = profile.Faculty.ToString();
      _profile = profile;
      Move = move;
      Priority = priority;
    }

    public void SetResizeWidth(double width)
    {
      Width = width;
      textBoxFaculty.Width = Width / 5;
      textBoxName.Width = Width - textBoxFaculty.Width;
      textBoxFaculty.Margin = new Thickness(textBoxName.Width, 0, 0, 0);
    }

    public CProfile GetProfile()
    {
      return new CProfile(textBoxName.Text, textBoxFaculty.Text, _profile.ID);
    }

    private void ButtonUp_Click(object sender, RoutedEventArgs e)
    {
      try {
        ButtonMoveUp?.Invoke(Priority);
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonDown_Click(object sender, RoutedEventArgs e)
    {
      try {
        ButtonMoveDown?.Invoke(Priority);
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

  }
}
