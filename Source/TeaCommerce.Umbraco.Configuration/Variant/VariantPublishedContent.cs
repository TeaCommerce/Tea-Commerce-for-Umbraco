using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Web.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using TeaCommerce.Umbraco.Configuration.Variant.Product;

namespace TeaCommerce.Umbraco.Configuration.Variant {
  public class VariantPublishedContent : PublishedContentBase {
    private readonly bool _isPreviewing;
    private readonly string _variantId;
    private readonly IPublishedContent _parentContent;
    private readonly List<IPublishedProperty> _properties;
    private readonly PublishedContentType _contentType;
    private readonly List<Combination> _combinations;

    public VariantPublishedContent( Product.Variant variant, PublishedContentType contentType, IPublishedContent parentContent = null, bool isPreviewing = false ) {
      _parentContent = parentContent;
      _isPreviewing = isPreviewing;
      _contentType = contentType;
      _variantId = variant.Id;

      _combinations = variant.Combination;
      Validation = variant.Validation;

      _properties = new List<IPublishedProperty>();

      foreach ( JProperty property in variant.Properties ) {
        _properties.Add( new VariantPublishedProperty( contentType.GetPropertyType( property.Name ), property.Value ) );
      }
    }

    public string VariantId {
      get { return _variantId; }
    }

    public string ProductIdentifier {
      get { return _parentContent.Id + "_" + _variantId; }
    }

    public List<Combination> Combinations {
      get { return _combinations; }
    }

    public VariantValidation Validation { get; private set; }

    public override IEnumerable<IPublishedContent> Children {
      get { return Enumerable.Empty<IPublishedContent>(); }
    }

    public override PublishedContentType ContentType {
      get { return _contentType; }
    }

    public override DateTime CreateDate {
      get { return _parentContent != null ? _parentContent.CreateDate : DateTime.MinValue; }
    }

    public override int CreatorId {
      get { return _parentContent != null ? _parentContent.CreatorId : 0; }
    }

    public override string CreatorName {
      get { return _parentContent != null ? _parentContent.CreatorName : ""; }
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
      get { return _isPreviewing; }
    }

    public override PublishedItemType ItemType {
      get { return PublishedItemType.Content; }
    }

    public override int Level {
      get { return 0; }
    }

    public override string Name {
      get { return String.Join( " - ", _combinations.Select( c => c.Name ) ); }
    }

    public override IPublishedContent Parent {
      get { return _parentContent; }
    }

    public override string Path {
      get { return _parentContent != null ? _parentContent.Path : ""; }
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
      get { return _parentContent != null ? _parentContent.UpdateDate : DateTime.MinValue; }
    }

    public override string UrlName {
      get { return _parentContent != null ? _parentContent.UrlName : ""; }
    }

    public override string Url {
      get { return _parentContent != null ? _parentContent.Url : ""; }
    }

    public override Guid Version {
      get { return _parentContent != null ? _parentContent.Version : Guid.Empty; }
    }

    public override int WriterId {
      get { return _parentContent != null ? _parentContent.WriterId : 0; }
    }

    public override string WriterName {
      get { return _parentContent != null ? _parentContent.WriterName : ""; }
    }

  }
}
