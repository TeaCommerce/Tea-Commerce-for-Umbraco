
using System;
namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public abstract class ADeleteTask : ATask {

    protected string[] idTokens;

    public override bool Delete() {
      idTokens = Alias.Split( new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries );
      return Delete( long.Parse( idTokens[ 1 ] ), long.Parse( idTokens[ 2 ] ) );
    }

    public abstract bool Delete( long storeId, long entityId );

  }
}