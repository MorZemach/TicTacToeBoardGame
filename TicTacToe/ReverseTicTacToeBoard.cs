namespace B21_Ex02
{
    public class ReverseTicTacToeBoard
    {
        private const byte k_BoardMinimumSize = 3;
        private const byte k_BoardMaximumSize = 9;

        // $G$ CSS-999 (-3) this member should be readonly
        private char[,] m_BoardGame;


        private byte m_BoardSize;
        private byte m_AmountOfFullCells;

        public ReverseTicTacToeBoard(byte i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_AmountOfFullCells = 0;
            m_BoardGame = new char[i_BoardSize, i_BoardSize];
            initializeBoardGame();
        }

        private void initializeBoardGame()
        {
            for(int row = 0; row < m_BoardSize; row++)
            {
                for(int col = 0; col < m_BoardSize; col++)
                {
                    m_BoardGame[row, col] = ' ';
                }
            }
        }

        public char this[int row, int col]
        {
            get { return m_BoardGame[row, col]; }
            private set { m_BoardGame[row, col] = value; }
        }

        public byte BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }

        public byte AmountOfFullCells
        {
            get { return m_AmountOfFullCells; }
        }

        public eInputValidationCheck UpdateCellAtBoardGame(BoardCell io_BoardCell, char i_Sign)
        {
            eInputValidationCheck inputValidationCheckResult;

            if (!cellIsInBoardRange(io_BoardCell))
            {
                inputValidationCheckResult = eInputValidationCheck.OutOfRange;
            }
            else if (!cellIsFree(io_BoardCell))
            {
                inputValidationCheckResult = eInputValidationCheck.OccupiedCell;
            }
            else
            {
                this[io_BoardCell.Row - 1, io_BoardCell.Col - 1] = i_Sign;
                m_AmountOfFullCells++;
                inputValidationCheckResult = eInputValidationCheck.IsVaildInput;
            }

            return inputValidationCheckResult;
        }

        private bool cellIsFree(BoardCell i_BoardCell)
        {
            bool cellIsFree = true;

            if (m_BoardGame[i_BoardCell.Row - 1, i_BoardCell.Col - 1] != ' ')
            {
                cellIsFree = false;
            }

            return cellIsFree;
        }

        private bool cellIsInBoardRange(BoardCell io_BoardCell)
        {
            bool cellIsInRange = false;

            if (legalInputForCell(io_BoardCell.Row) && legalInputForCell(io_BoardCell.Col))
            {
                cellIsInRange = true;
            }

            return cellIsInRange;
        }

        private bool legalInputForCell(byte i_RowOrColCell)
        {
            bool IsLegalInput = false;

            if (i_RowOrColCell > 0 && i_RowOrColCell <= m_BoardSize)
            {
                IsLegalInput = true;
            }

            return IsLegalInput;
        }

        public bool ThereIsASequence(char i_sign, BoardCell i_BoardCell)
        {
            bool thereIsASequence = false;

            if (i_BoardCell.m_Row == i_BoardCell.m_Col)
            {
                thereIsASequence = thereIsASequenceAtStraightDiagonal(i_sign);
            }
            else if (i_BoardCell.m_Row + i_BoardCell.m_Col == m_BoardSize - 1)
            {
                thereIsASequence = thereIsASequenceAtReverseDiagonal(i_sign);
            }

            if (!thereIsASequence)
            {
                thereIsASequence = thereIsSequenceAtRow(i_sign, i_BoardCell.m_Row) ||
                                   thereIsSequenceAtCol(i_sign, i_BoardCell.m_Col);
            }

            return thereIsASequence;
        }

        private bool thereIsSequenceAtRow(char i_sign, byte i_RelevantRow)
        {
            bool sequenceAtRow = true;

            for (int col = 0; col < m_BoardSize && sequenceAtRow; col++)
            {
                if (m_BoardGame[i_RelevantRow - 1, col] != i_sign)
                {
                    sequenceAtRow = false;
                }
            }

            return sequenceAtRow;
        }

        private bool thereIsSequenceAtCol(char i_sign, byte i_RelevantCol)
        {
            bool sequenceAtCol = true;

            for (int row = 0; row < m_BoardSize && sequenceAtCol; row++)
            {
                if (m_BoardGame[row, i_RelevantCol - 1] != i_sign)
                {
                    sequenceAtCol = false;
                }
            }

            return sequenceAtCol;
        }

        private bool thereIsASequenceAtStraightDiagonal(char i_sign)
        {
            bool sequenceAtStraightDiagonal = true;

            for (int diagonal = 0; diagonal < m_BoardSize && sequenceAtStraightDiagonal; diagonal++)
            {
                if (m_BoardGame[diagonal, diagonal] != i_sign)
                {
                    sequenceAtStraightDiagonal = false;
                }
            }

            return sequenceAtStraightDiagonal;
        }

        private bool thereIsASequenceAtReverseDiagonal(char i_sign)
        {
            bool sequenceAtReverseDiagonalm = true;

            for (int row = 0, col = m_BoardSize - 1; row < m_BoardSize && col >= 0
                && sequenceAtReverseDiagonalm; row++, col--)
            {
                if (m_BoardGame[row, col] != i_sign)
                {
                    sequenceAtReverseDiagonalm = false;
                }
            }

            return sequenceAtReverseDiagonalm;
        }

        public static eInputValidationCheck IsTheBoardSizeInRange(byte i_BoardSize)
        {
            eInputValidationCheck inputValidationCheckResult = eInputValidationCheck.IsVaildInput;

            if (i_BoardSize < k_BoardMinimumSize || i_BoardSize > k_BoardMaximumSize)
            {
                inputValidationCheckResult = eInputValidationCheck.OutOfRange;
            }

            return inputValidationCheckResult;
        }
    }
}
