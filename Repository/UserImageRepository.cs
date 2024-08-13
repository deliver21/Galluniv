using GallUni.Data;
using GallUni.Models;

namespace GallUni.Repository
{
    public class UserImageRepository : Repository<UserImage>, IUserImageRepository
    {
        private ApplicationDbContext _db;
        public UserImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserImage userImage)
        {
            _db.UserImages.Update(userImage);
        }
    }
}
