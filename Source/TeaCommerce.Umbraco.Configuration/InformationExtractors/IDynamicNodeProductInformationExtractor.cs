using System;
using umbraco.MacroEngines;
using DynamicXml = Umbraco.Core.Dynamics.DynamicXml;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public interface IDynamicNodeProductInformationExtractor {
    string GetPropertyValue( DynamicNode model, string propertyAlias, Func<DynamicNode, bool> func = null );
    DynamicXml GetXmlPropertyValue( DynamicNode model, string propertyAlias, Func<DynamicNode, bool> func = null );
  }
}
