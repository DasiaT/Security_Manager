using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Companys;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Company_Services
{
    public class Company_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Company_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }
        public async Task<List<ErrorServices>> Company_Valid_Post(Company_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoName = await _context.Company.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoName != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Name already exists in another user.", 400));
                }

            }


            return errores;
        }

        public async Task<List<ErrorServices>> Company_Valid_Patch(Company_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Emp_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Emp_Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Name) || string.IsNullOrWhiteSpace(value.Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoID = await _context.Company.FirstOrDefaultAsync(x => x.Emp_Id == value.Emp_Id);

                if (validoID == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Emp_Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Company.FirstOrDefaultAsync(x => x.Name == value.Name);

                if (validoName != null && validoName.Emp_Id != value.Emp_Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Name already exists in another user.", 400));
                }
            }


            return errores;
        }
    }
}
