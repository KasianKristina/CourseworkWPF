using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static ClassLibrary.DynamicField;
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
        private int WhoPlay = 0;
        private int Click = 0;
        private DynamicField.StrategyDelegate str_player1 = null;
        private DynamicField.StrategyDelegate str_player2 = null;
        private DynamicField.PlayerDelegate str_player1_user = null;
        private DynamicField.PlayerDelegate str_player2_user = null;
        private Figure figure = null;
        private Player player;

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

        private void DrawFigure(Figure figure)
        {
            if (field.IsGameOver())
            {
                labelWinner.Content = "Победитель: " + field.win;
                GameCanvas.IsEnabled = false;
            }
            images[figure.Offset.Row, figure.Offset.Column].Source = detailsImages[Math.Abs(figure.Id)];
        }

        private void DrawFigure(Position position, int id)
        {
            if (field.IsGameOver())
            {
                labelWinner.Content = "Победитель: " + field.win;
                GameCanvas.IsEnabled = false;
            }
            if (position != null)
                images[position.Row, position.Column].Source = detailsImages[Math.Abs(id)];
        }

        private void DrawAllPositions(Figure figure, Player player_s, Player enemy)
        {
            List<Position> list = figure.GetAllPosition(field.player2.history.Count + 1, player_s.motionColor, enemy.queen, enemy.king, player_s.queen);
            foreach (Position pos in list)
            {
                images[pos.Row, pos.Column].Source = detailsImages[6];
            }
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
            Draw(field);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            btnPlay.IsEnabled = false;
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
                case 4:
                    str_player2_user = field.player2.StrategyUser;
                    break;
                default:
                    break;
            }
            field.Walls((int)sliderCountWalls.Value);
            if (str_player1 != null && str_player2 != null)
            {
                field.check_delegate(str_player1, str_player2);
                slider1.Maximum = field.player1.history.Keys.Count;
                btnPlay.IsEnabled = false;

            }
            DrawField(field.GameField);
            DrawFigure(field.player1.king.StartOffset, field.player1.king.Id);
            DrawFigure(field.player1.queen.StartOffset, field.player1.queen.Id);
            DrawFigure(field.player2.king.StartOffset, field.player2.king.Id);
            DrawFigure(field.player2.queen.StartOffset, field.player2.queen.Id);
            if (str_player1_user != null)
            {
                WhoPlay = 1;
            }
            else if (str_player2_user != null)
            {

                WhoPlay = 2;
                int check = field.check_delegate(str_player1);
                check_check(check);
                DrawField(field.GameField);
                DrawFigure(field.player1.king);
                DrawFigure(field.player1.queen);
                DrawFigure(field.player2.king.StartOffset, field.player2.king.Id);
                DrawFigure(field.player2.queen.StartOffset, field.player2.queen.Id);
            }


            if (field.IsGameOver())
            {
                labelWinner.Content = "Победитель: " + field.win;
                GameCanvas.IsEnabled = false;
            }
            else
            {
                GameCanvas.IsEnabled = true;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            double value = slider.Value;

            if (text1 != null)
            {
                text1.Text = "" + value.ToString("0");
            }
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

                (int, Position) value2 = Seek_second(motion, -3, field.player2);

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
            if (id_seek == -1 || id_seek == -3)
                return (id_seek, player.king.StartOffset);
            else return (id_seek, player.queen.StartOffset);
        }

        private void comboboxick2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnPlay.IsEnabled = true;
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            field = new DynamicField();
            Draw(field);
            btnPlay.IsEnabled = true;
            GameCanvas.IsEnabled = false;
            slider1.Maximum = field.player1.history.Keys.Count;
            Click = 0;
            str_player1 = null;
            str_player2 = null;
            str_player1_user = null;
            str_player2_user = null;
            figure = null;
            labelWinner.Content = "Победитель: ";
            count.Content = "Ферзь на горизонтали: 0";
            historyLabel.Content = "";
        }

        private void opot()
        {
            int Coloumn = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).X / 50);
            int Row = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).Y / 50);
            int id = field.GameField[Row, Coloumn];

            if (WhoPlay == 1)
            {
                switch (id)
                {
                    case -1:
                        figure = field.player1.king;
                        Draw(field);
                        DrawAllPositions(figure, field.player1, field.player2);
                        break;
                    case -2:
                        figure = field.player1.queen;
                        Draw(field);
                        DrawAllPositions(figure, field.player1, field.player2);
                        break;
                    default:
                        Draw(field);
                        Click = 0;
                        break;
                }
            }
            else
                switch (id)
                {
                    case -3:
                        figure = field.player2.king;
                        Draw(field);
                        DrawAllPositions(figure, field.player2, field.player1);
                        break;
                    case -4:
                        figure = field.player2.queen;
                        Draw(field);
                        DrawAllPositions(figure, field.player2, field.player1);
                        break;
                    default:
                        Draw(field);
                        Click = 0;
                        break;
                }
        }
        private void check_check(int check)
        {
            if (check == 0)
            {
                labelWinner.Content = "Победитель: " + field.win;
                GameCanvas.IsEnabled = false;
            }
        }


        private void focusOn(object sender, MouseButtonEventArgs e)
        {
            int playerMotionColor = 0;
            string hod = "";
            Click++;
            if (WhoPlay != 0 && Click == 1)
            {
                opot();
            }
            if (WhoPlay != 0 && Click == 2)
            {
                int Coloumn = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).X / 50);
                int Row = (int)Math.Truncate(Mouse.GetPosition(GameCanvas).Y / 50);
                Position pos = new Position(Row, Coloumn);
                if (images[pos.Row, pos.Column].Source == detailsImages[6])
                {
                    if (str_player1_user != null && str_player2 != null)
                    {
                        int check = field.check_delegate(str_player1_user, str_player2, pos, figure);
                        check_check(check);
                        playerMotionColor = field.player1.motionColor;
                        hod = "Белый ферзь: ";
                        field.player1.CheckPat(playerMotionColor);
                    }
                    if (str_player2_user != null && str_player1 != null)
                    {

                        int check = field.check_delegate(str_player2_user, pos, figure, true);
                        check_check(check);
                        check = field.check_delegate(str_player1);
                        check_check(check);
                        Draw(field);
                        playerMotionColor = field.player2.motionColor;
                        hod = "Черный ферзь: ";  
                    }
                    if ((str_player1_user != null && str_player2_user != null))
                    {
                        bool flaghoda = true;
                        PlayerDelegate player = str_player2_user;
                        if (WhoPlay == 1)
                        {
                            flaghoda = false;
                            WhoPlay = 2;
                            player = str_player1_user;
                            playerMotionColor = field.player2.motionColor;
                            hod = "Черный ферзь: ";
                        }
                        else
                        {
                            WhoPlay = 1;
                            playerMotionColor = field.player1.motionColor;
                            hod = "Белый ферзь: ";
                        }
                        int check = field.check_delegate(player, pos, figure, flaghoda);
                        check_check(check);
                        Draw(field);
                    }
                    slider1.Maximum = field.player1.history.Keys.Count;
                    slider1.Value = slider1.Maximum;
                    Click = 0;
                }
                else
                {
                    Click = 1;
                    opot();
                };
                count.Content = hod + playerMotionColor;
            }
        }
    }
}
