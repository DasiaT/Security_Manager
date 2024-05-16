using Manager_Security_BackEnd.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Models.Generals.General_Valid_Exist
{
    public class General_Valid_Privileges_Exist
    {
        private readonly conectionDBcontext _context;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        public General_Valid_Privileges_Exist(conectionDBcontext context, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<bool> IsValidExistPrivilegesAsync(int Id)
        {
            var privileges = await _context.Privileges.FirstOrDefaultAsync(x => x.Id == Id);

            return privileges == null;
        }
    }
}
