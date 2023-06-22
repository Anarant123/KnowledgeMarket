using AdBoards.ApiClient.Extensions;
using KnowledgeMarketDesktop.Data;
using KnowledgeMarketWebAPI.ApiClient.Contracts.Requests;
using KnowledgeMarketWebAPI.Data.Models.db;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            DateTime birthday = DateTime.Now;

            string ValidateFields()
            {
                string result = string.Empty;
                if (string.IsNullOrWhiteSpace(tbLogin.Text))
                    result += "Введите логин.\n";

                if (string.IsNullOrWhiteSpace(tbEmail.Text) || !IsValidEmail(tbEmail.Text))
                    result += "Введите корректный email.\n";

                if (string.IsNullOrWhiteSpace(tbPassword1.Password) || string.IsNullOrWhiteSpace(tbPassword2.Password) || tbPassword1.Password.Length < 8)
                    result += "Введите пароль в оба поля.";
                else if (tbPassword1.Password != tbPassword2.Password)
                    result += "Пароли не совпадают.\n";

                return result;
            }

            bool IsValidEmail(string email)
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                Match match = Regex.Match(email, pattern);
                return match.Success;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                MessageBox.Show(ValidateFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserReg person = new UserReg();
            person.Login = tbLogin.Text;
            person.Email = tbEmail.Text;
            person.Password = tbPassword1.Password;

            var result = (await Context.Api.Registr(person)).ToList();

            if (result.Count == 0)
            {
                this.NavigationService.Navigate(new AuthorizationPage());
                return;
            }

            var error = string.Join(Environment.NewLine, result.Select(x => x.Message));

            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
