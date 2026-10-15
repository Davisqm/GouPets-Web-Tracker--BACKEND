namespace PetsApi.ViewModels
{
  public class AccountAuthorizedViewModel : EmailBaseViewModel
  {
    public long SupplierId { get; set; }
    
    public AccountAuthorizedViewModel(long supplierId)
    {
      SupplierId = supplierId;
    }
  }
}
