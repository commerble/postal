# 手順

## 郵政CSVデータ(KEN_ALL.csv)をダウンロード
http://www.post.japanpost.jp/zipcode/dl/kogaki-zip.html  
の[全国一括]をダウンロードする。

## csv ファイルの生成
```
> pconv -i KEN_ALL.csv -t {変換テンプレートファイル}
```
テンプレート実行した結果を標準出力。
ファイル出力したかったらリダイレクト( > {出力ファイル名})。

※ コマンドプロンプト(cmd.exe)で実行しましょう。

## BCPでデータベースに登録する（カスタム）
```
delete from zipCodeAddr  
delete from zipCode  
DBCC CHECKIDENT ('zipCodeAddr', RESEED, 0);  
```

```
> bcp zipCode in zc.csv -f zc.fmt /S {server} /U {user} /P {password} /d {database}
> bcp zipCodeAddr in zca.csv -f zca.fmt /S {server} /U {user} /P {password} /d {database}
```

※ Windows認証なら/U /P 指定じゃなく/T  
※ zipCodeAddrテーブルのstreet列 初期値に''を指定しておく。null許すまじ、設定だけどbcpで判断できなそうだから。
