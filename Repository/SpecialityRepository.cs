using GallUni.Data;
using GallUni.Models;
using System.Linq.Expressions;

namespace GallUni.Repository
{
    public class SpecialityRepository : Repository<Speciality>, ISpecialityRepository
    {
        private ApplicationDbContext _db;
        public SpecialityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Speciality speciality)
        {
            _db.Specialities.Update(speciality);
        }
    }
}
