namespace PetsApi.ViewModels
{
  public class DocumentsRequestedViewModel : EmailBaseViewModel
  {
    public long SupplierId { get; set; }
    
    public DocumentsRequestedViewModel(long supplierId)
    {
      SupplierId = supplierId;
    }
  }
}
