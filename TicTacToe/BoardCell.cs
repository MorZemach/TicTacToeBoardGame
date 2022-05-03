namespace B21_Ex02
{
    public struct BoardCell
    {
        public byte m_Row;
        public byte m_Col;

        public BoardCell(byte i_Row, byte i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public byte Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public byte Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }
    }
}
