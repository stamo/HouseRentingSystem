namespace HouseRentingSystem.Core.Exceptions
{
    public class HouseRentingException : ApplicationException
    {
        public HouseRentingException()
        {
                
        }

        public HouseRentingException(string errorMessage)
            : base(errorMessage)
        {

        }
    }
}
