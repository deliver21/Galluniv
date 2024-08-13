using GallUni.Models;

namespace GallUni.Repository
{
    public interface IFacultyRepository:IRepository<Faculty>
    {
        void Update(Faculty faculty);   
    }
}
