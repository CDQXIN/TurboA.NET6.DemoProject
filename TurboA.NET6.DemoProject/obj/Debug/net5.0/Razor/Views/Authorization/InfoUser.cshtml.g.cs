#pragma checksum "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Authorization\InfoUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "11473a8414dfb5385dd7fff26c418d73ce324802"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Authorization_InfoUser), @"mvc.1.0.view", @"/Views/Authorization/InfoUser.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"11473a8414dfb5385dd7fff26c418d73ce324802", @"/Views/Authorization/InfoUser.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5298333be17cbcbc273ecddef5621a9e7b530894", @"/Views/_ViewImports.cshtml")]
    public class Views_Authorization_InfoUser : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Authorization\InfoUser.cshtml"
  
    ViewData["Title"] = "Info";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <h1>InfoUser</h1>\r\n<h3>UserName:");
#nullable restore
#line 7 "E:\TurboA.NET6.DemoProject\TurboA.NET6.DemoProject\Views\Authorization\InfoUser.cshtml"
        Write(base.Context.User.Identity.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n\r\n");
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
