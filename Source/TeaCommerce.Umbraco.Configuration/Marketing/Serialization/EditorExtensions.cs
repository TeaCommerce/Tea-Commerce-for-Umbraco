using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Umbraco.Configuration.Marketing.Models;

namespace TeaCommerce.Umbraco.Configuration.Marketing.Serialization {
  public static class EditorExtensions {

    #region json

    public static string ToJson( this Editor editor ) {
      string rtnStr = "null";

      if ( editor != null ) {

        StringBuilder json = new StringBuilder();

        json.Append( "{" );
        json.Append( SerializeHelper.CreateJsonProperty( "view", editor.View, false ) );
        json.Append( "}" );

        rtnStr = json.ToString();

      }
      return rtnStr;
    }

    public static string ToJson( this IEnumerable<Editor> editors ) {
      string rtnStr = "null";

      if ( editors != null ) {

        StringBuilder json = new StringBuilder();

        json.Append( "[" );
        json.Append( string.Join( ",", editors.Select( i => i.ToJson() ) ) );
        json.Append( "]" );

        rtnStr = json.ToString();

      }

      return rtnStr;
    }

    #endregion

    #region xml

    public static XElement ToXml( this Editor editor ) {
      XElement xEle = new XElement( "editor" );

      if ( editor != null ) {
        xEle.Add( SerializeHelper.CreateXmlAttribute( "view", editor.View ) );
      }

      return xEle;
    }

    public static XElement ToXml( this IEnumerable<Editor> editors ) {
      XElement xEle = new XElement( "editors" );

      if ( editors != null ) {
        xEle.Add( editors.Select( i => i.ToXml() ) );
      }

      return xEle;
    }

    #endregion

  }
}
