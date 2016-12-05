using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace TeaCommerce.Umbraco.Configuration.Variants.Models {
  public class VariantPublishedContent : PublishedContentBase, IVariant {
    private readonly string _variantIdentifier;
    private readonly List<IPublishedProperty> _properties;
    private readonly PublishedContentType _contentType;
    private readonly List<Specification> _combination;

    public VariantPublishedContent( Variant variant, PublishedContentType contentType ) {
      _variantIdentifier = variant.Id;
      _contentType = contentType;

      _combination = variant.Combination;
      Validation = variant.Validation;

      _properties = new List<IPublishedProperty>();

      foreach ( string key in variant.Properties.Keys ) {
        _properties.Add( new VariantPublishedProperty( contentType.GetPropertyType( key ), variant.Properties[key] ) );
      }
    }

    public string VariantIdentifier {
      get { return _variantIdentifier; }
    }

    public List<Specification> Combination {
      get { return _combination; }
    }

    public VariantValidation Validation { get; private set; }

    public override IEnumerable<IPublishedContent> Children {
      get { return Enumerable.Empty<IPublishedContent>(); }
    }

    public override PublishedContentType ContentType {
      get { return _contentType; }
    }

    public override DateTime CreateDate {
      get { return DateTime.MinValue; }
    }

    public override int CreatorId {
      get { return 0; }
    }

    public override string CreatorName {
      get { return ""; }
    }

    public override string DocumentTypeAlias {
      get { return _contentType.Alias; }
    }

    public override int DocumentTypeId {
      get { return _contentType.Id; }
    }

    public override IPublishedProperty GetProperty( string alias ) {
      return _properties.FirstOrDefault( x => x.PropertyTypeAlias.InvariantEquals( alias ) );
    }

    public override IPublishedProperty GetProperty( string alias, bool recurse ) {
      if ( recurse ) {
        throw new NotSupportedException();
      }

      return GetProperty( alias );
    }

    public override int Id {
      get { return 0; }
    }

    public override bool IsDraft {
      get { return false; }
    }

    public override PublishedItemType ItemType {
      get { return PublishedItemType.Content; }
    }

    public override int Level {
      get { return 0; }
    }

    public override string Name {
      get { return String.Join( " - ", _combination.Select( c => c.Name ) ); }
    }

    public override IPublishedContent Parent {
      get { return null; }
    }

    public override string Path {
      get { return ""; }
    }

    public override ICollection<IPublishedProperty> Properties {
      get { return _properties.ToArray(); }
    }

    public override int SortOrder {
      get { return 0; }
    }

    public override int TemplateId {
      get { return 0; }
    }

    public override DateTime UpdateDate {
      get { return DateTime.MinValue; }
    }

    public override string UrlName {
      get { return ""; }
    }

    public override string Url {
      get { return ""; }
    }

    public override Guid Version {
      get { return Guid.Empty; }
    }

    public override int WriterId {
      get { return 0; }
    }

    public override string WriterName {
      get { return ""; }
    }

  }
}
