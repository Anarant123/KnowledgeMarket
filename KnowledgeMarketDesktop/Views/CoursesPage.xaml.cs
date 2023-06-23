using AdBoards.ApiClient.Extensions;
using ApiClient.Extensions;
using KnowledgeMarketDesktop.Data;
using KnowledgeMarketWebAPI.Data.Models.db;
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
    /// Логика взаимодействия для CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : Page
    {
        public CoursesPage()
        {
            InitializeComponent();
            //getAds();

        }

        private async void lvCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.CourseNow = new Course();
            Context.CourseNow = await Context.Api.GetCourse((lvCourses.SelectedItem as Course).Id);

            this.NavigationService.Navigate(new CoursePage());
        }
        private async void getAds()
        {
            Context.CourseList = new CourseListViewModel();
            Context.CourseList.Courses = await Context.Api.GetCourses();
            lvCourses.ItemsSource = Context.CourseList.Courses.ToList();
        }
    }
}
