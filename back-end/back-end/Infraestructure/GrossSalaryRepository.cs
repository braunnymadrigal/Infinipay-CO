namespace back_end.Infraestructure
{
    public class GrossSalaryRepository : IGrossSalaryRepository
    {
        private readonly AbstractConnectionRepository connectionRepository;
        private readonly IUtilityRepository utilityRepository;

        public GrossSalaryRepository()
        {
            connectionRepository = new ConnectionRepository();
            utilityRepository = new UtilityRepository();
        }
    }
}
