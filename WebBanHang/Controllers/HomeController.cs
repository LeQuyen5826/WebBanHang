using Microsoft.AspNetCore.Mvc;

namespace WebBanHang.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error() => View();
}
