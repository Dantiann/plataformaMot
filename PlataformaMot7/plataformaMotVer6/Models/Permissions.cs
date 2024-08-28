using plataformaMotVer6.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace plataformaMotVer6.Models

{
    public class Permissions : ActionFilterAttribute
    {
        private readonly List<string> permittedRoles;

        public Permissions(params string[] roles)
        {
            permittedRoles = roles.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] != null)
            {
                TblUsuarios usuario = HttpContext.Current.Session["Usuario"] as TblUsuarios;
                
                if (!permittedRoles.Contains(usuario.Rol))
                {
                    filterContext.Result = new RedirectResult("~/Home/Home");
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Home/Home");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}