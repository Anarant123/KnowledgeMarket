using AdBoards.ApiClient.Extensions;
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
    /// Логика взаимодействия для BuyPage.xaml
    /// </summary>
    public partial class BuyPage : Page
    {
        public BuyPage()
        {
            InitializeComponent();
        }

        private async void btnSentCardDetails_Click(object sender, RoutedEventArgs e)
        {
            await Context.Api.BuyCourse(Context.CourseNow.Id);
            this.NavigationService.Navigate(new CoursePage(true));
        }
    }
}
