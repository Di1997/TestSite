#pragma checksum "C:\Users\Di1997\Source\Repos\TestSite\TestSite\Pages\Experiments\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a824e2e39ea5041c4277ab28cf3e43b2b679afc3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TestSite.Pages.Experiments.Pages_Experiments_Index), @"mvc.1.0.razor-page", @"/Pages/Experiments/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Experiments/Index.cshtml", typeof(TestSite.Pages.Experiments.Pages_Experiments_Index), null)]
namespace TestSite.Pages.Experiments
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Di1997\Source\Repos\TestSite\TestSite\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\Di1997\Source\Repos\TestSite\TestSite\Pages\_ViewImports.cshtml"
using TestSite;

#line default
#line hidden
#line 3 "C:\Users\Di1997\Source\Repos\TestSite\TestSite\Pages\_ViewImports.cshtml"
using TestSite.Data;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a824e2e39ea5041c4277ab28cf3e43b2b679afc3", @"/Pages/Experiments/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dc6b36d787dd0b4a1dd3a5b1e7dc018ea14a2c2d", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Experiments_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Di1997\Source\Repos\TestSite\TestSite\Pages\Experiments\Index.cshtml"
  
    ViewData["Title"] = "Home page";

#line default
#line hidden
            BeginContext(71, 85, true);
            WriteLiteral("\r\n\r\n<ul class=\"pagination\" id=\"buttons\">\r\n    <li id=\"1\"><a>1</a></li>\r\n</ul>\r\n\r\n\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(174, 410, true);
                WriteLiteral(@"
    
    <script>
        function test() {
            var count = $($(this).parent()).children().length;
            $(this).addClass(""active"");
            $(""#buttons"").append($(`<li id='${count + 1}'><a>${count + 1}</a></li>`));
            $(this).unbind(""click"");
            $($(this).next()).bind(""click"",test);
        }
        $($(""#buttons"").children()[0]).click(test);
    </script>
");
                EndContext();
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
