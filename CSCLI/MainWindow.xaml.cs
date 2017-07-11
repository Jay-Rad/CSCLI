using CSCLI.Models;
using CSCLI.Windows;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
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

namespace CSCLI
{
    public partial class MainWindow : Window
    {
        private List<string> CommandHistory { get; set; } = new List<string>();
        private int HistoryPosition { get; set; } = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Settings.Current;
            Settings.Current.Load();
            Scripting.NewScript();
            WPF_Auto_Update.Updater.RemoteFileURI = "https://translucency.azurewebsites.net/Downloads/CSCLI.exe";
            WPF_Auto_Update.Updater.ServiceURI = "https://translucency.azurewebsites.net/Services/VersionCheck.cshtml?Path=/Downloads/CSCLI.exe";
            WPF_Auto_Update.Updater.CheckCommandLineArgs();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WPF_Auto_Update.Updater.CheckForUpdates(true);
        }
        private async void textInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                if (String.IsNullOrWhiteSpace(textInput.Text))
                {
                    return;
                }
                e.Handled = true;
                var strInput = textInput.Text;
                textInput.Text = "";
                CommandHistory.Add(strInput);
                HistoryPosition = CommandHistory.Count;
                if (strInput == "/?")
                {
                    textOutput.Text += $"C# CLI Help{Environment.NewLine}---------------------------{Environment.NewLine + Environment.NewLine}Add using statements and references by typing the namespace (and only the namespace) into the respective window and hitting Enter.{Environment.NewLine + Environment.NewLine}Remove usings and references by selecting one or more and hitting Delete.{Environment.NewLine + Environment.NewLine}A new script environment is loaded after modifying usings or references.{Environment.NewLine + Environment.NewLine}Usings and references are retained and stored in %appdata%\\CSCLI\\Settings.json.{Environment.NewLine + Environment.NewLine}Omit the ending \";\" to have a result returned to the output stream and written to the window.{Environment.NewLine + Environment.NewLine}The window has a splitter that can be dragged to adjust the height of the top and bottom sections.{Environment.NewLine + Environment.NewLine}Author: Jared Goodwin (https://translucency.azurewebsites.net){Environment.NewLine + Environment.NewLine}Third Party Library: Fody/Costura (https://github.com/Fody/Costura)";
                }
                else
                {
                    textOutput.Text += await Scripting.RunScript(strInput);
                }
                textOutput.ScrollToEnd();
            }
            else if (e.Key == Key.Up)
            {
                if (HistoryPosition > 0)
                {
                    HistoryPosition--;
                    textInput.Text = CommandHistory[HistoryPosition];
                    textInput.Select(textInput.Text.Length, 0);
                }
            }
            else if (e.Key == Key.Down)
            {
                if (HistoryPosition < CommandHistory.Count - 1)
                {
                    HistoryPosition++;
                    textInput.Text = CommandHistory[HistoryPosition];
                    textInput.Select(textInput.Text.Length, 0);
                }
            }
        }

        private void buttonUsings_Click(object sender, RoutedEventArgs e)
        {
            var win = new Usings();
            win.Owner = this;
            win.ShowDialog();
        }

        private void buttonReferences_Click(object sender, RoutedEventArgs e)
        {
            var win = new References();
            win.Owner = this;
            win.ShowDialog();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textOutput.Text = "";
        }
    }
}
