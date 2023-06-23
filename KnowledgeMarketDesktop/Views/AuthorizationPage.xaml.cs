using ApiClient.Extensions;
using KnowledgeMarketDesktop.Data;
using KnowledgeMarketWebAPI.Domain.Enums;
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

namespace KnowledgeMarketDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegistrationPage());
        }

        private async void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            var result = string.Empty;
            string ValidateFields()
            {
                if (string.IsNullOrWhiteSpace(tbLogin.Text))
                    result += "Введите логин!\n";

                if (string.IsNullOrWhiteSpace(tbPassword.Password))
                    result += "Введите пароль!\n";

                return result;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Context.UserNow = await Context.Api.Authorize(tbLogin.Text, tbPassword.Password);
            if (Context.UserNow == null)
            {
                MessageBox.Show("Неудачная авторизация!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Context.Api.Jwt = Context.UserNow.Token;

            if (Context.UserNow.User.RoleId == (int?)RoleType.Admin)
            {
                Window window = Application.Current.MainWindow;
                if (window != null)
                {
                    Button btnAddCourse = FindVisualChild<Button>(window, "btnAddCourse");
                    if (btnAddCourse != null)
                        btnAddCourse.Visibility = Visibility.Visible;

                    Button btnShowPurchasedCourses = FindVisualChild<Button>(window, "btnShowPurchasedCourses");
                    if (btnShowPurchasedCourses != null)
                        btnShowPurchasedCourses.Visibility = Visibility.Collapsed;

                    TextBlock tbBtnExit = FindVisualChild<TextBlock>(window, "tbBtnExit");
                    if (tbBtnExit != null)
                        tbBtnExit.Text = "Выйти";
                }

            }
            else
            {
                Window window = Application.Current.MainWindow;
                if (window != null)
                {
                    Button btnAddCourse = FindVisualChild<Button>(window, "btnAddCourse");
                    if (btnAddCourse != null)
                        btnAddCourse.Visibility = Visibility.Collapsed;

                    Button btnShowPurchasedCourses = FindVisualChild<Button>(window, "btnShowPurchasedCourses");
                    if (btnShowPurchasedCourses != null)
                        btnShowPurchasedCourses.Visibility = Visibility.Visible;

                    TextBlock tbBtnExit = FindVisualChild<TextBlock>(window, "tbBtnExit");
                    if (tbBtnExit != null)
                        tbBtnExit.Text = "Выйти";
                }
            }

            this.NavigationService.Navigate(new CoursesPage());
            
            T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T typedChild && (child as FrameworkElement)?.Name == childName)
                        return typedChild;
                    else
                    {
                        var result = FindVisualChild<T>(child, childName);
                        if (result != null)
                            return result;
                    }
                }
                return null;
            }
        }
    }
}
