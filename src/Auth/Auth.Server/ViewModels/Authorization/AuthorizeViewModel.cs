using System.ComponentModel.DataAnnotations;

namespace Dyvenix.App1.Auth.Server.ViewModels.Authorization;

public class AuthorizeViewModel
{
    [Display(Name = "Application")]
    public string ApplicationName { get; set; }

    [Display(Name = "Scope")]
    public string Scope { get; set; }
}
