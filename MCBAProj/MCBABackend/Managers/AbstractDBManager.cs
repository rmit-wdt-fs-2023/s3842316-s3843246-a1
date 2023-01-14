namespace MCBA.Managers
{
	internal abstract class AbstractDBManager
	{
        protected string ConnectionStr;

        //protected string _connectionStr;
        public AbstractDBManager(string connection)
		{
            ConnectionStr = connection;
		}
    }
}

