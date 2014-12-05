using Autofac;
using System;
using System.Xml.Linq;
using TeaCommerce.Api;
using TeaCommerce.Api.Dependency;
using umbraco.MacroEngines;
using umbraco.interfaces;
using DynamicXml = Umbraco.Core.Dynamics.DynamicXml;

namespace TeaCommerce.Umbraco.Configuration.InformationExtractors {
  public class DynamicNodeProductInformationExtractor : IDynamicNodeProductInformationExtractor {

    public static IDynamicNodeProductInformationExtractor Instance { get { return DependencyContainer.Instance.Resolve<IDynamicNodeProductInformationExtractor>(); } }

    public virtual string GetPropertyValue( DynamicNode model, string propertyAlias, Func<DynamicNode, bool> func = null ) {
      string rtnValue = "";

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        //Check if this node or ancestor has it
        DynamicNode currentNode = func != null ? model.AncestorOrSelf( func ) : model;
        if ( currentNode != null ) {
          rtnValue = GetPropertyValueInternal( currentNode, propertyAlias, func == null );
        }

        //Check if we found the value
        if ( string.IsNullOrEmpty( rtnValue ) ) {

          //Check if we can find a master relation
          string masterRelationNodeId = GetPropertyValueInternal( model, Constants.ProductPropertyAliases.MasterRelationPropertyAlias, true );
          if ( !string.IsNullOrEmpty( masterRelationNodeId ) ) {
            rtnValue = GetPropertyValue( new DynamicNode( masterRelationNodeId ), propertyAlias, func );
          }

        }
      }

      return rtnValue;
    }

    protected virtual string GetPropertyValueInternal( DynamicNode model, string propertyAlias, bool recursive ) {
      string strValue = "";

      if ( model != null && !string.IsNullOrEmpty( propertyAlias ) ) {
        IProperty property = null;

        if ( !recursive ) {
          property = model.GetProperty( propertyAlias );
        } else {
          DynamicNode tempModel = model;
          IProperty tempProperty = tempModel.GetProperty( propertyAlias );
          if ( tempProperty != null && !string.IsNullOrEmpty( tempProperty.Value ) ) {
            property = tempProperty;
          }

          while ( property == null && tempModel != null && tempModel.Id > 0 ) {
            tempModel = tempModel.Parent;
            if ( tempModel != null ) {
              tempProperty = tempModel.GetProperty( propertyAlias );
              if ( tempProperty != null && !string.IsNullOrEmpty( tempProperty.Value ) ) {
                property = tempProperty;
              }
            }
          }
        }

        if ( property != null ) {
          strValue = property.Value;
        }
      }

      return strValue;
    }

    public virtual DynamicXml GetXmlPropertyValue( DynamicNode model, string propertyAlias, Func<DynamicNode, bool> func = null ) {
      DynamicXml xmlNode = null;
      string propertyValue = GetPropertyValue( model, propertyAlias, func );

      if ( !string.IsNullOrEmpty( propertyValue ) ) {
        xmlNode = new DynamicXml( XElement.Parse( propertyValue, LoadOptions.None ) );
      }

      return xmlNode;
    }
  }
}
