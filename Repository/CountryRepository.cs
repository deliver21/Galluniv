using GallUni.Data;
using GallUni.Models;

namespace GallUni.Repository
{
    public class CountryRepository:Repository<Country>, ICountryRepository
    {
        private ApplicationDbContext _db;
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Country country)
        {
            _db.Countries.Update(country);
        }
    }
}
