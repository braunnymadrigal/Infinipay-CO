namespace back_end.Infraestructure
{
    public interface IUtilityRepository
    {
        string ConvertDatabaseValueToString(Object? databaseValue);
    }
}
