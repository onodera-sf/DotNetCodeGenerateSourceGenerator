using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace CodeGenerator;

[Generator(LanguageNames.CSharp)]
public partial class SampleGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    // コードを解析せずそのままコードを出力するならこれを使用します
    context.RegisterPostInitializationOutput(static postInitializationContext =>
    {
      // コード生成処理が中断されることもあるので CancellationToken 処理は入れたほうがいいです
      System.Threading.CancellationToken token = postInitializationContext.CancellationToken;
      token.ThrowIfCancellationRequested();

      // 出力するコードを作ります
      var source = """
internal static class SampleClass
{
  public static void Hello() => Console.WriteLine("Hello Source Generator!!");
}
""";
      
      // コードファイルに出力します (実際に目に見えるどこかのフォルダに出力されるわけではありません)
      postInitializationContext.AddSource("SampleGeneratedFile.cs", SourceText.From(source, Encoding.UTF8));
    });
  }
}
