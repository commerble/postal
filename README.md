# 郵便番号データ正規化

[郵便番号データダウンロード \- 日本郵便](http://www.post.japanpost.jp/zipcode/download.html)

* カッコ内カンマ区切りデータの展開
* 丁目・番地削除
* 固定文字列削除("以下に掲載がない場合"など)

## pconv

pconv -i KEN_ALL.csv -t {変換テンプレートファイル}

* テンプレートはRazor(cs)
* テンプレート無指定ならテキストを標準出力
* テンプレート指定なら実行した結果を標準出力
* ファイル出力したかったらリダイレクトで保存

## サンプルテンプレート

csv.cshtml
`
    @foreach(var p in Model){
    <text>"@p.Code","@p.Prefecture","@p.City","@p.Street"</text>
}
`
