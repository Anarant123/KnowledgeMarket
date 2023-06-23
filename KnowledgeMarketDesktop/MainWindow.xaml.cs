using KnowledgeMarketDesktop.Data;
using KnowledgeMarketDesktop.Views;
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

namespace KnowledgeMarketDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new CoursesPage());
            if (Context.UserNow == null)
            {
                tbBtnExit.Text = "Войти";
            }
        }

        private void btnShowCourses_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.Navigate(new CoursesPage());
        }

        private void btnShowPurchasedCourses_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow == null)
            {
                mainFrame.NavigationService.Navigate(new AuthorizationPage());
                return;
            }
            mainFrame.NavigationService.Navigate(new MyCoursesPage());
        }

        private void btnAddCourse_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow == null)
            {
                mainFrame.NavigationService.Navigate(new AuthorizationPage());
                return;
            }
            mainFrame.NavigationService.Navigate(new AddCoursePage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Context.UserNow == null)
            {
                mainFrame.NavigationService.Navigate(new AuthorizationPage());
                return;
            }

            Context.UserNow = null;
            mainFrame.NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
