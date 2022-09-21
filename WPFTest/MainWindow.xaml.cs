using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Media;
using WPFTest.Models;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MatrixElement> _board;

        private List<int> _marked;

        private int _counter = 0;

        private int _wins = 0;

        private readonly string _levelsJSONPath = "Levels/lights-out-levels.json";

        private readonly string _onImage = "Assets/PNG/IMAGE_on.png";

        private readonly string _offImage = "Assets/PNG/IMAGE_off.png";
        public string Rows { get; set; }
        public string Columns { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FillComboWithLevels();
        }
        private void FillComboWithLevels()
        {
            using (StreamReader r = new StreamReader(_levelsJSONPath))
            {
                string json = r.ReadToEnd();
                List<Level> items = JsonConvert.DeserializeObject<List<Level>>(json);
                foreach (var item in items)
                {
                    CmbGamelevel.Items.Add(item.Name);
                }
                CmbGamelevel.SelectedIndex = 0;
            }
        }
        private void CreateDrawings(int rows, int columns, List<int> markedList)
        {
            _board = new List<MatrixElement>();
            int markedPosition = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    bool IsMarked = markedList.Contains(markedPosition);
                    if (IsMarked) { _board.Add(new MatrixElement(r, c) { Image = _onImage }); }
                    else { _board.Add(new MatrixElement(r, c) { Image = _offImage }); }
                    markedPosition++;
                }
            }
            Board.ItemsSource = _board;
        }
        private void LoadLevel(string levelName)
        {
            _marked = new List<int>();
            using (StreamReader r = new StreamReader(_levelsJSONPath))
            {
                string json = r.ReadToEnd();
                List<Level> items = JsonConvert.DeserializeObject<List<Level>>(json);
                foreach (var item in items)
                {
                    if (item.Name.ToString() == levelName)
                    {
                        Rows = item.Rows.ToString();
                        Columns = item.Columns.ToString();
                        _marked.AddRange(item.On);
                    }
                }
                CreateDrawings(Int32.Parse(Rows), Int32.Parse(Columns), _marked);
            }
        }
        private void RenderStage(int x, int y)
        {
            List<MatrixElement> nextStepMatrix = new List<MatrixElement>();

            var currentPoint = _board.FirstOrDefault(r => r.X == x && r.Y == y);
            if (currentPoint != null)
            {

                if (currentPoint.Image == _offImage)
                {
                    currentPoint.Image = _onImage;
                }
                else if (currentPoint.Image == _onImage)
                {
                    currentPoint.Image = _offImage;
                }
            }

            var westPoint = _board.FirstOrDefault(r => r.X == (x - 1) && r.Y == y);
            if (westPoint != null)
            {
                if (westPoint.Image == _offImage)
                {
                    westPoint.Image = _onImage;
                }
                else if (westPoint.Image == _onImage)
                {
                    westPoint.Image = _offImage;
                }
            }

            var northPoint = _board.FirstOrDefault(r => r.X == x && r.Y == (y - 1));
            if (northPoint != null)
            {
                if (northPoint.Image == _offImage)
                {
                    northPoint.Image = _onImage;
                }
                else if (northPoint.Image == _onImage)
                {
                    northPoint.Image = _offImage;
                }
            }

            var eastPoint = _board.FirstOrDefault(r => r.X == (x + 1) && r.Y == y);
            if (eastPoint != null)
            {
                if (eastPoint.Image == _offImage)
                {
                    eastPoint.Image = _onImage;
                }
                else if (eastPoint.Image == _onImage)
                {
                    eastPoint.Image = _offImage;
                }
            }

            var southPoint = _board.FirstOrDefault(r => r.X == x && r.Y == (y + 1));
            if (southPoint != null)
            {
                if (southPoint.Image == _offImage)
                {
                    southPoint.Image = _onImage;
                }
                else if (southPoint.Image == _onImage)
                {
                    southPoint.Image = _offImage;
                }
            }

            foreach (var el in _board)
            {
                nextStepMatrix.Add(new MatrixElement(el.X, el.Y) { Image = el.Image });
            }

            Board.ItemsSource = nextStepMatrix;
            var onpoints = nextStepMatrix.FirstOrDefault(r => r.Image == _onImage);

            if (onpoints == null)
            {
                GameStatus.Visibility = Visibility.Visible;
                Board.Background = Brushes.Green;
                _wins++;
                UserWins.Text = _wins.ToString();
            }
            else if (onpoints != null)
            {
                GameStatus.Visibility = Visibility.Collapsed;
                Board.Background = Brushes.Red;
            }
        }
        private void CellClick(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            var point = (MatrixElement)border.Tag;
            _counter++;
            UserMoves.Text = _counter.ToString();
            RenderStage(point.X, point.Y);
        }
        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            _counter = 0;
            _wins = 0;
            UserMoves.Text = _counter.ToString();
            UserWins.Text = _wins.ToString();
            GameStatus.Visibility = Visibility.Collapsed;
            Board.Background = Brushes.Red;
            string levelName = CmbGamelevel.SelectedItem.ToString();
            LoadLevel(levelName);
        }
    }
}
