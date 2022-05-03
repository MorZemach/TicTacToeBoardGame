namespace B21_Ex02
{
    using System;
    using System.Text;
    using System.Threading;

    public class ConsoleReverseTicTacToeBoardGameUI
    {
        public const char k_QuitGameSign = 'Q';
        public const byte k_FirstPlayerNumber = 1;
        public const byte k_SecondPlayerNumber = 2;
        public const byte k_IndexRowInStringInput = 0;
        public const byte k_IndexColInStringInput = 2;
        private static ReverseTicTacToeGame m_Game;

        public static void StartGame()
        {
            buildGame();
            printBoard();
            runGame();
        }

        private static void buildGame()
        {
            byte boardSize;
            string firstPlayerName, secondPlayerName;
            bool isComputerPlaying;

            boardSize = getBoardSize();
            firstPlayerName = getPlayerName(k_FirstPlayerNumber);
            isComputerPlaying = checkIfTheGameIsAgainstTheComputer();
            if (isComputerPlaying)
            {
                m_Game = new ReverseTicTacToeGame(isComputerPlaying, firstPlayerName, "Computer", boardSize);
            }
            else
            {
                secondPlayerName = getPlayerName(k_SecondPlayerNumber);
                m_Game = new ReverseTicTacToeGame(isComputerPlaying, firstPlayerName, secondPlayerName, boardSize);
            }

        }   

        private static void runGame()
        {
            Player currentPlayer = m_Game.PlayerOne;

            while(m_Game.StatusGame == eCurrentStatusOfTheGame.PlayingGame)
            {
                playCurrentPlayerTurn(ref currentPlayer);
            }

            displayCurrentStatusOfTheGame();
            endOfRound();
        }

        private static string getPlayerName(byte i_PlayerNumber)
        {
            string playerName;
            string msg;

            msg = string.Format("Please enter the name of player number {0}:", i_PlayerNumber);
            Console.WriteLine(msg);
            playerName = Console.ReadLine();
            while (playerName.Length == 0)
            {
                Console.WriteLine("You are must to choose player name.");
                playerName = Console.ReadLine();
            }

            return playerName;
        }

        private static bool checkIfTheGameIsAgainstTheComputer()
        {
            Console.Write("Would you like to play against the computer? ");

            return getYesOrNoAnswer();
        }

        private static byte getBoardSize()
        {
            byte boardSize;
            bool isByteInput = false;
            string msg;

            msg = string.Format(@"Please enter the size of 'Reverse Tic Tac Toe' game board. 
The size must be between 3 to 9 only.");
            Console.WriteLine(msg);
            isByteInput = byte.TryParse(Console.ReadLine(), out boardSize);
            while (!isByteInput || ReverseTicTacToeBoard.IsTheBoardSizeInRange(boardSize) != 
                                   eInputValidationCheck.IsVaildInput)
            {
                if (!isByteInput)
                {
                    Console.WriteLine("Invaild input, You must enter a number. Let's try again!");
                }
                else
                {
                    Console.WriteLine("The input exceeds the possible size range of the game board, let's try again!");
                }

                isByteInput = byte.TryParse(Console.ReadLine(), out boardSize);
            }

            return boardSize;
        }

        private static void getMoveFromUser(Player io_CurrentPlayer, ref BoardCell io_userSelectedBoardCell)
        {
            string userInput;

            userInput = Console.ReadLine();
            while (!checkIfUserInputEnterInRightFormat(userInput) && !checkIfUserDecidedToQuitGame(userInput, io_CurrentPlayer))
            {
                Console.WriteLine("The input you enter is not in the right format, let's try again!");
                userInput = Console.ReadLine();
            }

            if(m_Game.StatusGame == eCurrentStatusOfTheGame.PlayingGame)
            {
                convertStringToBoardCell(userInput, ref io_userSelectedBoardCell);
            }

        }

        private static bool checkIfUserDecidedToQuitGame(string i_UserInput, Player i_CurrentPlayer)
        {
            bool userDecidedToQuitGame = false; 

            if(i_UserInput == Char.ToString(k_QuitGameSign))
            {
                userDecidedToQuitGame = true;
                if (i_CurrentPlayer == m_Game.PlayerOne)
                {
                    m_Game.StatusGame = eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame;
                }
                else
                {
                    m_Game.StatusGame = eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame;
                }

                m_Game.UpdatePlayerScore(i_CurrentPlayer);
            }

            return userDecidedToQuitGame;
        }

        private static void convertStringToBoardCell(string i_UserInput, ref BoardCell io_UserSelectedBoardCell)
        {
            io_UserSelectedBoardCell.Row = Convert.ToByte(char.GetNumericValue(i_UserInput[k_IndexRowInStringInput]));
            io_UserSelectedBoardCell.Col = Convert.ToByte(char.GetNumericValue(i_UserInput[k_IndexColInStringInput]));
        }

        private static bool checkIfUserInputEnterInRightFormat(string i_UserInput)
        {
            bool userInputIsInRightFormat = false;

            if(i_UserInput.Length == 3 && char.IsDigit(i_UserInput[k_IndexRowInStringInput]) &&
               char.IsDigit(i_UserInput[k_IndexColInStringInput]) && i_UserInput[k_IndexRowInStringInput + 1] == ' ')
            {
                userInputIsInRightFormat = true;
            }

            return userInputIsInRightFormat;
        }

        private static void playCurrentPlayerTurn(ref Player io_CurrentPlayer)
        {
            eInputValidationCheck turnPlayedProperly = eInputValidationCheck.GeneralError;
            BoardCell playerSelectedCell = new BoardCell();
            bool isUserDecidedToQuitGame = false;
            bool firstTryOfThePlayer = true; 

            while (turnPlayedProperly != eInputValidationCheck.IsVaildInput && !isUserDecidedToQuitGame)
            {
                playerSelectedCell = chooseMoveToPlay(io_CurrentPlayer, ref firstTryOfThePlayer);

                if (m_Game.StatusGame == eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame ||
                    m_Game.StatusGame == eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame)
                {
                    isUserDecidedToQuitGame = true;
                }
                else
                {
                    m_Game.UpdateStatusGame(playerSelectedCell, ref io_CurrentPlayer, ref turnPlayedProperly);

                    if (m_Game.IsComputerPlaying == false || io_CurrentPlayer == m_Game.PlayerOne)
                    {
                        if (turnPlayedProperly == eInputValidationCheck.OccupiedCell)
                        {
                            Console.WriteLine("The cell is occupied, let's try again!");
                        }
                        else if (turnPlayedProperly == eInputValidationCheck.OutOfRange)
                        {
                            Console.WriteLine("The cell you chose is out of board range, let's try again!");
                        }
                    }
                }
            }

            printBoard();
        }

        private static BoardCell chooseMoveToPlay(Player i_CurrentPlayer, ref bool io_FirstTryOfHumanPlayer)
        {
            BoardCell playerSelectedCell = new BoardCell();
            string msg;

            if (m_Game.IsComputerPlaying == true && i_CurrentPlayer == m_Game.PlayerTwo)
            {
                m_Game.ComputerMove(ref playerSelectedCell);
                Thread.Sleep(150);
            }
            else
            {
                if(io_FirstTryOfHumanPlayer == true)
                {
                    msg = string.Format(@"It is {0} turn.
Please enter the cell that you want to place your sign {1}.
Enter according to the following order: row number (space) col number.
To quit the game, press 'Q'.", i_CurrentPlayer.Name, i_CurrentPlayer.Sign);
                    Console.WriteLine(msg);
                    io_FirstTryOfHumanPlayer = false;
                }

                getMoveFromUser(i_CurrentPlayer, ref playerSelectedCell);
            }

            return playerSelectedCell;
        }

        private static void displayCurrentStatusOfTheGame()
        {
            string stautusScore;

            stautusScore = string.Format(@"
The status score so far:
{0} has {1} points.
{2} has {3} points.
", m_Game.PlayerOne.Name, m_Game.PlayerOne.Score, m_Game.PlayerTwo.Name, m_Game.PlayerTwo.Score);

            switch(m_Game.StatusGame)
            {
                case eCurrentStatusOfTheGame.ItIsADraw:
                    {
                        Console.WriteLine("OH NO! we have a tie!");
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerOneLost:
                    {
                        string msg = string.Format(@"Well done {0}! you are the winner at this round!", m_Game.PlayerTwo.Name);
                        Console.WriteLine(msg);
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerTwoLost:
                    {
                        string msg = string.Format(@"Well done {0}! you are the winner at this round!", m_Game.PlayerOne.Name);
                        Console.WriteLine(msg);
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerOneChoseToQuitTheGame:
                    {
                        string msg = string.Format(@"OH NO! {0} decided to quit the game!", m_Game.PlayerOne.Name);
                        Console.WriteLine(msg);
                        break;
                    }
                case eCurrentStatusOfTheGame.PlayerTwoChoseToQuitTheGame:
                    {
                        string msg = string.Format(@"OH NO! {0} decided to quit the game!", m_Game.PlayerTwo.Name);
                        Console.WriteLine(msg);
                        break;
                    }

            }

            Console.WriteLine(stautusScore);
        }

        private static void endOfRound()
        {
            bool isPlayerWantToPlayAnotherRound = false;

            isPlayerWantToPlayAnotherRound = checkIfPlayerWantAnotherRound();

            if(isPlayerWantToPlayAnotherRound == true)
            {
                m_Game.InitializationGameForNewRound(m_Game.BoardGame.BoardSize);
                printBoard();
                runGame();
            }
            else
            {
                Console.WriteLine("You decided to stop playing. See you next time!");
            }

        }

        private static bool checkIfPlayerWantAnotherRound()
        {
            Console.Write("Would you like to play another round? ");

            return getYesOrNoAnswer();
        }

        // $G$ CSS-999 (-3) You should have used constants\enum here.
        private static bool getYesOrNoAnswer()
        {
            bool userDecidedYes = false;
            string userDecision;

            Console.WriteLine(@"choose 'y' (for yes) or 'n' (for no).");
            userDecision = Console.ReadLine();
            while (userDecision != "y" && userDecision != "n" && userDecision != "Y" && userDecision != "N")
            {
                Console.WriteLine("It's a yes or no question only, let's try again.");
                userDecision = Console.ReadLine();
            }

            userDecidedYes = (userDecision == "y" || userDecision == "Y") ? true : false;

            return userDecidedYes;
        }

        private static void printBoard()
        {
            StringBuilder boardGame = new StringBuilder();

            Ex02.ConsoleUtils.Screen.Clear();
            printColumnsNumbersLine(ref boardGame);

            for (byte row = 0; row < m_Game.BoardGame.BoardSize; row++)
            {
                printCurrentRowAtBoard(ref boardGame, row);
            }

            Console.WriteLine(boardGame);
        }

        private static void printCurrentRowAtBoard(ref StringBuilder io_BoardGame, byte i_RowNumber)
        {
            io_BoardGame.Append(i_RowNumber + 1);
            printDataOfCurrentRow(ref io_BoardGame, i_RowNumber);
            printSeparationOfLines(ref io_BoardGame);
        }

        private static void printDataOfCurrentRow(ref StringBuilder io_BoardGame, byte i_Row)
        {
            for (int col = 0; col < m_Game.BoardGame.BoardSize; col++)
            {
                io_BoardGame.Append("| " + m_Game.BoardGame[i_Row, col] + " ");

                if(col == m_Game.BoardGame.BoardSize - 1)
                {
                    io_BoardGame.Append("|");
                }
            }

            io_BoardGame.Append(Environment.NewLine);
        }

        private static void printColumnsNumbersLine(ref StringBuilder io_BoardGame)
        {
            for (int col = 0; col < m_Game.BoardGame.BoardSize; col++)
            {
                io_BoardGame.Append("   " + (col + 1));
            }

            io_BoardGame.Append(Environment.NewLine);
        }

        private static void printSeparationOfLines(ref StringBuilder io_BoardGame)
        {
            for(int sizeBoardRow = 0; sizeBoardRow < m_Game.BoardGame.BoardSize; sizeBoardRow++)
            {
                if(sizeBoardRow == 0)
                {
                    io_BoardGame.Append("  ");
                }

                io_BoardGame.Append("====");
            }

            io_BoardGame.Append(Environment.NewLine);
        }
    }
}
