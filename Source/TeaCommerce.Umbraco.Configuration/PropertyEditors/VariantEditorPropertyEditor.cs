using System.Collections.Generic;
using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace TeaCommerce.Umbraco.Configuration.PropertyEditors {
  
  [PropertyEditorAsset( ClientDependencyType.Javascript, "/App_Plugins/TeaCommerce/PropertyEditors/js.cookie.js", Priority = 2 )]
  [PropertyEditorAsset( ClientDependencyType.Css, "/App_Plugins/TeaCommerce/PropertyEditors/variant-editor.css", Priority = 2 )]
  [PropertyEditorAsset( ClientDependencyType.Javascript, "/App_Plugins/TeaCommerce/PropertyEditors/variant-editor.controller.js", Priority = 2 )]
  [PropertyEditorAsset( ClientDependencyType.Javascript, "/App_Plugins/TeaCommerce/PropertyEditors/variant-editor-node-type.js", Priority = 2 )]
  [PropertyEditor( "TeaCommerce.VariantEditor", "Tea Commerce: Variant Editor", "/App_Plugins/TeaCommerce/PropertyEditors/variant-editor.html", ValueType = "JSON" )]
  public class VariantEditorPropertyEditor : PropertyEditor {
    private IDictionary<string, object> _defaultPreValues;
    public override IDictionary<string, object> DefaultPreValues
    {
      get { return _defaultPreValues; }
      set { _defaultPreValues = value; }
    }

    public VariantEditorPropertyEditor() {
      // Setup default values
      this.
      _defaultPreValues = new Dictionary<string, object>
      {
        {"languageSource", "installed"},
        {"mandatoryBehaviour", "ignore"}
      };
    }

    #region Pre Value Editor

    protected override PreValueEditor CreatePreValueEditor() {
      return new VariantPreValueEditor();
    }

    internal class VariantPreValueEditor : PreValueEditor {

      [PreValueField( "xpathOrNode", "Variant Attribute Groups XPath Or Node", "/App_Plugins/TeaCommerce/PropertyEditors/variant-editor-node-type.html", Description = "Enter an XPath statement to locate node containing all attribute groups or select the node." )]
      public string XPathOrNode { get; set; }
      
      [PreValueField( "variantDocumentType", "Variant document type", "textstring", Description = "[Optional] A document type that makes it possible to enrich each variant combination with extra information." )]
      public string VariantDocumentType { get; set; }

      [PreValueField( "extraListInformation", "Extra list information", "textstring", Description = "[Optional] Comaseparated list of property aliases from the " )]
      public string ExtraListInformation { get; set; }

      [PreValueField( "variantGroupDocumentTypes", "Variant group document types", "textstring", Description = "[Optional] Commaseparated list of the variant group document types that can be chosen from. e.g. \"Color,Size\"." )]
      public string VariantGroupDocumentTypes { get; set; }

      [PreValueField( "variantGroupNodeName", "Variant group name", "textstring", Description = "[Optional] Commaseparated list of the variant group node names that can be chosen from. e.g. \"Color,Size\"." )]
      public string VariantGroupNodeName { get; set; }

      [PreValueField( "forceEditorToChooseAllVariantGroups", "Force editor to choose all variant groups", "boolean", Description = "Editor will be forced to choose at least one variant type from each variant group when creating variants." )]
      public bool ForceEditorToChooseAllVariantGroups { get; set; }

      [PreValueField( "hideLabel", "Hide Label", "boolean", Description = "Hide the Umbraco property title and description, making the Variant Editor span the entire page width" )]
      public bool HideLabel { get; set; }
    }

    #endregion

  }
}
