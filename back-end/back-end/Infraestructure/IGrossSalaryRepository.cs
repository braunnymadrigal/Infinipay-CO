using back_end.Domain;

namespace back_end.Infraestructure
{
    public interface IGrossSalaryRepository
    {
        List<GrossSalaryModel> GetGrossSalaries(string employerId);
    }
}
