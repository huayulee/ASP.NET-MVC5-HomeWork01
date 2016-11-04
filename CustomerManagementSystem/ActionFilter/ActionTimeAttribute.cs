using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerManagementSystem.ActionFilter
{
    public class ActionTimeAttribute: ActionFilterAttribute
    {
        public DateTime begin { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // 2.
            DateTime now = DateTime.Now;
            var totSpanAction = now - begin;
            Debug.WriteLine("OnActionExecuted_" + now.ToString());
            Debug.WriteLine(string.Format("--------{0}---------", totSpanAction.ToString()));
            filterContext.Controller.ViewBag.totSpanAction = totSpanAction;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 1.
            begin = DateTime.Now;
            Debug.WriteLine("OnActionExecuting_" + begin.ToString());
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            DateTime now = DateTime.Now;
            var totSpanActionResult = now - begin;
            Debug.WriteLine("OnResultExecuted_" + now.ToString());
            Debug.WriteLine(string.Format("--------{0}---------", totSpanActionResult.ToString()));
            filterContext.Controller.ViewBag.totSpanActionResult = totSpanActionResult;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            begin = DateTime.Now;
            Debug.WriteLine("OnResultExecuting_" + begin.ToString());
        }
    }
}