using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Element_Pages;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Services.Error_Services;
using Microsoft.EntityFrameworkCore;

namespace Manager_Security_BackEnd.Services.Element_Pages
{
    public class ElementPageServices : IElement_Page
    {
        private readonly conectionDBcontext _context;
        private readonly IError _errorService;
        private readonly General_Generate_Cache_Key _generate_Cache_Key;
        private readonly Element_Page_Error_Manager _element_Page_Error_Manager;
        public ElementPageServices(conectionDBcontext context, IError errorService, Element_Page_Error_Manager element_Page_Error_Manager, General_Generate_Cache_Key generate_Cache_Key)
        {
            _context = context;
            _errorService = errorService;
            _element_Page_Error_Manager = element_Page_Error_Manager;
            _generate_Cache_Key = generate_Cache_Key;
        }
        public async Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> GetElementPageAsync(Comun_Filters value)
        {
            int take = 15;
            int skip = 0;

            Element_Page_Response? results = new();
            List<Element_Page>? element_Pages = [];
            List<ErrorServices> errores = [];

            string Key_Value = "ListElementPages_";

            Key_Value = _generate_Cache_Key.General_GenerateCacheKey(value, Key_Value);//CREA LLAVE UNICA NECESARIA PARA ALMACENAR O BUSCAR EN CACHE

            element_Pages = await _generate_Cache_Key.Buscar_En_CacheAsync<Element_Page>(Key_Value);//VERIFICA SI ESTA EN CACHE

            if (element_Pages?.Count > 0)
            {
                results.Result = element_Pages?.OrderByDescending(x => x.Element_Id).ToList();
                results.Count = element_Pages?.Count;

                return (false, errores, results);
            }
            else
            {
                if (value.Take > 0)//PARA USAR LIMIT DE SQL
                {
                    take = value.Take;
                }

                if (value.Skip > 0)//PARA SALTAR LAS FILAS ES EL OFFSET DE SQL
                {
                    skip = value.Skip;
                }

                if (value.Id != null && value.Search != null)
                {
                    element_Pages = await _context.Element_Page.Where(x => x.Element_Id == value.Id && x.Name_Element.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).ToListAsync();
                }
                else if (value.Id != null)
                {
                    element_Pages = await _context.Element_Page.Where(x => x.Element_Id == value.Id).ToListAsync();
                }
                else if (value.Search != null)
                {
                    element_Pages = await _context.Element_Page.Where(x => x.Name_Element.ToLower().Contains(value.Search.ToLower()))
                        .Skip(skip).Take(take).OrderByDescending(x => x.Element_Id).ToListAsync();
                }
                else
                {
                    element_Pages = await _context.Element_Page.Skip(skip).Take(take).OrderByDescending(x => x.Element_Id).ToListAsync();
                }

                if (element_Pages != null)
                {
                    results.Result = element_Pages;
                    results.Count = element_Pages.Count;

                    await _generate_Cache_Key.Almacenar_En_CacheAsync(Key_Value, element_Pages);//GUARDAR EN CACHE
                }

                return (false, errores, results);
            }
        }

        public async Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> PostElementPageAsync(Element_Page_Request_Post value)
        {
            Element_Page_Response? results = new();
            List<Element_Page> element_Pages = [];
            List<ErrorServices> errores = [];

            errores = await _element_Page_Error_Manager.Element_Page_Valid_Post(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            var new_element = new Element_Page
            {
                Name_Element = value.Name_Element ?? "",
                Description = value.Description ?? "",
                Date_Insert = currentDateUtc
            };

            _context.Element_Page.Add(new_element);

            await _context.SaveChangesAsync();

            element_Pages = await _context.Element_Page.Where(x => x.Name_Element == value.Name_Element).ToListAsync();

            if (element_Pages != null)
            {
                results.Result = element_Pages;
                results.Count = element_Pages.Count();

                element_Pages = await _context.Element_Page.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListElementPages_0_0", element_Pages);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> PatchElementPageAsync(Element_Page_Request_Patch value)
        {
            Element_Page_Response? results = new();
            List<Element_Page>? element_Pages = [];
            List<ErrorServices> errores = [];

            errores = await _element_Page_Error_Manager.Element_Page_Valid_Patch(value);

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var elements = await _context.Element_Page.FirstOrDefaultAsync(x => x.Element_Id == value.Element_Id);

            if (elements == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Element Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            DateTime currentDateUtc = DateTime.UtcNow;

            elements.Name_Element = value.Name_Element != null ? value.Name_Element : elements.Name_Element;
            elements.Description = value.Description != null ? value.Description : elements.Description;
            elements.Date_Update = currentDateUtc;

            await _context.SaveChangesAsync();


            element_Pages = await _context.Element_Page.Where(x => x.Element_Id == value.Element_Id).ToListAsync();

            if (element_Pages != null)
            {
                results.Result = element_Pages;
                results.Count = element_Pages.Count;

                element_Pages = await _context.Element_Page.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListElementPages_0_0", element_Pages);
            }

            return (false, errores, results);
        }
        public async Task<(bool isError, List<ErrorServices> error, Element_Page_Response? result)> DeleteElementPageAsync(Element_Page_Request_Delete value)
        {
            Element_Page_Response? results = new();
            List<Element_Page> element_Pages = [];
            List<ErrorServices> errores = [];

            if (errores.Count > 0)
            {
                return (true, errores, null);
            }

            var elements = await _context.Element_Page.FirstOrDefaultAsync(e => e.Element_Id == value.Element_Id);

            if (elements == null)
            {
                errores.Add(_errorService.GetBadRequestException("The Element Id not exists, insert a valid.", 400));

                return (true, errores, null);
            }

            _context.Element_Page.Remove(elements);

            await _context.SaveChangesAsync();

            if (element_Pages != null)
            {
                results.Result = element_Pages;
                results.Count = element_Pages.Count;

                element_Pages = await _context.Element_Page.ToListAsync();
                await _generate_Cache_Key.Almacenar_En_CacheAsync("ListElementPages_0_0", element_Pages);
            }

            return (false, errores, results);
        }
    }
}
