some useful links
-------------------

host header
http://it-notebook.org/iis/article/understanding_host_headers.htm

Each website set up in IIS "binds" to an IP address, port number and host header name. 
Each website's configuration is stored in the metabase property ServerBindings, which has the string format IP:Port:Hostname. 
An example would look like 192.168.0.1:80:www.gafvert.info. 
The host header name (www.gafvert.info in the example) and IP (192.168.0.1 in the example) can be omitted.


How IIS Handle a request?

binding: "IP:Port:HostHeader"(e.g.: "*:59920:test.localhost")

1 checks if website match to "IP + Port + Host Header", if not match, goto next step
2 checks if website match to "*  + Port + Host Header"(in IIS Manager called IP "All Unassigned"), if not match, goto next step
3 checks if website match to "*  + Port + *"(in IIS Manager set a blank host header)


landlord vs tenant
//https://www.codeproject.com/articles/848111/multi-tenancy-system-with-separate-databases-in-mv
//http://nocture.dk/2015/02/12/subdomain-based-multitenancy-asp-net-mvc-5/

//IIS Express config: %USERPROFILE%\My Documents\IISExpress\config\applicationhost.config
//<site name="NbCloud.Web(13)" id="472">
//    <application path="/" applicationPool="Clr4IntegratedAppPool">
//        <virtualDirectory path="/" physicalPath="D:\WS_Local\NbCloudV4x\src\NbCloud.Web" />
//    </application>
//    <bindings>
//        <binding protocol="http" bindingInformation="*:25437:localhost" />
//        <binding protocol="http" bindingInformation="*:25437:tenant1.localhost" />
//        <binding protocol="http" bindingInformation="*:25437:tenant2.localhost" />
//    </bindings>
//</site>

////C:\Windows\System32\drivers\etc\hosts
//127.0.0.1	localhost
//127.0.0.1	tenant1.localhost.com
//127.0.0.1	tenant2.localhost.com

//public class TenantActionFilter : ActionFilterAttribute, IActionFilter
//{
//    public void OnActionExecuting(ActionExecutingContext filterContext) 
//    { 
//        var fullAddress = filterContext.HttpContext.Request.Headers["Host"].Split('.'); 
//        if (fullAddress.Length < 2) 
//        { 
//            filterContext.Result = new HttpStatusCodeResult(404); //or redirect filterContext.Result = new RedirectToRouteResult(..);
//        } 

//        var tenantSubdomain = fullAddress[0]; 

//        // Lookup tenant id (preferably use a cache) 
//        var tenantId = ... filterContext.RouteData.Values.Add("tenant", tenantId);
//        base.OnActionExecuting(filterContext); 
//    }
//}