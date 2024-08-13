using GallUni.Models;

namespace GallUni.Repository
{
    public interface ICountryRepository:IRepository<Country>
    {
        void Update(Country country);
    }
}
