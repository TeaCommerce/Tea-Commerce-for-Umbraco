namespace TeaCommerce.Umbraco.Configuration.Services {
  public interface ILanguageService {
    long? GetLanguageIdByNodePath( string nodePath );
  }
}