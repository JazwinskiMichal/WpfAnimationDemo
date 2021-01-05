using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ConveyorDemo
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Declarations
        private bool _stopClicked;
        #endregion

        #region General
        public MainWindow()
        {
            InitializeComponent();

            _play.Visibility = Visibility.Visible;
            _stop.Visibility = Visibility.Hidden;

            InitializeRotateAnimation(ref _cog1, ref _rotateTransform_cog1);
            InitializeRotateAnimation(ref _cog2, ref _rotateTransform_cog2);
            InitializeRotateAnimation(ref _cog3, ref _rotateTransform_cog3);
            InitializeRotateAnimation(ref _cog4, ref _rotateTransform_cog4);
            InitializeRotateAnimation(ref _cog5, ref _rotateTransform_cog5);

            InitializeCaseAnimation(ref _box, ref _positionTransform_case);
        }
        #endregion

        #region User Input
        private void _play_Click(object sender, RoutedEventArgs e)
        {
            _play.Visibility = Visibility.Hidden;
            _stop.Visibility = Visibility.Visible;

            SimpleRotateAnimation();
        }

        private void _stop_Click(object sender, RoutedEventArgs e)
        {
            _play.Visibility = Visibility.Visible;
            _stop.Visibility = Visibility.Hidden;

            ((Storyboard)Resources["_cog1_Storyboard"]).Pause();
            ((Storyboard)Resources["_cog2_Storyboard"]).Pause();
            ((Storyboard)Resources["_cog3_Storyboard"]).Pause();
            ((Storyboard)Resources["_cog4_Storyboard"]).Pause();
            ((Storyboard)Resources["_cog5_Storyboard"]).Pause();

            ((Storyboard)Resources["_box_Storyboard1"]).Pause();
            ((Storyboard)Resources["_box_Storyboard2"]).Pause();
            ((Storyboard)Resources["_box_Storyboard3"]).Pause();
            ((Storyboard)Resources["_box_Storyboard4"]).Pause();
            _stopClicked = true;
        }

        private void _exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Storyboard Animations
        private void InitializeRotateAnimation(ref Button givenButton, ref RotateTransform givenTransform)
        {
            Storyboard storyboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(2))
            };
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                To = givenTransform.Angle + 360,
                Duration = storyboard.Duration
            };

            Storyboard.SetTarget(rotateAnimation, givenButton);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            storyboard.Children.Add(rotateAnimation);

            Resources.Add(givenButton.Name + "_Storyboard", storyboard);

            ((Storyboard)Resources[givenButton.Name + "_Storyboard"]).Completed += new EventHandler(myanim_Completed);
        }

        private void InitializeCaseAnimation(ref Button caseButton,ref TranslateTransform givenTransform)
        {
            //Move Animation from middle to the edge
            Storyboard storyboard1 = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(4))
            };
            DoubleAnimation moveAnimation = new DoubleAnimation()
            {
                To = givenTransform.X + 280,
                From = givenTransform.X,
                Duration = storyboard1.Duration
            };

            Storyboard.SetTarget(moveAnimation, caseButton);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard1.Children.Add(moveAnimation);

            Resources.Add(caseButton.Name + "_Storyboard1", storyboard1);

            ((Storyboard)Resources[caseButton.Name + "_Storyboard1"]).Completed += new EventHandler(caseAnim_Completed1);


            //Opacity animation on the Right Edge
            Storyboard storyboard2 = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };
            DoubleAnimation caseDoneAnmiation = new DoubleAnimation()
            {
                To = 0,
                Duration = storyboard2.Duration
            };
            Storyboard.SetTarget(caseDoneAnmiation, caseButton);
            Storyboard.SetTargetProperty(caseDoneAnmiation, new PropertyPath("Opacity"));

            storyboard2.Children.Add(caseDoneAnmiation);

            Resources.Add(caseButton.Name + "_Storyboard2", storyboard2);

            ((Storyboard)Resources[caseButton.Name + "_Storyboard2"]).Completed += new EventHandler(caseAnim_Completed2);

            //Appear Animation - new Case
            Storyboard storyboard3 = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500))
            };
            DoubleAnimation caseNewAnmiation = new DoubleAnimation()
            {
                To = 1,
                Duration = storyboard3.Duration
            };
            Storyboard.SetTarget(caseNewAnmiation, caseButton);
            Storyboard.SetTargetProperty(caseNewAnmiation, new PropertyPath("Opacity"));

            storyboard3.Children.Add(caseNewAnmiation);

            Resources.Add(caseButton.Name + "_Storyboard3", storyboard3);

            ((Storyboard)Resources[caseButton.Name + "_Storyboard3"]).Completed += new EventHandler(caseEnd_Completed3);

            //Transition to Left Edge of the Belt
            Storyboard storyboard4 = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(1))
            };
            DoubleAnimation caseStartAnimation = new DoubleAnimation()
            {
                From = givenTransform.X,
                To = givenTransform.X-280,
                Duration = storyboard4.Duration
            };
            Storyboard.SetTarget(caseStartAnimation, caseButton);
            Storyboard.SetTargetProperty(caseStartAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard4.Children.Add(caseStartAnimation);

            Resources.Add(caseButton.Name + "_Storyboard4", storyboard4);

            ((Storyboard)Resources[caseButton.Name + "_Storyboard4"]).Completed += new EventHandler(caseStart_Completed4);
        }
        #endregion

        #region Helper Functions
        private void SimpleRotateAnimation()
        {
            if (_stopClicked)
            {
                ((Storyboard)Resources["_cog1_Storyboard"]).Resume();
                ((Storyboard)Resources["_cog2_Storyboard"]).Resume();
                ((Storyboard)Resources["_cog3_Storyboard"]).Resume();
                ((Storyboard)Resources["_cog4_Storyboard"]).Resume();
                ((Storyboard)Resources["_cog5_Storyboard"]).Resume();

                ((Storyboard)Resources["_box_Storyboard1"]).Resume();
                ((Storyboard)Resources["_box_Storyboard2"]).Resume();
                ((Storyboard)Resources["_box_Storyboard3"]).Resume();
                ((Storyboard)Resources["_box_Storyboard4"]).Resume();
                _stopClicked = false;
            }
            else
            {
                ((Storyboard)Resources["_cog1_Storyboard"]).Begin();
                ((Storyboard)Resources["_cog2_Storyboard"]).Begin();
                ((Storyboard)Resources["_cog3_Storyboard"]).Begin();
                ((Storyboard)Resources["_cog4_Storyboard"]).Begin();
                ((Storyboard)Resources["_cog5_Storyboard"]).Begin();

                ((Storyboard)Resources["_box_Storyboard1"]).Begin();
            }
        }

        private void myanim_Completed(object sender, EventArgs e)
        {
            ((Storyboard)Resources["_cog1_Storyboard"]).Seek(TimeSpan.FromSeconds(0));
            ((Storyboard)Resources["_cog2_Storyboard"]).Seek(TimeSpan.FromSeconds(0));
            ((Storyboard)Resources["_cog3_Storyboard"]).Seek(TimeSpan.FromSeconds(0));
            ((Storyboard)Resources["_cog4_Storyboard"]).Seek(TimeSpan.FromSeconds(0));
            ((Storyboard)Resources["_cog5_Storyboard"]).Seek(TimeSpan.FromSeconds(0));
            ((Storyboard)Resources["_cog1_Storyboard"]).Resume();
            ((Storyboard)Resources["_cog2_Storyboard"]).Resume();
            ((Storyboard)Resources["_cog3_Storyboard"]).Resume();
            ((Storyboard)Resources["_cog4_Storyboard"]).Resume();
            ((Storyboard)Resources["_cog5_Storyboard"]).Resume();
        }

        private void caseAnim_Completed1(object sender, EventArgs e)
        {
            ((Storyboard)Resources["_box_Storyboard2"]).Begin();
        }

        private void caseAnim_Completed2(object sender, EventArgs e)
        {
            ((Storyboard)Resources["_box_Storyboard4"]).Begin();
            ((Storyboard)Resources["_box_Storyboard3"]).Begin();

            ((Storyboard)Resources["_box_Storyboard1"]).Begin();
        }

        private void caseEnd_Completed3(object sender, EventArgs e)
        {
           
        }

        private void caseStart_Completed4(object sender, EventArgs e)
        {

        }

        private void headerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Left += e.HorizontalChange;
            Top += e.VerticalChange;
        }
        #endregion
    }
}
