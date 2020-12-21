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
  /// Логика взаимодействия для СBasketUserControl.xaml
  /// </summary>
  public partial class СBasketUserControl : UserControl
  {
    private List<CDirectoryUserControl> _directoryUserControls = new List<CDirectoryUserControl>();

    private double _heightDirectoryUserControls = 0;

    public delegate void ClickButtonAdd(CDirectionWithProfile directionWithProfile);

    public event ClickButtonAdd ButtonAdd;

    public CPriorityDirection PriorityDirection;

    public СBasketUserControl()
    {
      InitializeComponent();
    }

    public СBasketUserControl(CPriorityDirection priorityDirection, bool notModify)
    {
      InitializeComponent();

      for (int i = 0; i < priorityDirection.GetCount(); i++) {
        Add(priorityDirection.GetDirectionWithProfile(i + 1), notModify);
      }
    }

    public void SetSizeVerticals(double height)
    {
      Height = height;
      scrollBar.Height = height;

      if (_heightDirectoryUserControls > Height) {
        scrollBar.Maximum = _heightDirectoryUserControls - Height;
      }
    }

    public void SetSizeHorizontsly(double width)
    {
      Width = width;

      foreach (CDirectoryUserControl directoryUserControl in _directoryUserControls) {
        directoryUserControl.SetResizeWidth(Width - scrollBar.Width);
      }
    }

    public void Clear()
    {
      if (_directoryUserControls.Count == 0) {
        throw new Exception("Корзина уже пуста!");
      }

      foreach (CDirectoryUserControl directoryUserControl in _directoryUserControls) {
        ((Grid)this.Content).Children.Remove(directoryUserControl);
      }
      _directoryUserControls.Clear();
      PriorityDirection = null;
    }

    public bool NotModify { get; set; }

    public void Add(CDirectionWithProfile directionWithProfile, bool notModify)
    {
      _heightDirectoryUserControls = 0;
      NotModify = notModify;

      if (PriorityDirection == null) {
        PriorityDirection = new CPriorityDirection(directionWithProfile);
      } else {
        PriorityDirection.AddDirection(directionWithProfile);

        foreach(CDirectoryUserControl directoryUserControl in _directoryUserControls) {
          ((Grid)this.Content).Children.Remove(directoryUserControl);
        }
        _directoryUserControls.Clear();
      }

      double height = 0;
      for (int i = 0; i < PriorityDirection.GetCount(); i++) {
        CDirectionWithProfile localDirectionWithProfile = PriorityDirection.GetDirectionWithProfile(i + 1);
        CDirectoryUserControl directoryUserControl;
        if (NotModify) {
          directoryUserControl = new CDirectoryUserControl(localDirectionWithProfile, -1, EMove.Null, NotModify);
        } else {
          if (PriorityDirection.GetCount() == 1) {
            directoryUserControl = new CDirectoryUserControl(localDirectionWithProfile, i + 1, EMove.Null, NotModify);
          } else {
            if (i == 0) {
              directoryUserControl = new CDirectoryUserControl(localDirectionWithProfile, i + 1, EMove.Down, NotModify);
            } else if (i == PriorityDirection.GetCount() - 1) {
              directoryUserControl = new CDirectoryUserControl(localDirectionWithProfile, i + 1, EMove.Up, NotModify);
            } else {
              directoryUserControl = new CDirectoryUserControl(localDirectionWithProfile, i + 1, EMove.DownUp, NotModify);
            }
          }
        }

        directoryUserControl.HorizontalAlignment = HorizontalAlignment.Left;
        directoryUserControl.VerticalAlignment = VerticalAlignment.Top;
        directoryUserControl.Margin = new Thickness(0, height, 0, 0);
        height = height + directoryUserControl.Height;
        directoryUserControl.ButtonMoveUp += DirectoryUserControlMoveUp;
        directoryUserControl.ButtonMoveDown += DirectoryUserControlMoveDown;

        if (Width > 0) {
          directoryUserControl.SetResizeWidth(Width - scrollBar.Width); ;
        }
        _directoryUserControls.Add(directoryUserControl);
        _heightDirectoryUserControls = _heightDirectoryUserControls + directoryUserControl.Height;
        directoryUserControl.ButtonAdd += AddIntoBasket;
        ((Grid)this.Content).Children.Add(directoryUserControl);
      }

      if (_heightDirectoryUserControls > Height) {
        scrollBar.Maximum = _heightDirectoryUserControls - Height;
      }

    }

    private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
    {
      _heightDirectoryUserControls = 0;
      foreach (CDirectoryUserControl directoryUserControl in _directoryUserControls) {
        directoryUserControl.Margin = new Thickness(0, _heightDirectoryUserControls - scrollBar.Value, 0, 0);
        _heightDirectoryUserControls = _heightDirectoryUserControls + directoryUserControl.Height;
      }
    }

    private void DirectoryUserControlMoveUp(int priority)
    {
      DirectoryUserControlMove(priority, EStep.Up);
    }

    private void DirectoryUserControlMoveDown(int priority)
    {
      DirectoryUserControlMove(priority, EStep.Down);
    }

    private void DirectoryUserControlMove(int priority, EStep step)
    {
      PriorityDirection.ChangePriority(step, priority);
      CDirectoryUserControl buf = _directoryUserControls[priority - 1];
      int thisControl = priority - 1;
      int thatControl = priority - 2 + (int)step * 2;

      _directoryUserControls[thisControl].Priority = _directoryUserControls[thisControl].Priority - 1 + (int)step * 2;
      _directoryUserControls[thatControl].Priority = _directoryUserControls[thatControl].Priority + 1 - (int)step * 2;

      Thickness thickness = new Thickness(0, _directoryUserControls[thisControl].Margin.Top, 0, 0);
      _directoryUserControls[thisControl].Margin = new Thickness(0, _directoryUserControls[thatControl].Margin.Top * (1 - (int)step) +//Up
        (_directoryUserControls[thisControl].Margin.Top + _directoryUserControls[thatControl].Height) * (int)step, 0, 0);//Down
      _directoryUserControls[thatControl].Margin = new Thickness(0, thickness.Top * (int)step +//Up
        (_directoryUserControls[thatControl].Margin.Top + _directoryUserControls[thisControl].Height) * (1 - (int)step), 0, 0);//Down

      _directoryUserControls[thisControl] = _directoryUserControls[priority - 2 + (int)step * 2];
      _directoryUserControls[thatControl] = buf;

      GetUpMove(_directoryUserControls[thisControl]);
      GetUpMove(_directoryUserControls[priority - 2 + (int)step * 2]);
    }

    private void GetUpMove(CDirectoryUserControl directoryUserControl)
    {
      if (!NotModify){
        if (_directoryUserControls.Count > 1) {
          if (directoryUserControl.Priority == 1) {
            directoryUserControl.Move = EMove.Down;
          } else if (directoryUserControl.Priority == _directoryUserControls.Count) {
            directoryUserControl.Move = EMove.Up;
          } else {
            directoryUserControl.Move = EMove.DownUp;
          }
        } else {
          directoryUserControl.Move = EMove.Null;
        }
      }
    }

    private void AddIntoBasket(int id)
    {
      CDirectoryUserControl directoryUserControl = _directoryUserControls[0];
      List<CDirectionWithProfile> directionWithProfiles = new List<CDirectionWithProfile>();
      foreach (CDirectoryUserControl localDirectoryUserControl in _directoryUserControls) {
        if (localDirectoryUserControl.DirectionWithProfile.ID == id) {
          directoryUserControl = localDirectoryUserControl;
        } else {
          directionWithProfiles.Add(localDirectoryUserControl.DirectionWithProfile);
        }
      }

      if (directoryUserControl.Added) {
        ButtonAdd?.Invoke(directoryUserControl.DirectionWithProfile);
      }
      else {
        Clear();
        foreach(CDirectionWithProfile directionWithProfile in directionWithProfiles) {
          Add(directionWithProfile, false);
        }
      }
    }


  }
}
