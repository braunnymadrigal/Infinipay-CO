namespace back_end.Infraestructure
{
    public class UtilityRepository : IUtilityRepository
    {
        public string ConvertDatabaseValueToString(Object? databaseValue)
        {
            if (databaseValue == null)
            {
                throw new Exception("Could not convert database value to string.");
            }
            var valueOfReturn = "";
            var convertedValue = Convert.ToString(databaseValue);
            if (convertedValue != null)
            {
                valueOfReturn = convertedValue;
            }
            return valueOfReturn;
        }
    }
}
