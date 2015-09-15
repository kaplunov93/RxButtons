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

            //FirstVariant();

            SecondVariant();
            

        }

        public void SecondVariant()
        {

            var click3 = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => button3.Click += h, h => button3.Click -= h).Take(1);
            var click1 = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => button1.Click += h, h => button1.Click -= h).SkipUntil(click3).Take(1);
            var click2 = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => button2.Click += h, h => button2.Click -= h).SkipUntil(click3).TakeUntil(click1);
            var click4 = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => button4.Click += h, h => button4.Click -= h).SkipUntil(click3).TakeUntil(click1);
            var clicks = click1.Merge(click2).Merge(click3).Merge(click4);
            var res = from c in clicks  select c.Sender.ToString();
            res.Subscribe(h => textBox.Text += h + "\n", () => textBox.Text += "Complited\n");
        }

        public void FirstVariant()
        {
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
            click.Subscribe(h => textBox.Text += h + "\n", () => textBox.Text += button1.ToString() + "\nComplited");
        }
    }
}
