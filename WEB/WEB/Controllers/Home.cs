using Microsoft.AspNetCore.Mvc;
using WEB.Models;

namespace WEB.Controllers
{
    public class Home : Controller
    {
        [ViewData]
        public string Name { get; set; }
        public IActionResult Index()
        {
            Name = "Лабораторная работа номер 2";
            List<DemoList> demoList = new List<DemoList> { new DemoList(1,"Oleg"), new DemoList(2,"Tima"),
            new DemoList(3,"Sergey")};
            return View(demoList);
        }
    }

}
