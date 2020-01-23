
~~~
> npm install -D packageName //ローカルへインストール
> npm install -g packageName //グローバルへインストール
~~~

# gulpのインストール
~~~
> npm install -D gulp
~~~

# taskの作成
### sassファイルコンパイラーをインストール
~~~
> npm install -D gulp-sass
~~~

### gulpfile.jsの作成
~~~ js
// gulpプラグインの読み込み
const gulp = require("gulp");
// Sassをコンパイルするプラグインの読み込み
const sass = require("gulp-sass");

// style.scssをタスクを作成する
gulp.task("default", function() {
  // style.scssファイルを取得
  return (
    gulp
      .src("css/style.scss")
      // Sassのコンパイルを実行
      .pipe(sass())
      // cssフォルダー以下に保存
      .pipe(gulp.dest("css"))
  );
});
~~~

### cssフォルダと、style.scssの作成
~~~ scss
div {
  p {
    font-weight: bold;
  }
}

// 変数のテスト
$fontColor: #525252;

h1 {
  color: $fontColor;
}
~~~

### タスクの実行
~~~
> npx gulp
//cssフォルダの直下にstyle.cssができる
~~~
- タスク名の変更
  - defulat以外を指定した場合は、`npx gulp taskName` で実行できる

### オプション指定
~~~ js
// Sassのコンパイルを実行
.pipe(sass({
  outputStyle: "expanded" //追加　閉じ } を改行してくれる
}))
~~~

# watch機能の追加

### retrunをgulp.watch()で囲う
~~~ js
//style.scssファイルを監視
return gulp.watch("css/style.scss", function() {
  return (
    //処理
  );
});
~~~

### 監視開始
~~~
> npx gulp
//style.scssの変更がstyle.cssに随時反映される
~~~

### task()は非推奨
- 2019/5で非推奨になっている
  - 推奨されるのは、taskごとに関数を作って、coomonJsのmodule機能を用いるやり方

~~~
> 
~~~