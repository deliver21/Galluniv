using GallUni.Data;

namespace GallUni.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IFacultyRepository Faculty { get; private set; }
        public ISpecialityRepository Speciality { get; private set; }
        public ICountryRepository Country { get; private set; }
        public IApplicationUserRepository ApplicationUser {get; private set;}
        public IUserImageRepository UserImage { get; private set;}  
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Faculty = new FacultyRepository(_db);
            Speciality= new SpecialityRepository(_db);
            Country=new CountryRepository(_db);
            UserImage=new UserImageRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
