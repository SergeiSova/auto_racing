using System.Collections.Generic;
using System.Windows;

namespace Автогонки
{
    public partial class ResultsWindow : Window
    {
        public ResultsWindow(IList<GameResult> results)
        {
            InitializeComponent();
            ResultsGrid.ItemsSource = results;
        }
    }
}
