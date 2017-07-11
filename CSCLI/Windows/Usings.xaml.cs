using CSCLI.Models;
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
using System.Windows.Shapes;

namespace CSCLI.Windows
{
    /// <summary>
    /// Interaction logic for Usings.xaml
    /// </summary>
    public partial class Usings : Window
    {
        public Usings()
        {
            InitializeComponent();
            this.DataContext = Settings.Current;
        }

        private void textNewUsing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textNewUsing.Text.Length == 0)
                {
                    return;
                }
                var strUsing = textNewUsing.Text;
                textNewUsing.Text = "";
                Settings.Current.Usings.Add(strUsing);
                listUsings.Items.Refresh();
                Settings.Current.Save();
                Scripting.NewScript();
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void listUsings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (var selected in listUsings.SelectedItems)
                {
                    Settings.Current.Usings.Remove(selected.ToString());
                }
                Settings.Current.Save();
                listUsings.Items.Refresh();
                Scripting.NewScript();
            }
        }
    }
}
