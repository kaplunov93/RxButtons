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
using System.Reactive;
using System.Reactive.Linq;

namespace RxButtons
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var click = from cl in Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                h =>
                {
                    button1.Click += h;
                    button2.Click += h;
                    button3.Click += h;
                    button4.Click += h;
                }, 
                h =>
                {
                    button1.Click -= h;
                    button2.Click -= h;
                    button3.Click -= h;
                    button4.Click -= h;
                })
                     .SkipWhile(i => i.EventArgs.Source.ToString() != button3.ToString())
                     .TakeWhile(i => i.EventArgs.Source.ToString() != button1.ToString())
                        select cl.EventArgs.Source.ToString();
            /*var ev = from cl in click
                     .SkipWhile(i=>i!=button3.ToString())
                     .TakeWhile(i=> i!=button1.ToString())
                     select cl;*/
            click.Subscribe(h => textBox.Text += h + "\n",()=> textBox.Text +=button1.ToString()+ "\nComplited");
        }
    }
}
