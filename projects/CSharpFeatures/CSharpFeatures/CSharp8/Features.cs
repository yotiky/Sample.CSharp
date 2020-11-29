using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFeatures.CSharp8
{
    public partial class Features
    {
        #region null許容参照型

        public static void null許容参照型()
        {
            // 機能
            {
                // C# 7.3 以前は警告が出ない
                string s = null;
                Debug.WriteLine(s.Length);
            }

#nullable enable
            {
                // C# 8.0 null 許容参照型を有効にした場合
                // null 非許容参照型に null を入れると警告が出る （宣言時に出る警告は annotations と呼ばれる
                // CS8600  Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
                string s1 = null;
                string s2 = "";
                string? s3 = null;

                // null 非許容参照型に null を入れていると警告が出る (使用時に出る警告は warnings と呼ばれる
                // CS8602  null 参照の可能性があるものの逆参照です。
                Debug.WriteLine(s1.Length);

                // nullable でない型に null を入れてなければ警告は出ない
                Debug.WriteLine(s2.Length);

                // nullable の型を null チェックせずに使うと警告が出る
                if (s3 != null)
                {
                    // ここでは null じゃないことが確定するので警告が出ない
                    Debug.WriteLine(s3.Length);
                }
                // ここでは null の可能性がある
                // CS8602  null 参照の可能性があるものの逆参照です。
                Debug.WriteLine(s3.Length);
                // 前行が通れば null じゃないことが確定するので警告が出ない
                Debug.WriteLine(s3.Length);

            }
#nullable disable

            // 有効化/無効化
            // a. csproj ファイルにオプションを指定する
            //    プロジェクト全体で適用される

            // b. #nullable enable|disable|restore [warnings|annotations]
            //    有効にしたコードだけ適用される
            {
#nullable enable
                // [warnings|annotations] を指定しなければ両方に対して
                // 有効化されているので警告が出る
                string s1 = null;
                Debug.WriteLine(s1.Length);
#nullable disable
                // 無効化されているので警告が出ない
                string s2 = null;
                Debug.WriteLine(s2.Length);
#nullable enable
#nullable restore
                // restore はプロジェクトの設定に戻す
                string s3 = null;
                Debug.WriteLine(s3.Length);


                // null 非許容参照型が無効の場合警告が出る
                // CS8632  '#nullable' 注釈コンテキスト内のコードでのみ、Null 許容参照型の注釈を使用する必要があります。
                string? s4 = null;
                Debug.WriteLine(s4.Length);

#nullable enable warnings
                // warnings を有効にすると変数参照時の警告が出る
                // annotations は無効のままなので nullable の変数宣言時の警告が出る
                string s5 = null;
                string? s6 = null;
                Debug.WriteLine(s5.Length);
                Debug.WriteLine(s6.Length);
#nullable disable warnings

#nullable enable annotations
                // annotations を有効にすると nullable の変数宣言時に警告が出ない
                // warnings は無効のままなので変数参照時の警告は出ない
                string s7 = null;
                string? s8 = null;
                Debug.WriteLine(s7.Length);
                Debug.WriteLine(s8.Length);
#nullable disable annotations
            }

#nullable enable
            // !(null 免除)演算子
            {
                // null 非許容参照型に null を入れると警告が出る
                string s1 = null;
                // null 非許容参照型に ! 演算子を使うと一時的に null を許可することができる
                string s2 = null!;
                string? s3 = null;

                Debug.WriteLine(s1.Length);
                Debug.WriteLine(s2.Length);
                // null 許容参照型に ! 演算子を使うと許容性を無視するので警告が出ない
                Debug.WriteLine(s3!.Length);
            }
#nullable disable
        }

#nullable enable
        // null 非許容参照型をフィールドやプロパティで使う場合は、宣言時かコンストラクタで初期化しないと警告が出る
        public string X { get; set; } = "";
        // null 非許容参照型に一時的に null を許可する場合は !演算子 が使える
        public string Y = null!;


        // null 許容値型との違い
        private void NullableValueType(DateTime? x)
        {
            // null チェックしても Nullable<T> なのでエラー
            if (x is null) { return; }
            // CS1061  'DateTime?' に 'Minute' の定義が含まれておらず、型 'DateTime?' の最初の引数を受け付けるアクセス可能な拡張メソッド 'Minute' が見つかりませんでした。using ディレクティブまたはアセンブリ参照が不足していないことを確認してください
            //Debug.WriteLine(x.Minute);
        }
        private void NullableReferenceType(string? x)
        {
            // null チェックすれば警告が出ない
            if (x is null) { return; }
            Debug.WriteLine(x.Length);

            // 内部的に同じ型なので typeof は使えない
            // CS8639  NULL 許容参照型では typeof 演算子を使用できません
            //var t = typeof(string?);
        }

        private void NullableValueType(DateTime x) { }
        // null 許容値型は型が違うのでオーバーロードできるが null 許容参照型は型が同じなのでオーバーロードできない
        //private void NullableReferenceType(string x) { }　// エラー
#nullable disable

        #endregion
    }
}
