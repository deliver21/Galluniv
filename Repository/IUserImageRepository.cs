using GallUni.Models;

namespace GallUni.Repository
{
    public interface IUserImageRepository:IRepository<UserImage>
    {
        void Update(UserImage userImage);
    }
}
