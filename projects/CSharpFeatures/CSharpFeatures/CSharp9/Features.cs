using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFeatures.CSharp9
{
    #region init 専用セッター

    public record WeatherObservation
    {
        // set の代わりに初期化 init が使える
        public DateTime RecordedAt { get; init; }
        
        public decimal TemperatureInCelsius { get; init; }

        public readonly decimal PressureInMillibars = 1000.0m;
        
        public WeatherObservation()
        {
            // init アクセサーのプロパティはコンストラクタかオブジェクト初期化子でのみ値を設定できる
            TemperatureInCelsius = 10;
            // readonly なフィールドはフィールド初期化子かコンストラクタで初期化する
            PressureInMillibars = 1000.0m;
        }
    }

    public partial class Features
    {
        public static void Init専用セッター()
        {
            var now = new WeatherObservation
            {
                RecordedAt = DateTime.Now,
                TemperatureInCelsius = 20,
                // readonly なフィールドにはオブジェクト初期化子では書き込みできない
                //PressureInMillibars = 998.0m,
            };

            // 初期化後は値を変更できない
            //now.RecordedAt = DateTime.MaxValue;
        }
    }

    #endregion

    public partial class Features
    {
        #region Records

        // レコードはデフォルトでは immutable なクラスに展開される
        public record Person
        {
            public string LastName { get; }
            public string FirstName { get; }

            public Person(string first, string last) 
                => (FirstName, LastName) = (first, last);
        }

        // レコードは継承することも、sealed にすることも可能
        public record Teacher : Person
        {
            public string Subject { get; }

            public Teacher(string first, string last, string sub) : base(first, last)
                => Subject = sub;
        }

        // Positional records / 位置指定の初期化子 と呼ばれる完結な形式で定義できる
        public record Pet(string Name, string Type);

        public static void Records()
        {
            // レコードはデフォルトでは immutable
            // セッターがないので値を設定できない
            var p1 = new Person("Taro", "Saito");
            //p1.FirstName = "Ichiro";

            // 比較は同じインスタンスかではなく、プロパティの値が一致するか
            var p2 = new Person("Taro", "Saito");
            Debug.WriteLine($"p1==p2 : {p1 == p2}");
            //【出力】p1 == p2 : True

            // ToString() は独自に加工されている
            Debug.WriteLine(p1.ToString());
            //【出力】Person { LastName = Saito, FirstName = Taro }

            // Positional records で定義すると init 専用プロパティとなるので値を設定できない
            // init 専用プロパティまたはインデクサー 'Features.Pet.Name' を割り当てることができるのは、オブジェクト初期化子の中か、インスタンス コンストラクターまたは 'init' アクセサーの 'this' か 'base' 上のみです。
            var cat = new Pet("Pochi", "Cat");
            //pet.Name = "Tama";

            // with を使うと値を上書きしたコピーを作成できる
            var cat2 = cat with { Name = "Tama" };
            Debug.WriteLine(cat2.ToString());
            //【出力】Pet { Name = Tama, Type = Cat }

            // プロパティの分解もできる
            var (name, type) = cat2;
            Debug.WriteLine($"{type}の{name}。");
            //【出力】CatのTama。
        }

        #endregion 

        #region ターゲットからの new 型推論

        // `new 型名()` の型名を省略できる
        // フィールドやメソッドの引数などで使えるが、var は推論できないので使えない
        Dictionary<string, string> _dic = new();

        Dictionary<string, List<(int x, int y)>> _cache = new()
        {
            { "A", new() { (1, 1), (2, 2) } },
            { "B", new() { (1, 1), (2, 2) } },
        };

        public static void ターゲットからのnew型推論()
        {
            // メソッドの引数の型がわかってるので使える
            ReturnNew(new());

            // これも型がわかるので使える
            (int X, int Y) p = new(1, 2);

            // Exception が飛ぶ
            throw new("This is Exception.");
        }
        private static List<int> ReturnNew(List<int> list)
        {
            // 戻り値の型がわかってるので使える
            return new();
        }

        #endregion

        #region unsafe/ネイティブ相互運用向け機能

        // 省略

        #endregion

        #region ラムダ式の引数を破棄

        public static void ラムダ式の引数を破棄()
        {
            Action<int, int> action = (_, _) =>
            {
                // '_' は、現在のコンテキストに存在しません。 エラー
                //Console.WriteLine(_);
            };

            Action<int, int> action2 = (_, _1) =>
            {
                // 引数 _ が1個だけの場合は破棄されない
                Console.WriteLine(_);
            };
        }

        #endregion

        #region ローカル関数への属性適用

        public static void ローカル関数への属性適用()
        {
            // ローカル関数に属性を付けれる
            [return: NotNullIfNotNull("s")]
            string? toLower(string? s) => s?.ToLower();

        }

        #endregion

        #region パターンマッチングの拡張機能

        public static void パターンマッチングの拡張機能()
        {
            var x = 80;

            // 先頭の変数に対して条件だけを書ける
            // 変数が長かったりプロパティの深い入れ子の場合に有用そう
            if (x is (>= 0 and < 10) or (> 80 and <= 100)) { }

            // == ではなく直接値を指定する
            if (x is 80 or > 80) { }

            // 関数は1回の実行だけで済む
            Func<int> func = () => 5;
            if (func() is > 0 and < 10) { }

            string s = null;

            // s != null と同じ
            if (s is not null) { }
        }

        #endregion
    }
}
