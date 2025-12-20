namespace GraphMaster
{
    public class NotHamiltonianGraphException : GraphMasterException
    {
        public NotHamiltonianGraphException() : base("Граф не содержит гамильтонов цикл")
        {
        }

        public NotHamiltonianGraphException(string message) : base(message)
        {
        }
    }
}

