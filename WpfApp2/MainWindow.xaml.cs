using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace YaKrestik
{
    public partial class MainWindow : Window
    {
        private bool isPlayerX = true; 
        private int[,] board = new int[3, 3];
        private object grid;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CellButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (board[row, col] == 0)
            {
                button.Content = "X";
                board[row, col] = 1;

                if (CheckForWinner())
                {
                    MessageBox.Show("я выграл кампуктер");
                    ResetGame();
                }
                else if (IsBoardFull())
                {
                    MessageBox.Show("Никто не выйграл поэтом всем по пивасу");
                    ResetGame();
                }
                else
                {
                    MakeComputerMove();
                }
            }
        }

        private void MakeComputerMove()
        {
            List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        emptyCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            if (emptyCells.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, emptyCells.Count);
                Tuple<int, int> computerMove = emptyCells[randomIndex];

                Button computerButton = GetButtonByRowAndColumn(computerMove.Item1, computerMove.Item2);
                computerButton.Content = "O";
                board[computerMove.Item1, computerMove.Item2] = -1;

                if (CheckForWinner())
                {
                    MessageBox.Show("кампуктер выйгряль(");
                    ResetGame();
                }
                else if (IsBoardFull())
                {
                    MessageBox.Show("выйграла передружба");
                    ResetGame();
                }
            }
        }

        private Button GetButtonByRowAndColumn(int row, int col)
        {
            string buttonName = $"btn{row}{col}";
            return (Button)this.FindName(buttonName);
        }


        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(board[i, 0] + board[i, 1] + board[i, 2]) == 3 ||
                    Math.Abs(board[0, i] + board[1, i] + board[2, i]) == 3)
                {
                    return true;
                }
            }

            if (Math.Abs(board[0, 0] + board[1, 1] + board[2, 2]) == 3 ||
                Math.Abs(board[0, 2] + board[1, 1] + board[2, 0]) == 3)
            {
                return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private void ResetGame()
        {
            isPlayerX = true;
            board = new int[3, 3];

            foreach (var button in GetAllButtons())
            {
                button.Content = "";
            }
        }

        private IEnumerable<Button> GetAllButtons()
        {
            foreach (var child in LogicalTreeHelper.GetChildren((DependencyObject)this.grid))
            {
                if (child is Button button)
                {
                    yield return button;
                }
            }
        }
    }
}
