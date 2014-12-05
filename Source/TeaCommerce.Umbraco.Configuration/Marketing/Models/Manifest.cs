using System.Collections.Generic;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Models {
  public class Manifest {
    public string Name { get; set; }
    public string Alias { get; set; }
    public Editor Editor { get; set; }
    public List<string> JavaScripts { get; set; }

    public Manifest() {
      JavaScripts = new List<string>();
    }

  }
}