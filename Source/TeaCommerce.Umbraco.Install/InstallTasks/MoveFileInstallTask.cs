using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class MoveFileInstallTask : IInstallTask {

    public string TargetFilePath { get; set; }
    public string SourceFilePath { get; set; }
    public bool OverwriteFile { get; set; }
    public bool EmbeddedResource { get; set; }

    public MoveFileInstallTask( string sourceFile, string targetFile ) {
      OverwriteFile = true;
      SourceFilePath = sourceFile;
      TargetFilePath = targetFile;
    }

    public void Install() {
      SourceFilePath = !EmbeddedResource ? HostingEnvironment.MapPath( SourceFilePath ) : SourceFilePath;
      TargetFilePath = HostingEnvironment.MapPath( TargetFilePath );

      if ( OverwriteFile && File.Exists( TargetFilePath ) )
        File.Delete( TargetFilePath );

      if ( OverwriteFile || !File.Exists( TargetFilePath ) ) {
        if ( !EmbeddedResource ) {
          File.Move( SourceFilePath, TargetFilePath );
          File.Delete( SourceFilePath );
        } else {
          Directory.CreateDirectory( Path.GetDirectoryName( TargetFilePath ) );
          using ( Stream input = Assembly.GetExecutingAssembly().GetManifestResourceStream( SourceFilePath ) ) {
            using ( FileStream output = new FileStream( TargetFilePath, FileMode.OpenOrCreate ) ) {
              input.CopyTo( output );
            }
          }
        }
      }
    }

    public void Uninstall() {
      File.Delete( HostingEnvironment.MapPath( TargetFilePath ) );
    }
  }
}
