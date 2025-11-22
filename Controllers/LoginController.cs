using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.ViewModels; // Asegúrate de que coincida con donde creaste el ViewModel

public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    
    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
       
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Debe ingresar usuario y contraseña.";
            return View("Index", model); // Devuelve la vista con el error
        }

        
        if (_authenticationService.Login(model.Username, model.Password))
        {
            
            return RedirectToAction("Index", "Home");
        }

        
        model.ErrorMessage = "Credenciales inválidas.";
        return View("Index", model);
    }

    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index"); 
    }
}