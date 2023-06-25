using KnowledgeMarketDesktop.Data;
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
    /// Логика взаимодействия для CoursePage.xaml
    /// </summary>
    public partial class CoursePage : Page
    {
        public CoursePage()
        {
            InitializeComponent();
            tbName.Text = Context.CourseNow.Name;
            tbDescription.Text = Context.CourseNow.Description;
            tbAuthor.Text = Context.CourseNow.Author;
            tbLink.Text = Context.CourseNow.Link;
            tbPrice.Text = Context.CourseNow.Price.ToString();
            imgCourse.Source = new BitmapImage(new Uri(Context.CourseNow.PhotoLink));
        }

        public CoursePage(bool isPurchated)
        {
            InitializeComponent();
            tbName.Text = Context.CourseNow.Name;
            tbDescription.Text = Context.CourseNow.Description;
            tbAuthor.Text = Context.CourseNow.Author;
            tbLink.Text = Context.CourseNow.Link;
            tbPrice.Text = Context.CourseNow.Price.ToString();
            imgCourse.Source = new BitmapImage(new Uri(Context.CourseNow.PhotoLink));
            tbLink.Visibility = Visibility.Visible;
            btnBuyCourse.Visibility = Visibility.Collapsed;
        }

        private void btnBuyCourse_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new BuyPage());
        }
    }
}
