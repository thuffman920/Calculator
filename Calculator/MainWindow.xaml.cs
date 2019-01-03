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

namespace Calculator
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CalculatorUI ui;

        public MainWindow()
        {
            InitializeComponent();
            nroot.Content = "\u036F\u221A";
            arcsin.Content = "sin\u02C9\u00B9";
            arccos.Content = "cos\u02C9\u00B9";
            arctan.Content = "tan\u02C9\u00B9";
            exp.Content = "e \u036F";
            tenx.Content = "10 \u036F";
            pi.Content = "\u03C0";
            Solution1.Items.Add("");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = Solution1.Items[Solution1.Items.Count - 1].ToString();
            string part = (string)((sender as Button).Content);
            if (sender == buttonFor0 || sender == addition || sender == minus || sender == multiplication || sender == division || sender == caret) {
                if (!result.Equals("")) {
                    int piece = (int)(result[result.Length - 1]);
                    if (piece >= 48 && piece < 58)
                        result += part;
                }
            } else 
                result += part;
            Solution1.Items.RemoveAt(Solution1.Items.Count - 1);
            Solution1.Items.Add(result);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender == equals && Solution1.Items.Count % 2 != 0) {
                //ui = new CalculatorUI(Solution1.Items[Solution1.Items.Count - 1].ToString());
                ListBoxItem item = new ListBoxItem();
                item.HorizontalContentAlignment = HorizontalAlignment.Right;
                item.Foreground = Brushes.Red;
                item.Content = "25";
                Solution1.Items.Add(item);
                Solution1.Items.Add("");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
