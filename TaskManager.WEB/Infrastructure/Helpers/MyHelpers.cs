using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.WEB.Infrastructure.Helpers
{
    public static class MyHelpers
    {
        public static IHtmlString ProfileImage(string id, string imgclass, string height ="45" , string width = "45",
                                     Dictionary<string,string> htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", imgclass);
            builder.MergeAttribute("height", height);
            builder.MergeAttribute("width", width);
            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("src", "/Account/ProfileImage/" + id);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
        public static IHtmlString ProfileImageLink(string id,
                             object htmlAttributes = null)
        {
            var a = new TagBuilder("a");
            a.MergeAttribute("href", "/Account/Details/" + id);
            var img = new TagBuilder("img");
            img.MergeAttribute("class", "img-circle elevation-2");
            img.MergeAttribute("height", "45");
            img.MergeAttribute("width", "45");
            img.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            img.MergeAttribute("src", "/Account/ProfileImage/" + id);
            a.InnerHtml = img.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(a.ToString());
        }
        public static IHtmlString Progress(int progress,
                     object htmlAttributes = null)
        {
            string pr = progress.ToString();
            var div = new TagBuilder("div");
            div.MergeAttribute("class", "progress progress-sm");
            var innerDiv = new TagBuilder("div");
            innerDiv.MergeAttribute("class", "progress-bar bg-green");
            innerDiv.MergeAttribute("role", "progressbar");
            innerDiv.MergeAttribute("aria-volumenow", pr);
            innerDiv.MergeAttribute("aria-volumemin", "0");
            innerDiv.MergeAttribute("aria-volumemax", "100");
            innerDiv.MergeAttribute("style", $"width: {pr}%");
            div.InnerHtml = innerDiv.ToString();
            return MvcHtmlString.Create(div.ToString());
        }
        public static IHtmlString Status(string status,
             object htmlAttributes = null)
        {
            
            var span = new TagBuilder("span");
            switch (status)
            {
                case "Выполняется":
                    span.MergeAttribute("class", "badge badge-warning");                    
                    break;
                case "Завершен":
                    span.MergeAttribute("class", "badge badge-success");
                    break;
                case "Отменен":
                    span.MergeAttribute("class", "badge badge-danger");
                    break;
                default:
                    span.MergeAttribute("class", "badge badge-info");
                    break;

            }
            span.SetInnerText(status);
            return MvcHtmlString.Create(span.ToString());
        }
    }
}