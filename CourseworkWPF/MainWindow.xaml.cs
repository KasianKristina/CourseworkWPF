﻿using System;
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
using Figure = ClassLibrary.Figure;

namespace CourseworkWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageSource[] detailsImages = new ImageSource[]
       {
            new BitmapImage(new Uri("Assets/squareEmptyNew.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/kingWhite.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/queenWhite.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/kingBlack.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/queenBlack.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/squareWall.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/squarePosition.png", UriKind.Relative))
       };


        private Image[,] images;
        private DynamicField field = new DynamicField();
        private bool WhoPlay = false;
        private int Click = 0;
        private DynamicField.StrategyDelegate str_player1 = null;
        private DynamicField.StrategyDelegate str_player2 = null;
        private DynamicField.PlayerDelegate str_player1_user = null;
        private Figure figure = null;
        private bool gameOver = false;
        public MainWindow()
        {
            InitializeComponent();
            images = SetupGameCanvas(field.GameField);
        }

        private Image[,] SetupGameCanvas(Field grid)
        {
            Image[,] images = new Image[grid.Rows, grid.Columns];
            int cellSize = 50;

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
            images[figure.Offset.Row, figure.Offset.Column].Source = detailsImages[Math.Abs(figure.Id)];
        }

        private void DrawFigure(Position position, int id)
        {
            if (position != null)
                images[position.Row, position.Column].Source = detailsImages[Math.Abs(id)];
        }

        private void DrawAllPositions(Figure figure)
        {
            List<Position> list = figure.GetAllPosition(figure.Offset.Row, figure.Offset.Column, field.player2.king.Offset.Row, field.player2.king.Offset.Column, 1);
            foreach(Position pos in list)
            {
                images[pos.Row, pos.Column].Source = detailsImages[6];
            }
        }

        private void Draw(DynamicField field, Position position, int id)
        {
            DrawField(field.GameField);
            DrawFigure(position, id);
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
            // field.Walls();
            Draw(field);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            switch (comboboxick1.SelectedIndex)
            {
                case 0:
                    str_player1 = field.player1.StrategySimple;
                    break;
                case 1:
                    str_player1 = field.player1.Strategy2;
                    break;
                case 2:
                    str_player1 = field.player1.StrategySecurity;
                    break;
                case 3:
                    str_player1 = field.player1.Strategy4;
                    break;
                case 4:
                    str_player1_user = field.player1.StrategyUser;
                    break;
                default:
                    break;
            }

            switch (comboboxick2.SelectedIndex)
            {
                case 0:
                    str_player2 = field.player2.StrategySimple;
                    break;
                case 1:
                    str_player2 = field.player2.Strategy2;
                    break;
                case 2:
                    str_player2 = field.player2.StrategySecurity;
                    break;
                case 3:
                    str_player2 = field.player2.Strategy4;
                    break;
                default:
                    break;
            }
            //DynamicField.GameStrategy(str_player1, str_player2);
            //if (comboboxick1.SelectedValue != null) // если не равно null
            //{
            //    if (comboboxick1.SelectedValue.ToString() == "1")
            //    {
            //        str_player1 = field.player1.Str;
            //    }
            //}

            if (str_player1 != null && str_player2 != null)
            {
                field.check_delegate(str_player1, str_player2);
                slider1.Maximum = field.player1.history.Keys.Count;
                btnPlay.IsEnabled = false;
            }
            else
            {
                WhoPlay = true;
                // field.check_delegate(str_player1, str_player2);
            }
            field.Walls((int)sliderCountWalls.Value);
            DrawField(field.GameField);
            DrawFigure(field.player1.king.StartOffset, field.player1.king.Id);
            DrawFigure(field.player1.queen.StartOffset, field.player1.queen.Id);
            DrawFigure(field.player2.king.StartOffset, field.player2.king.Id);
            DrawFigure(field.player2.queen.StartOffset, field.player2.queen.Id);
            // Draw(field);


        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;

            if (text1 != null)
            {
                text1.Text = "" + value.ToString("0");
            }

            this.Title = "Value: " + value.ToString("0") + "/" + slider.Maximum;
            if (value >= 0)
                DrawMotion((int)value);

        }

        private void DrawMotion(int motion)
        {
            DrawField(field.GameField);
            if (motion > 0)
            {
                (int, Position) value1;
                field.player1.history.TryGetValue(motion, out value1);
                (int, Position) value_second = Seek_second(motion, value1.Item1, field.player1);

                DrawFigure(value1.Item2, value1.Item1);
                DrawFigure(value_second.Item2, value_second.Item1);

                (int, Position) value2;
                field.player2.history.TryGetValue(motion, out value2);
                DrawFigure(value2.Item2, value2.Item1);
                (int, Position) value_second2 = Seek_second(motion, value2.Item1, field.player2);
                DrawFigure(value_second2.Item2, value_second2.Item1);
            }
            else
            {
                DrawFigure(field.player1.king.StartOffset, field.player1.king.Id);
                DrawFigure(field.player1.queen.StartOffset, field.player1.queen.Id);
                DrawFigure(field.player2.king.StartOffset, field.player2.king.Id);
                DrawFigure(field.player2.queen.StartOffset, field.player2.queen.Id);
            }
        }

        private (int, Position) Seek_second(int motion, int id, Player player)
        {
            int id_seek = 0;
            if (id == -1 || id == -3)
                id_seek = id - 1;
            if (id == -2 || id == -4)
                id_seek = id + 1;
            (int, Position) value;
            for (int i = motion; i > 0; i--)
            {

                player.history.TryGetValue(i, out value);
                if (value.Item1 == id_seek)
                    return value;
            }
            if (id_seek == -1 || id_seek == -1)
                return (id_seek, player.king.StartOffset);
            else return (id_seek, player.queen.StartOffset);
        }

        private void sliderCountWalls_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void comboboxick2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPlay.IsEnabled = true;
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            images = SetupGameCanvas(field.GameField);
            //Draw(field);
        }
        
        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ButtonState == MouseButtonState.Pressed)
            //  currentPoint = e.GetPosition(this);
        }

        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //GameCanvas.Children.
        }

        private void focusOn(object sender, MouseButtonEventArgs e)
        {
            Click++;
            if (WhoPlay && Click == 1 && !gameOver)
            {

                int Coloumn = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).X / 50);
                int Row = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).Y / 50);
                int id = field.GameField[Row, Coloumn];
                switch (id)
                {
                    case -1:
                        figure = field.player1.king;
                        DrawAllPositions(figure);
                        break;
                    case -2:
                        figure = field.player1.queen;
                        DrawAllPositions(figure);
                        break;
                    case -3:
                        figure = field.player2.king;
                        DrawAllPositions(figure);
                        break;
                    case -4:
                        figure = field.player2.queen;
                        DrawAllPositions(figure);
                        break;
                    default:
                        Click = 0;
                        break;
                }
            }
            if (WhoPlay && Click == 2 && !gameOver)
            {
                int Coloumn = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).X / 50);
                int Row = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).Y / 50);
                Position pos = new Position(Row, Coloumn);
                if (images[pos.Row, pos.Column].Source == detailsImages[6])
                {
                    int check = field.check_delegate(str_player1_user, str_player2, pos, figure);
                    if (check == 0)
                        gameOver = true;
                    slider1.Maximum = field.player1.history.Keys.Count;
                    slider1.Value = slider1.Maximum;
                    //DrawField(field.GameField);
                    Click = 0;
                }
                else { MessageBox.Show("Невозможный ход");
                    Click = 1;
                };
            }
        }
    }
}
