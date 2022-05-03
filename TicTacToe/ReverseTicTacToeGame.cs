namespace B21_Ex02
{
    using System;
    public class ReverseTicTacToeGame
    {
        public const char k_FirstPlayerSign = 'X';
        public const char k_SecondPlayerSign = 'O';
        private readonly bool r_IsComputerPlaying;
        private ReverseTicTacToeBoard m_BoardGame;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private eCurrentStatusOfTheGame m_StatusGame; 

        public ReverseTicTacToeGame(bool i_IsPlayingAgainstComputer, string i_FirstPlayerName,
                             string i_SecondPlayerName, byte i_BoardSize)
        {
            m_FirstPlayer = new Player(i_FirstPlayerName, k_FirstPlayerSign);
            m_SecondPlayer = new Player(i_SecondPlayerName, k_SecondPlayerSign);
            m_BoardGame = new ReverseTicTacToeBoard(i_BoardSize);
            r_IsComputerPlaying = i_IsPlayingAgainstComputer;
            m_StatusGame = eCurrentStatusOfTheGame.PlayingGame;
        }

        public Player PlayerOne
        {
            get { return m_FirstPlayer; }
        }

        public Player PlayerTwo
        {
            get { return m_SecondPlayer; }
        }

        public ReverseTicTacToeBoard BoardGame
        {
            get { return m_BoardGame; }
        }

        public bool IsComputerPlaying
        {
            get { return r_IsComputerPlaying; }
        }

        public eCurrentStatusOfTheGame StatusGame
        {
            get { return m_StatusGame; }
            set { m_StatusGame = value; }
        }

        public void UpdateStatusGame(BoardCell i_PlayerMove, ref Player io_CurrentPlayer,
                                     ref eInputValidationCheck io_TurnStatusAtBoardGame)
        {
            io_TurnStatusAtBoardGame = doPlayerMove(i_PlayerMove, ref io_CurrentPlayer);

            if (io_TurnStatusAtBoardGame == eInputValidationCheck.IsVaildInput)
            {
                if (ThePlayerLostTheGame(io_CurrentPlayer.Sign, i_PlayerMove))
                {
                    if(io_CurrentPlayer == m_FirstPlayer)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerOneLost;
                    }
                    else if(io_CurrentPlayer == m_SecondPlayer)
                    {
                        m_StatusGame = eCurrentStatusOfTheGame.PlayerTwoLost;
                    }

                    UpdatePlayerScore(io_CurrentPlayer);
                }
                else if(CheckIfGameIsOver())
                {
                    m_StatusGame = eCurrentStatusOfTheGame.ItIsADraw;
                }
                else
                {
                    m_StatusGame = eCurrentStatusOfTheGame.PlayingGame;
                    SwitchPlayers(ref io_CurrentPlayer);
                }

            }
        }


        // $G$ NTT-007 (-10) There's no need to re-instantiate the Random instance each time it is used.
        public void ComputerMove(ref BoardCell io_ComputerSelectedCell)
        {
            Random move = new Random();

            io_ComputerSelectedCell.Row = Convert.ToByte(move.Next(1, m_BoardGame.BoardSize + 1));
            io_ComputerSelectedCell.Col = Convert.ToByte(move.Next(1, m_BoardGame.BoardSize + 1));
        }

        private eInputValidationCheck doPlayerMove(BoardCell io_PlayerMove, ref Player io_CurrentPlayer)
        {
            eInputValidationCheck isMoveDoneProperly;

            isMoveDoneProperly = m_BoardGame.UpdateCellAtBoardGame(io_PlayerMove, io_CurrentPlayer.Sign);

            return isMoveDoneProperly;
        }

        public void SwitchPlayers(ref Player io_CurrentPlayer)
        {
            if (io_CurrentPlayer == PlayerOne)
            {
                io_CurrentPlayer = PlayerTwo;
            }
            else
            {
                io_CurrentPlayer = PlayerOne;
            }

        }

        public void UpdatePlayerScore(Player i_CurrentPlayer)
        {
            if (i_CurrentPlayer == PlayerOne)
            {
                PlayerTwo.Score++;
            }
            else
            {
                PlayerOne.Score++;
            }

        }

        public bool ItIsADraw()
        {
            bool itIsADraw = true;

            if (m_BoardGame.AmountOfFullCells < Math.Pow(m_BoardGame.BoardSize, 2))
            {
                itIsADraw = false;
            }

            return itIsADraw;
        }

        // $G$ CSS-013 (-3) Input parameters names should start with i_PascaleCase.
        public bool ThePlayerLostTheGame(char io_Sign, BoardCell io_BoardCell)
        {
            bool thePlayerLostTheGame = false;

            if(m_BoardGame.ThereIsASequence(io_Sign, io_BoardCell))
            {
                thePlayerLostTheGame = true;
            }

            return thePlayerLostTheGame;
        }

        public bool CheckIfGameIsOver()
        {
            bool gameOver = false;

            if (ItIsADraw())
            {
                gameOver = true; 
            }

            return gameOver;
        }

        public void InitializationGameForNewRound(byte i_BoardSize)
        {
            m_BoardGame = new ReverseTicTacToeBoard(i_BoardSize);
            m_StatusGame = eCurrentStatusOfTheGame.PlayingGame;
        }
    }
}
