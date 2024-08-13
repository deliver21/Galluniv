using GallUni.Data;
using GallUni.Models;

namespace GallUni.Repository
{
    public class FacultyRepository: Repository<Faculty>, IFacultyRepository
    {
        private ApplicationDbContext _db;
        public FacultyRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Faculty faculty)
        {
            _db.Faculties.Update(faculty);
        }
    }
}
