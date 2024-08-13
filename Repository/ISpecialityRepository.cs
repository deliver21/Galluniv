using GallUni.Models;

namespace GallUni.Repository
{
    public interface ISpecialityRepository: IRepository<Speciality>
    {
        void Update(Speciality speciality);
    }
}
