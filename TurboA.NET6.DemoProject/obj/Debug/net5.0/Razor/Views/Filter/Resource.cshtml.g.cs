#pragma checksum "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "db7107b9f9dc7964e3d2cd68145b211a8018670b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Filter_Resource), @"mvc.1.0.view", @"/Views/Filter/Resource.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\_ViewImports.cshtml"
using TurboA.NET6.DemoProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\_ViewImports.cshtml"
using TurboA.NET6.DemoProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"db7107b9f9dc7964e3d2cd68145b211a8018670b", @"/Views/Filter/Resource.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5298333be17cbcbc273ecddef5621a9e7b530894", @"/Views/_ViewImports.cshtml")]
    public class Views_Filter_Resource : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
  
    ViewData["Title"] = "Resource";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Filter-Resource</h1>\r\n<h3>ActionNow:");
#nullable restore
#line 7 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
         Write(base.ViewBag.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n<h3>AlwasRunResultNow:");
#nullable restore
#line 8 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
                 Write(base.Context.Items["AlwasRunResultNow"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n<h3>Now:");
#nullable restore
#line 9 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
   Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n<p>\r\n");
#nullable restore
#line 11 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
     if (1 < 3)
    {
        Console.WriteLine("This is Filter Resource View");

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h4> 111111111111111111 </h4>\r\n");
#nullable restore
#line 15 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Filter\Resource.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
