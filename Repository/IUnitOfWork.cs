namespace GallUni.Repository
{
    public interface IUnitOfWork
    {
        IFacultyRepository Faculty { get; }
        ISpecialityRepository Speciality { get; }
        ICountryRepository Country { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IUserImageRepository UserImage { get; }
        void Save();
    }
}
