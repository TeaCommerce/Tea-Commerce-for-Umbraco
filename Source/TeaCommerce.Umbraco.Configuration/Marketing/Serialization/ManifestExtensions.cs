using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Umbraco.Configuration.Marketing.Models;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Serialization {
  public static class ManifestExtensions {

    #region json

    public static string ToJson( this Manifest manifest ) {
      string rtnStr = "null";

      if ( manifest != null ) {

        StringBuilder json = new StringBuilder();

        json.Append( "{" );
        json.Append( SerializeHelper.CreateJsonProperty( "name", manifest.Name, false ) );
        json.Append( SerializeHelper.CreateJsonProperty( "alias", manifest.Alias ) );
        json.Append( SerializeHelper.CreateJsonProperty( "editor", manifest.Editor.ToJson(), isObject: true ) );
        json.Append( SerializeHelper.CreateJsonProperty( "javascripts", manifest.JavaScripts.ToJson(), isObject: true ) );
        json.Append( "}" );

        rtnStr = json.ToString();

      }
      return rtnStr;
    }

    public static string ToJson( this IEnumerable<Manifest> manifests ) {
      string rtnStr = "null";

      if ( manifests != null ) {

        StringBuilder json = new StringBuilder();

        json.Append( "[" );
        json.Append( string.Join( ",", manifests.Select( i => i.ToJson() ) ) );
        json.Append( "]" );

        rtnStr = json.ToString();

      }

      return rtnStr;
    }

    #endregion

    #region xml

    public static XElement ToXml( this Manifest manifest ) {
      XElement xEle = new XElement( "manifest" );

      if ( manifest != null ) {
        xEle.Add( SerializeHelper.CreateXmlAttribute( "name", manifest.Name ) );
        xEle.Add( SerializeHelper.CreateXmlAttribute( "alias", manifest.Alias ) );
        xEle.Add( manifest.Editor.ToXml() );
        xEle.Add( manifest.JavaScripts.ToXml( "javascripts", "javascript" ) );
      }

      return xEle;
    }

    public static XElement ToXml( this IEnumerable<Manifest> manifests ) {
      XElement xEle = new XElement( "manifests" );

      if ( manifests != null ) {
        xEle.Add( manifests.Select( i => i.ToXml() ) );
      }

      return xEle;
    }

    #endregion

  }
}
