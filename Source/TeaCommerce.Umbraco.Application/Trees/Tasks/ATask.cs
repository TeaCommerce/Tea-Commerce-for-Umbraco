using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Application.Trees.Tasks {
  public abstract class ATask : ITask {

    public string Alias { get; set; }
    public int ParentID { get; set; }
    public int TypeID { get; set; }
    public int UserId { get; set; }

    public virtual bool Delete() { return true; }
    public virtual bool Save() { return true; }

  }
}