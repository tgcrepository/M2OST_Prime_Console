// Decompiled with JetBrains decompiler
// Type: IBHFL.Controllers.HomeController
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Web.Mvc;

namespace IBHFL.Controllers
{
    public class HomeController : Controller
  {
    public ActionResult Index() => (ActionResult) this.View();

    public ActionResult Logout(string session)
    {
      this.ViewData["type"] = (object) session;
      return (ActionResult) this.View();
    }
  }
}
