namespace B21_Ex02
{
    public class Player
    {
        // $G$ CSS-999 (-3) this member should be readonly
        private char m_Sign;
        private string m_Name;

        private short m_Score = 0; 

        public Player(string i_PlayerName, char i_Sign)
        {
            m_Name = i_PlayerName;
            m_Sign = i_Sign;
        }

        public string Name
        {
            get { return m_Name; }
        }

        public short Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public char Sign
        {
            get { return m_Sign; }
        }
    }
}
