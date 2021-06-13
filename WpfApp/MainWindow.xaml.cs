using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1NASTYA
{
    public partial class MainWindow : Window
    {
        // константы 
        private const double DeltaTime = 0.001;
        private const double ResistanceForce = 6;
        private const int Mass = 1;
        private const double G = 8.91;

        // параметры для таймера 
        private DispatcherTimer timer = new DispatcherTimer();

        // начальные данные
        private double _speed, _angle;
        // координаты
        private double _x, _y;
        // скорость
        private double _speedX, _speedY;
       
        //Канвас 
        private double aW, aH, coefficientWidth, coefficientHeight;    

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20); // частота обновления таймера 
        }

        private void Output() // ввод данных из окна 
        {
            _speed = Convert.ToDouble(speed.Text);
            _angle = Convert.ToDouble(angle.Text);
            _angle = _angle * Math.PI / 180;
        }

        private void Canvass() // настройка размеров канваса
        {
            aW = canvas.ActualWidth / 2; // середина канваса 
            aH = canvas.ActualHeight / 2; // середина канаваса 

            coefficientWidth = aW / 2; // коэффицент маштобирования 
            coefficientHeight = aW / 2; // коэффицент маштобирования 
        }

        private void Button_Click(object sender, RoutedEventArgs e) // нажатие на кнопку
        {
            Output(); // вводим данные
            Canvass(); // считаем размеры канваса 
            _x = _y = 0; // чистим координаты 
            _speedX = _speed * Math.Cos(_angle); // начальная скорость под уголм 
            _speedY = _speed * Math.Sin(_angle); // начальная скорость под углом
            poliline.Points.Clear(); // чистим полилине
            ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0)); // меняем цвет у эллепса 
            timer.Start(); // запускаем таймер
        }

        private void OnTimer(object sender, EventArgs e) // таймер
        {
            // расчёт координат по предыдущим
            _x = _x + DeltaTime * _speedX;
            _y = _y + DeltaTime * _speedY;
            // расчёт скорости по предыдущим 
            _speedX = _speedX - DeltaTime * (ResistanceForce * _speedX / Mass);
            _speedY = _speedY - DeltaTime * (G + ResistanceForce * _speedY / Mass);
            // заполнение точек
            poliline.Points.Add(new Point(coefficientWidth * _x, aH + coefficientHeight * -_y));
            Canvas.SetLeft(ellipse, poliline.Points.Last().X - ellipse.Width / 2.0); // меняем полижение элипса по последней точки в полилайн
            Canvas.SetTop(ellipse, poliline.Points.Last().Y - ellipse.Height / 2.0); // анологично 

            if (_y <= 0) // останавливаем таймер при достижении игрика отрицательных значений 
                timer.Stop();
        }
    }
}

