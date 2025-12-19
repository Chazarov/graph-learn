namespace GraphMaster
{
    public class NotEulerianGraphException : GraphMasterException
    {
        public NotEulerianGraphException() : base("Граф не является эйлеровым")
        {
        }

        public NotEulerianGraphException(string message) : base(message)
        {
        }
    }
}

