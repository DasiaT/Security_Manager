using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Applications;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;


namespace Manager_Security_BackEnd.Services.Application_Services
{
    public class Application_Error_Manager
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        public Application_Error_Manager(conectionDBcontext context, IError errorService)
        {
            _context = context;
            _errorService = errorService;
        }

        public async Task<List<ErrorServices>> Application_Valid_Post(Application_Request_Post value)
        {
            List<ErrorServices> errores = new();

            if (value.Emp_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Emp_Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Application_Name) || string.IsNullOrWhiteSpace(value.Application_Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Application Name field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.URL_Server) || string.IsNullOrWhiteSpace(value.URL_Server))
            {
                errores.Add(_errorService.GetBadRequestException("The Application Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoCompany = await _context.Company.FirstOrDefaultAsync(x => x.Emp_Id == value.Emp_Id);

                if (validoCompany == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Emp_Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Application.FirstOrDefaultAsync(x => x.Application_Name == value.Application_Name);

                if (validoName != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Name already exists in another user.", 400));
                }

                var validoURL = await _context.Application.FirstOrDefaultAsync(x => x.Application_Name == value.Application_Name && x.URL_Server == value.URL_Server);

                if (validoURL != null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Name & URL server already exists in another user.", 400));
                }


            }


            return errores;
        }

        public async Task<List<ErrorServices>> Application_Valid_Patch(Application_Request_Patch value)
        {
            List<ErrorServices> errores = new();

            if (value.Emp_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Emp_Id field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.Application_Name) || string.IsNullOrWhiteSpace(value.Application_Name))
            {
                errores.Add(_errorService.GetBadRequestException("The Application Name field cannot be empty.", 400));
            }

            if (string.IsNullOrEmpty(value.URL_Server) || string.IsNullOrWhiteSpace(value.URL_Server))
            {
                errores.Add(_errorService.GetBadRequestException("The Application Name field cannot be empty.", 400));
            }

            if (errores.Count == 0)
            {
                var validoCompany = await _context.Company.FirstOrDefaultAsync(x => x.Emp_Id == value.Emp_Id);

                if (validoCompany == null)
                {
                    errores.Add(_errorService.GetBadRequestException("The Emp_Id not exists, insert a valid.", 400));
                }

                var validoName = await _context.Application.FirstOrDefaultAsync(x => x.Application_Name == value.Application_Name);

                if (validoName != null && validoName.Application_Id != value.Application_Id)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Name already exists in another user.", 400));
                }

                var validoURL = await _context.Application.FirstOrDefaultAsync(x => x.Application_Name == value.Application_Name);

                if (validoURL != null && validoURL.URL_Server != value.URL_Server)
                {
                    errores.Add(_errorService.GetBadRequestException("The Application Name & URL server already exists in another user.", 400));
                }


            }


            return errores;
        }

        public List<ErrorServices> Application_Valid_Delete(Application_Request_Delete value)
        {
            List<ErrorServices> errores = [];

            if (value.Application_Id == 0)
            {
                errores.Add(_errorService.GetBadRequestException("The Application Id field cannot be empty.", 400));
            }

            return errores;
        }
    }
}
