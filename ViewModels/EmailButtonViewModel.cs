namespace PetsApi.ViewModels
{
  public class EmailButtonViewModel
  {
    public string Text { get; set; }
    public string Url { get; set; }
    
    public EmailButtonViewModel(string text, string baseUrl, string routeUrl)
    {
      Text = text;
      Url = baseUrl + "#/" + routeUrl;
    }
  }
}
