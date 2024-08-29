namespace GeniusSquareWeb.Server.SolversWithDelay
{
    /// <summary>
    /// Game over exception
    /// </summary>
    public class GameOverException : Exception
    {
        public GameOverException(string errorMessage) :
            base(errorMessage)
        {
        }
    }
}
