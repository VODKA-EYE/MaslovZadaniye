using EasyCaptcha.Wpf;
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
using System.Windows.Threading;

namespace WpfApp1
{
    
    public partial class MainWindow : Window
    {
        DispatcherTimer DT = new DispatcherTimer();
        User authUser = null;
        int role;
        public MainWindow()
        {
            InitializeComponent();
            Cpt.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
            head.Background = new SolidColorBrush(Color.FromRgb(255, 204, 153));
            ButtonLogin.Background = new SolidColorBrush(Color.FromRgb(204, 102, 0));
            GuestButton.Background = new SolidColorBrush(Color.FromRgb(204, 102, 0));
        }

        // Проверка логина, пароля, капчи
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            using (TradeEntities db = new TradeEntities())
            {
                authUser = db.User.Where(b => b.UserLogin == LoginBox.Text && b.UserPassword == PasswordBox.Password).FirstOrDefault();
            }
            if (authUser != null && CaptchaBox.Text == Cpt.CaptchaText)
            {   
                
                ProductsList productsList = new ProductsList();
                productsList.Show();
                Close();
            }
            else
            {
                DT.Interval = TimeSpan.FromSeconds(10);
                DT.Tick += DT_Tick;
                DT.Start();
                ButtonLogin.IsEnabled = false;

            }
        }
        // 10 секунд блокировки кнопки
        private void DT_Tick(object sender, EventArgs e)
        {            
            Cpt.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
            ButtonLogin.IsEnabled = true;
            DT.Stop();
        }

        private void GuestMode(object sender, RoutedEventArgs e)
        {
            ProductsList productsList = new ProductsList();
            productsList.Show();
            Hide();
            productsList.UserName.Text = "Гость";
        }
    }
}
