using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using FontAwesome.WPF;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Xaml_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FontAwesomeIcon elozoKartya;
        private long score;
        private DispatcherTimer pendulumClock;
        private TimeSpan playTime;
        private Stopwatch stopwatch;
        private List<long> listReactionTimes;
        private FontAwesomeIcon[] kartyak;
        private Random kocka;
        




        public MainWindow()
        {
            InitializeComponent();

            pendulumClock = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, clockShock, Application.Current.Dispatcher);
            pendulumClock.Stop();
            stopwatch = new Stopwatch();



            kartyak = new FontAwesome.WPF.FontAwesomeIcon[6];


            kartyak[0] = FontAwesome.WPF.FontAwesomeIcon.Forward;
            kartyak[1] = FontAwesome.WPF.FontAwesomeIcon.Gears;
            kartyak[2] = FontAwesome.WPF.FontAwesomeIcon.Gitlab;
            kartyak[3] = FontAwesome.WPF.FontAwesomeIcon.Glide;
            kartyak[4] = FontAwesome.WPF.FontAwesomeIcon.Google;
            kartyak[5] = FontAwesome.WPF.FontAwesomeIcon.Hourglass;

            kocka = new Random();

            StartState();
        }

        private void StartState()
        {
            ButtonIndit.IsEnabled = true;
            ButtonYes.IsEnabled = false;
            ButtonNo.IsEnabled = false;
            score = 0;

            playTime = TimeSpan.FromSeconds(0);

            listReactionTimes = new List<long>();

            UjKartya();
        }
        private void FinalState()
        {
            pendulumClock.Stop();
            ButtonIndit.IsEnabled = true;
            ButtonYes.IsEnabled = false;
            ButtonNo.IsEnabled = false;
        }


        private void clockShock(object sender, EventArgs e)
        {
            playTime = playTime + TimeSpan.FromSeconds(1);
            LabelPlayTime.Content = $"{playTime.Minutes:00}:{playTime.Seconds:00}";
            

        }


        /// <summary>
        /// Kártya választó függvény
        /// </summary>
        public void UjKartya()
        {
          

            var dobas = kocka.Next(0, 5);

            elozoKartya = CardLeft.Icon;

            var animOut = new DoubleAnimation(1,0,TimeSpan.FromMilliseconds(100));
            var animIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));

           

            CardLeft.BeginAnimation(OpacityProperty, animOut);
           

            CardLeft.Icon = kartyak[dobas];

            CardLeft.BeginAnimation(OpacityProperty, animIn);

            stopwatch.Restart();



        }

       

        private void IgenValasz()
        {
            if (elozoKartya == CardLeft.Icon)
            {
                JoValasz();
            }
            else
            {
                RosszValasz();
            }

            UjKartya();
        }

        private void NemValasz()
        {
            if (elozoKartya != CardLeft.Icon)
            {
                JoValasz();
            }
            else
            {
                RosszValasz();
            }
            UjKartya();
        }

        private void RosszValasz()
        {
            Debug.WriteLine("Helytelen volt a válasz!");
            CardRight.Icon = FontAwesomeIcon.Times;
            CardRight.Foreground = Brushes.Red;

            Scoring(false);

            VisszajelzesEltuntetes();

        }
        /// <summary>
        /// Szkór függvény
        /// </summary>
        /// <param name="isGoodAnswer">Jelzi a jó vagy rossz választ</param>
        private void Scoring(bool isGoodAnswer)
        {
            

            stopwatch.Stop();
            listReactionTimes.Add(stopwatch.ElapsedMilliseconds);
           

            LabelReactTime.Content = $"{stopwatch.ElapsedMilliseconds}/{(long)listReactionTimes.Average()}";


            if (isGoodAnswer == true)
            {
                score = score + 100000/listReactionTimes.Last();
            }
            else
            {
                score = score - 100*(listReactionTimes.Last()/1000);
            }


            LabelScore.Content = score;
        }

        private void JoValasz()
        {
            Debug.WriteLine("Helyes volt a válasz!");
            CardRight.Icon = FontAwesomeIcon.Check;
            CardRight.Foreground = Brushes.Green;
            Scoring(true);
            VisszajelzesEltuntetes();
        }

        private void VisszajelzesEltuntetes()
        {
            var anim = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(1000));
            CardRight.BeginAnimation(OpacityProperty, anim);
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Igen gomb nyomása");
            IgenValasz();

        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("Nem gomb nyomása");
            NemValasz();
        }

       

        private void ButtonIndit_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Indít gomb lenyomása");
            StartGame();
        }

        private void StartGame()
        {
            ButtonIndit.IsEnabled = false;
            ButtonYes.IsEnabled = true;
            ButtonNo.IsEnabled = true;
            UjKartya();
            pendulumClock.Start();
            stopwatch.Restart();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if (e.Key==Key.Up)
            {
                StartGame();
            }
            if (e.Key == Key.Right)
            {
                NemValasz();
            }
            if (e.Key == Key.Left)
            {
                IgenValasz();
            }
        }
    }
}
