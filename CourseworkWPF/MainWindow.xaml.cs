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
using ClassLibrary;

namespace CourseworkWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageSource[] detailsImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/EmptySquare.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/newWhiteKing.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/whiteKing.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/blackKing.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/black_queen.jpg", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Orange.png", UriKind.Relative))
        };


        private Image[,] images;
        private DynamicField field = new DynamicField();
        public MainWindow()
        {
            InitializeComponent();
            images = SetupGameCanvas(field.GameField);
        }

        private Image[,] SetupGameCanvas(Field grid)
        {
            Image[,] images = new Image[grid.Rows, grid.Columns];
            int cellSize = 40;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, r * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    images[r, c] = imageControl;
                }
            }
            return images;
        }

        private void DrawField(Field field)
        {
            for (int r = 0; r < field.Rows; r++)
            {
                for (int c = 0; c < field.Columns; c++)
                {
                    int id = field[r, c];
                    if (id != -1 && id != -2 && id != -3 && id != -4)
                        images[r, c].Source = detailsImages[Math.Abs(id)];
                    else images[r, c].Source = detailsImages[0];
                }
            }
        }

        private void DrawFigure(ClassLibrary.Figure figure)
        {
            images[figure.offset.Row, figure.offset.Column].Source = detailsImages[Math.Abs(figure.Id)];
        }

        private void DrawFigure(Position position, int id)
        {
            if (position != null)
                images[position.Row, position.Column].Source = detailsImages[Math.Abs(id)];
        }

        private void Draw(DynamicField field, Position position, int id)
        {
            DrawField(field.GameField);
            DrawFigure(position, id);
            //DrawFigure(field.player1.queen);
            //DrawFigure(field.player2.king);
            //DrawFigure(field.player2.queen);
        }

        private void Draw(DynamicField field)
        {
            DrawField(field.GameField);
            DrawFigure(field.player1.king);
            DrawFigure(field.player1.queen);
            DrawFigure(field.player2.king);
            DrawFigure(field.player2.queen);
        }

        private void GameCanvas_Loaded_1(object sender, RoutedEventArgs e)
        {
            // field = new DynamicField();
            field.Walls();
            Draw(field);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            field.Walls();
            field.check_delegate();
            // Draw(field);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            field.check_delegate();
            
            Draw(field);
            
            slider1.Maximum = field.player1.history.Keys.Count;
            // this.Title = "Value: " + value.ToString("0.0") + "/" + slider1.Maximum;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;
            
            if (text1 != null)
            {
                text1.Text = "" + value.ToString("0");
            }
            
            //slider.Maximum = field.player1.history.Keys.Count;
            this.Title = "Value: " + value.ToString("0") + "/" + slider.Maximum;
            if (value > 1)
                DrawMotion((int)value);
            
        }

        private void DrawMotion(int motion)
        {
            (int, Position) value1;
            field.player1.history.TryGetValue(motion, out value1);
            
            //DrawFigure(value1.Item2, value1.Item1);
            Draw(field, value1.Item2, value1.Item1);
            //Draw(field);
            (int, Position) value2;
            field.player2.history.TryGetValue(motion, out value2);
            DrawFigure(value2.Item2, value2.Item1);
            //Draw(field);
        }
    }
}
