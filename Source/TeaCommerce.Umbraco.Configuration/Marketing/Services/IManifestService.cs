using System.Collections.Generic;
using TeaCommerce.Umbraco.Configuration.Marketing.Models;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Services {
  public interface IManifestService {
    IEnumerable<Manifest> GetAllForRules();
    IEnumerable<Manifest> GetAllForAwards();
  }
}