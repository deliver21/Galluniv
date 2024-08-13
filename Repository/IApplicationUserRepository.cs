using GallUni.Models;

namespace GallUni.Repository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);
    }
}
