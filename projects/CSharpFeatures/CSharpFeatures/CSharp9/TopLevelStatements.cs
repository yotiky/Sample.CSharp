#region トップレベルステートメント

using System;
using System.Threading.Tasks;
using CSharpFeatures.CSharp9;

// エントリーポイントとして展開される、だからプロジェクトで1ファイルのみ
// 名前空間やクラスよりも上に書く必要がある
Console.WriteLine("Hello world!");

// 暗黙的に引数 args を使える
Console.WriteLine(args.Length);

// メソッドも定義できる
// ローカル関数に展開される、他からアクセスできない
void m1(string s)
{
    Console.WriteLine(s);
}
void m2(string s) => Console.WriteLine(s);
m1("Hoge");
m2("Fuga");

// async/await も使える
await Task.Delay(1000);

Features.パターンマッチングの拡張機能();

// 暗黙的に戻り値 int を使える
return 0; 

public class TopLevelStatements
{
    public static void Features()
    {
        // このコンテキストでは、トップレベルのステートメントで宣言されたローカル変数またはローカル関数 'm1' を使用することはできません。
        //m1("hogehoge");
    }
}

#endregion