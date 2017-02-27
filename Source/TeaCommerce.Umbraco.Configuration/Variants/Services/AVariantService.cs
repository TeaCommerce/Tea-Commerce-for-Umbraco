using System.Collections.Generic;
using Autofac;
using System.Linq;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Umbraco.Configuration.Variants.Models;

namespace TeaCommerce.Umbraco.Configuration.Variants.Services {
  public abstract class AVariantService<T, T2> : IVariantService<T, T2> where T2 : IVariant {

    public static IVariantService<T, T2> Instance { get { return DependencyContainer.Instance.Resolve<IVariantService<T, T2>>(); } }

    public T2 GetVariant( long storeId, T product, string variantId, bool onlyValid = true ) {
      IEnumerable<T2> variants = GetVariants( storeId, product, onlyValid );

      return variants.FirstOrDefault( v => v.VariantIdentifier == variantId );
    }

    public abstract IEnumerable<T2> GetVariants( long storeId, T product, bool onlyValid = true );

    public IEnumerable<VariantGroup> GetVariantGroups( IEnumerable<T2> variants ) {
      List<VariantGroup> attributeGroups = new List<VariantGroup>();

      foreach ( T2 variant in variants ) {
        foreach ( Specification combination in variant.Combination ) {
          VariantGroup attributeGroup = attributeGroups.FirstOrDefault( ag => ag.Id == combination.GroupId );

          if ( attributeGroup == null ) {
            attributeGroup = new VariantGroup { Id = combination.GroupId, Name = combination.GroupName };
            attributeGroups.Add( attributeGroup );
          }

          if ( attributeGroup.Attributes.All( a => a.Id != combination.Id ) ) {
            attributeGroup.Attributes.Add( new VariantType { Id = combination.Id, Name = combination.Name } );
          }
        }
      }

      return attributeGroups;
    }

    public abstract string GetVariantJson( long storeId, IEnumerable<T> productContents, bool onlyValid = true );
  }
}
