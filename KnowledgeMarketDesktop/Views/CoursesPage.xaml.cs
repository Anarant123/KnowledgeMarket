using AdBoards.ApiClient.Extensions;
using ApiClient.Extensions;
using KnowledgeMarketDesktop.Data;
using KnowledgeMarketWebAPI.Data.Models.db;
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
    /// Логика взаимодействия для CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : Page
    {
        public CoursesPage()
        {
            InitializeComponent();
            getAds();
        }

        private void lvCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.CourseNow = new Course();
            Context.CourseNow = (lvCourses.SelectedItem as Course);

            if (Context.UserNow == null || Context.UserNow.User.RoleId == (int?)RoleType.Normal)
            {
                this.NavigationService.Navigate(new CoursePage());
                return;
            }

            this.NavigationService.Navigate(new AdminCoursePage());
        }
        private async void getAds()
        {
            Context.CourseList = new CourseListViewModel();
            Context.CourseList.Courses = await Context.Api.GetCourses();
            lvCourses.ItemsSource = Context.CourseList.Courses.ToList();
        }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(tbSearch.Text))
                {
                    Context.CourseList.Courses = await Context.Api.GetCourses();
                    lvCourses.ItemsSource = Context.CourseList.Courses.ToList();
                    return;
                }

                Context.CourseList.Courses = (await Context.Api.GetCourses()).Where(x => x.Name.Contains(tbSearch.Text)).ToList(); ;
                lvCourses.ItemsSource = Context.CourseList.Courses.ToList();
            }
        }
    }
}
