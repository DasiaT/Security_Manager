using Manager_Security_BackEnd.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Models.Generals.General_Valid_Exist
{
    public class General_Valid_User_Exist
    {
        private readonly conectionDBcontext _context;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public General_Valid_User_Exist(conectionDBcontext context, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<bool> IsValidExistUserAsync(int Id)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.User_Id == Id);

            return user == null;
        }
    }
}
