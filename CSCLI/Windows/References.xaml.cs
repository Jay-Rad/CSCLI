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
    /// Interaction logic for References.xaml
    /// </summary>
    public partial class References : Window
    {
        public References()
        {
            InitializeComponent();
            this.DataContext = Settings.Current;
        }
        private void textNewReference_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textNewReference.Text.Length == 0)
                {
                    return;
                }
                var strReference = textNewReference.Text;
                textNewReference.Text = "";
                Settings.Current.References.Add(strReference);
                listReferences.Items.Refresh();
                Settings.Current.Save();
                Scripting.NewScript();
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void listReferences_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (var selected in listReferences.SelectedItems)
                {
                    Settings.Current.References.Remove(selected.ToString());
                }
                Settings.Current.Save();
                listReferences.Items.Refresh();
                Scripting.NewScript();
            }
        }
    }
}
