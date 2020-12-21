using _2IT.Logic.Directions;
using _2IT.Logic.Priorityes;
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
  /// Логика взаимодействия для CDirectoryUserControl.xaml
  /// </summary>
  public partial class CDirectoryUserControl : UserControl
  {
    private List<CProfileUserControl> _profileUserControls = new List<CProfileUserControl>();

    public delegate void ClickButtonMove(int priority);

    public event ClickButtonMove ButtonMoveUp;

    public event ClickButtonMove ButtonMoveDown;

    public event ClickButtonMove ButtonAdd;

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
              textBoxName.Width = this.Width - textBoxPriority.Width;
              textBoxName.Margin = new Thickness(textBoxPriority.Width, 0, 0, 0);
              break;
            }
          case EMove.Down: {
              buttonUp.Visibility = Visibility.Hidden;
              buttonDown.Visibility = Visibility.Visible;
              textBoxName.Width = this.Width - textBoxPriority.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(textBoxPriority.Width + buttonDown.Width, 0, 0, 0);
              break;
            }
          case EMove.DownUp: {
              buttonUp.Visibility = Visibility.Visible;
              buttonDown.Visibility = Visibility.Visible;
              textBoxName.Width = this.Width - textBoxPriority.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(textBoxPriority.Width + buttonDown.Width, 0, 0, 0);
              break;
            }
          case EMove.Up: {
              buttonUp.Visibility = Visibility.Visible;
              buttonDown.Visibility = Visibility.Hidden;
              textBoxName.Width = this.Width - textBoxPriority.Width - buttonDown.Width;
              textBoxName.Margin = new Thickness(textBoxPriority.Width + buttonDown.Width, 0, 0, 0);
              break;
            }
        }
      }
    }

    private bool _added;

    public bool Added
    {
      get
      {
        return _added;
      }
      set
      {
        _added = value;
        if (value) {
          buttonAdded.Content = "+";
          buttonAdded.Foreground = Brushes.LightGreen;
        } else {
          buttonAdded.Content = "-";
          buttonAdded.Foreground = Brushes.LightPink;
        }
      }
    }

    public int Priority
    {
      get
      {
        return Convert.ToInt32(textBoxPriority.Text);
      }
      set
      {
        if (value != -1) {
          textBoxPriority.Text = value.ToString();
        }
      }
    }

    public CDirectionWithProfile DirectionWithProfile { get; set; }

    public CDirectoryUserControl(CDirectionWithProfile directionWithProfile, int priority, EMove move, bool added)
    {
      InitializeComponent();

      Priority = priority;
      DirectionWithProfile = directionWithProfile;
      textBoxName.Text = DirectionWithProfile.Direction.Code + " «" + DirectionWithProfile.Direction.Name + "»";
      Added = added;
      Load(DirectionWithProfile.PriorityProfile, Added);
      Move = move;
    }

    private void Load(CPriorityProfile priorityProfile, bool added)
    {
      for (int i = 0; i < priorityProfile.GetCount(); i++) {
        CProfileUserControl profileUserControl;
        if (!added) {
          if (priorityProfile.GetCount() == 1) {
            profileUserControl = new CProfileUserControl(priorityProfile.GetProfile(i + 1), EMove.Null, i + 1);
          } else {
            if (i == 0) {
              profileUserControl = new CProfileUserControl(priorityProfile.GetProfile(i + 1), EMove.Down, i + 1);
            } else if (i == priorityProfile.GetCount() - 1) {
              profileUserControl = new CProfileUserControl(priorityProfile.GetProfile(i + 1), EMove.Up, i + 1);
            } else {
              profileUserControl = new CProfileUserControl(priorityProfile.GetProfile(i + 1), EMove.DownUp, i + 1);
            }
          }
        } else {
          profileUserControl = new CProfileUserControl(priorityProfile.GetProfile(i + 1), EMove.Null, i + 1);
        }

        Height = Height + profileUserControl.Height;
        profileUserControl.HorizontalAlignment = HorizontalAlignment.Right;
        profileUserControl.VerticalAlignment = VerticalAlignment.Top;
        profileUserControl.Margin = new Thickness(0, Height - profileUserControl.Height, 0, 0);
        profileUserControl.ButtonMoveUp += CProfileUserControlMoveUp;
        profileUserControl.ButtonMoveDown += CProfileUserControlMoveDown;

        _profileUserControls.Add(profileUserControl);
        ((Grid)this.Content).Children.Add(profileUserControl);
      }
    }

    public void SetResizeWidth(double width)
    {
      if (width < 50) {
        throw new Exception("Элемент направления не может быть меньше 50 пикселей!");
      }

      Width = width;

      textBoxPriority.Width = Width / 10;
      buttonUp.Margin = new Thickness(textBoxPriority.Width, buttonUp.Margin.Top, 0, 0);
      buttonDown.Margin = new Thickness(textBoxPriority.Width, buttonDown.Margin.Top, 0, 0);
      textBoxName.Margin = new Thickness(textBoxPriority.Width + textBoxPriority.Width, 0, 0, 0);
      Move = Move;

      foreach (CProfileUserControl profileUserControl in _profileUserControls) {
        profileUserControl.SetResizeWidth(this.Width * 9 / 10);
        profileUserControl.Margin = new Thickness(Width - profileUserControl.Width, profileUserControl.Margin.Top, 0, 0);
      }

      buttonAdded.Margin = new Thickness(Width - buttonDown.Width, 0, 0, 0);
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

    private void CProfileUserControlMoveUp(int priority)
    {
      CProfileUserControlMove(priority, EStep.Up);
    }

    private void CProfileUserControlMoveDown(int priority)
    {
      CProfileUserControlMove(priority, EStep.Down);
    }

    private void CProfileUserControlMove(int priority, EStep step)
    {
      DirectionWithProfile.PriorityProfile.ChangePriority(step, priority);
      CProfileUserControl buf = _profileUserControls[priority - 1];
      int thisControl = priority - 1;
      int thatControl = priority - 2 + (int)step * 2;

      _profileUserControls[thisControl].Priority = _profileUserControls[thisControl].Priority - 1 + (int)step * 2;
      _profileUserControls[thatControl].Priority = _profileUserControls[thatControl].Priority + 1 - (int)step * 2;

      Thickness thickness = new Thickness(0, _profileUserControls[thisControl].Margin.Top, 0, 0);
      _profileUserControls[thisControl].Margin = new Thickness(0, _profileUserControls[thatControl].Margin.Top * (1 - (int)step) +//Up
        (_profileUserControls[thisControl].Margin.Top + _profileUserControls[thatControl].Height) * (int)step, 0, 0);//Down
      _profileUserControls[thatControl].Margin = new Thickness(0, thickness.Top * (int)step +//Up
        (_profileUserControls[thatControl].Margin.Top + _profileUserControls[thisControl].Height) * (1 - (int)step), 0, 0);//Down

      _profileUserControls[thisControl] = _profileUserControls[priority - 2 + (int)step * 2];
      _profileUserControls[thatControl] = buf;

      GetUpMove(_profileUserControls[thisControl]);
      GetUpMove(_profileUserControls[priority - 2 + (int)step * 2]);
    }

    private void GetUpMove(CProfileUserControl profileUserControl)
    {
      if (!Added) {
        if (_profileUserControls.Count == 1) {
          profileUserControl.Move = EMove.Null;
        } else {
          if (profileUserControl.Priority == 1) {
            profileUserControl.Move = EMove.Down;
          } else if (profileUserControl.Priority == _profileUserControls.Count) {
            profileUserControl.Move = EMove.Up;
          } else {
            profileUserControl.Move = EMove.DownUp;
          }
        }
      }
    }

    private void ButtonAdded_Click(object sender, RoutedEventArgs e)
    {
      try {
        ButtonAdd?.Invoke(DirectionWithProfile.ID);
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
