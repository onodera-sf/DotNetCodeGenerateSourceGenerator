using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeGenerator;

[Generator(LanguageNames.CSharp)]
public partial class Sample2Generator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    // 構文を解析し処理対象のノードをコード出力用に変換します
    var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(
      // すべてのノードが処理対象となるため、predicate で処理対象とするノードを限定します
      predicate: static (node, cancelToken) =>
      {
        // 処理が中断される場面は多々あるのでいつでもキャンセルできるようにしておく
        cancelToken.ThrowIfCancellationRequested();

        // クラスのノードのみを対象とする (record とかつけると別な種類のノードになるので注意)
        // また partial class であること
        return node is ClassDeclarationSyntax nodeClass && nodeClass.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
      },
      // 対象のノードをコード出力に必要な形に変換します
      transform: static (contextSyntax, cancelToken) =>
      {
        // いつでもキャンセルできるようにしておく
        cancelToken.ThrowIfCancellationRequested();

        // 今回は ClassDeclarationSyntax 確定なのでそのままキャストします
        ClassDeclarationSyntax classDecl = (ClassDeclarationSyntax)contextSyntax.Node;

        // データと構文から宣言されたシンボルを取得します
        // 今回利用場面がほとんどないですが取得しておくといろんな情報を取得できるので便利です
        var symbol = Microsoft.CodeAnalysis.CSharp.CSharpExtensions.GetDeclaredSymbol(contextSyntax.SemanticModel, classDecl, cancelToken)!;

        // 出力に必要な値を返します。大抵の型を渡すことができます
        return (symbol, classDecl);
      }
    );

    // 解析し変換した情報をもとにコードを出力します
    context.RegisterSourceOutput(syntaxProvider, static (contextSource, input) =>
    {
      // いつでもキャンセルできるようにしておく
      CancellationToken cancelToken = contextSource.CancellationToken;
      cancelToken.ThrowIfCancellationRequested();

      // 解析後に渡された値を受け取ります
      var (symbol, classDecl) = input;

      StringBuilder sbInitValues = new();

      // プロパティを列挙して値を初期化するコードを生成
      foreach (var syntax in classDecl.Members.OfType<PropertyDeclarationSyntax>())
      {
        sbInitValues.AppendLine($"{syntax.Identifier.ValueText} = default;");
      }

      // 出力するコードを作成
      var source = @$"
          {string.Join(" ", classDecl.Modifiers.Select(x => x.ToString()))} class {symbol.Name}
          {{
            public void Reset()
            {{
              {sbInitValues}
            }}
          }}";

      // ファイル名はクラス名に合わせておきます。作成したコードを出力します
      contextSource.AddSource($"{symbol.Name}.g.cs", source);
    });
  }
}
