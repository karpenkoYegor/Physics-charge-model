using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Diplom
{
    

    public partial class MainWindow : Window
    {
        bool changeMode = true;
        private List<Ellipse> ellipses = new List<Ellipse>();
        Potentional potentional;
        RaschetFI RaschetFI;
        PowerLines powerLines;
        private bool drop = false;
        private byte id;

        public MainWindow()
        {
            InitializeComponent();
            potentional = new Potentional(1080, 684);
            RaschetFI = new RaschetFI(1080, 684);
            powerLines = new PowerLines(1080, 684);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (listBox.SelectedIndex)
            {
                case 0:
                    radius.Visibility = Visibility.Hidden;
                    radiusLabel.Visibility = Visibility.Hidden;
                    height.Visibility = Visibility.Hidden;
                    heightLabel.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    radius.Visibility = Visibility.Visible;
                    radiusLabel.Visibility = Visibility.Visible;
                    height.Visibility = Visibility.Hidden;
                    heightLabel.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    radius.Visibility = Visibility.Visible;
                    radiusLabel.Visibility = Visibility.Visible;
                    height.Visibility = Visibility.Visible;
                    heightLabel.Visibility = Visibility.Visible;
                    break;
            }
        }

        

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaschetFI.coords.Add(new int[] { (int)e.GetPosition(rect).X, (int)e.GetPosition(rect).Y });
            RaschetFI.charge.Add(sliderCharge.Value);
            Reload(true, RaschetFI.ind);
            Create_Ellipce((int)e.GetPosition(rect).X, (int)e.GetPosition(rect).Y);
            listElements.Items.Add($"{RaschetFI.ind.ToString()}.  {listBox.SelectedItem.ToString().Substring(listBox.SelectedItem.ToString().IndexOf(':')+1)}");
            Load();
        }

        public void Reload(bool flag, int indx)
        {
            switch (listBox.SelectedIndex)
            {
                case 0:
                    RaschetFI.Raschet(sliderCharge.Value, RaschetFI.coords[indx][0], RaschetFI.coords[indx][1], flag, indx);
                    break;
                case 1:
                    RaschetFI.Raschet(sliderCharge.Value, RaschetFI.coords[indx][0], RaschetFI.coords[indx][1], radius.Value, flag, indx);
                    break;
                case 2:
                    RaschetFI.Raschet(sliderCharge.Value, RaschetFI.coords[indx][0], RaschetFI.coords[indx][1], radius.Value, height.Value * 100, flag, indx);
                    break;
            }
        }

        public void Load()
        {
            if (changeMode)
            {
                potentional.Element(RaschetFI.fi);
                img.Source = Potentional.image;
            }
            else
            {
                powerLines.Element(RaschetFI.fi);
                img.Source = PowerLines.image;
            }
        }

        private void Create_Ellipce(int x, int y)
        {
            byte c = Convert.ToByte(RaschetFI.ind-1);
            ellipses.Add(new Ellipse());
            ellipses[c].Height = 40;
            ellipses[c].Width = 40;
            ellipses[c].Margin = new Thickness(x - 15, y - 15, 0, 0);
            ellipses[c].Fill = (RaschetFI.charge[c] > 0) ? Brushes.Red : Brushes.Blue;
            ellipses[c].Stroke = Brushes.Black;
            ellipses[c].StrokeThickness = 3;
            ellipses[c].AllowDrop = true;
            ellipses[c].MouseDown += ellipse_MouseDown;
            ellipses[c].MouseMove += ellipse_MouseMove;
            ellipses[c].PreviewMouseUp += ellipse_MouseUp;
            ellipses[c].Uid = c.ToString();
            canvas.Children.Add(ellipses[c]);
            id = c;
        }

        private void listElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte lastId = id;
            if (ellipses.Count > 1)
            {
                ellipses[lastId].Stroke = Brushes.Black;
            }
            id = Convert.ToByte(listElements.SelectedIndex);
            ellipses[id].Stroke = Brushes.Yellow;
        }

        private void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            drop = true;
            byte lastId = id;
            id = Convert.ToByte(((Ellipse)sender).Uid);
            chargeLabel.Content = $"Заряд №{id + 1}";
            chargeLabel.Content = $"Величина q {RaschetFI.charge[id]:F4} Кл";
            sliderCharge.Value = RaschetFI.charge[id];
            listElements.SelectedIndex = id;
            if (ellipses.Count > 1)
            {
                ellipses[lastId].Stroke = Brushes.Black;
            }
            
            ellipses[id].Stroke = Brushes.Yellow;
        }

        private void ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            drop = false;
            Reload(false, id);
            Load();
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (drop)
            {
                short x_l = Convert.ToInt16(e.GetPosition(rect).X - 10);
                short y_l = Convert.ToInt16(e.GetPosition(rect).Y - 10);
                RaschetFI.coords[id][0] = Convert.ToInt16(x_l + 10);
                RaschetFI.coords[id][1] = Convert.ToInt16(y_l + 10);
                ellipses[id].Margin = new Thickness(x_l - 10, y_l - 10, 1080 - x_l, 684 - y_l);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changeMode = false;
            Load();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            changeMode = true;
            Load();
        }

        private void sliderCharge_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            chargeLabel.Content = sliderCharge.Value.ToString() + " нКл";
            if(RaschetFI.charge.Count >= 1)
            {
                RaschetFI.charge[id] = sliderCharge.Value;
                ellipses[id].Fill = (RaschetFI.charge[id] >= 0) ? Brushes.Red : Brushes.Blue;
                Reload(false, id);
                Load();
            }
                
        }

        private void radius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            radiusLabel.Content = radius.Value.ToString() + " мм";
        }

        private void height_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            heightLabel.Content = height.Value.ToString() + " мм";
        }
    }
}
