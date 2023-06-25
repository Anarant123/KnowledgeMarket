﻿using AdBoards.ApiClient.Extensions;
using KnowledgeMarketDesktop.Data;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AdminCoursePage.xaml
    /// </summary>
    public partial class AdminCoursePage : Page
    {
        AddCourseModel course = new AddCourseModel();

        public AdminCoursePage()
        {
            InitializeComponent();
            tbName.Text = Context.CourseNow.Name;
            tbDescription.Text = Context.CourseNow.Description;
            tbAuthor.Text = Context.CourseNow.Author;
            tbLink.Text = Context.CourseNow.Link;
            tbPrice.Text = Context.CourseNow.Price.ToString();
            imgCourse.Source = new BitmapImage(new Uri(Context.CourseNow.PhotoLink));
        }

        private void btnSetPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Image Files(*.BMP; *.JPG; *.GIF; *.PNG)| *.BMP; *.JPG; *.GIF; *.PNG | All files(*.*) | *.* ";
            if (o.ShowDialog() == true)
            {
                try
                {
                    imgCourse.Source = LoadImage.Load(File.ReadAllBytes(o.FileName));

                    var stream = new FileStream(o.FileName, FileMode.Open);
                    var formFile = new FormFile(stream, 0, stream.Length, "streamFile", System.IO.Path.GetFileName(o.FileName));

                    course.PhotoFile = formFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            int price;
            string ValidateFields()
            {
                var result = string.Empty;
                if (!int.TryParse(tbPrice.Text, out price) || price < 0)
                    result += "Некорректная цена\n";


                if (string.IsNullOrWhiteSpace(tbName.Text))
                    result += "Название объявления является обязательным полем.\n";

                if (string.IsNullOrWhiteSpace(tbLink.Text))
                    result += "Ссылка на источник является обязательным полем.\n";

                if (string.IsNullOrWhiteSpace(tbDescription.Text))
                    result += "Описание является обязательным полем.\n";

                return result;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            course.Id = Context.CourseNow.Id;
            course.Name = tbName.Text;
            course.Description = tbDescription.Text;
            course.Price = price;
            course.Author = tbAuthor.Text;
            course.Link = tbLink.Text;
            course.UserId = Context.UserNow.User.Id;
            course.IsDeleted = false;

            Context.CourseNow = await Context.Api.CourseUpdate(course);
            course.Id = Context.CourseNow.Id;

            if (course.PhotoFile != null)
                Context.CourseNow = await Context.Api.UpdateCoursePhoto(course);

            if (Context.CourseNow == null)
            {
                MessageBox.Show("Что то пошло не так\n");
                return;
            }

            MessageBox.Show("Данные успешно сохраннены");
            this.NavigationService.Navigate(new CoursesPage());
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = await Context.Api.DeleteCourse(Context.CourseNow.Id);

            if (result)
            {
                this.NavigationService.Navigate(new CoursesPage());
                return;
            }
            MessageBox.Show("Что то пошло не так...");
        }
    }
}
