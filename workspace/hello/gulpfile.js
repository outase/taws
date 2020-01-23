// gulpプラグインの読み込み
const { src, dest, watch } = require("gulp");
// Sassをコンパイルするプラグインの読み込み
const sass = require("gulp-sass");

// Sassをコンパイルするタスク
const compileSass = () =>
  // scssファイル取得
  src("css/*.scss")
    // Sassコンパイルを実行
    .pipe(
      sass({
        outputStyle: "expanded"
      })
    )
    // cssフォルダ以下に保存
    .pipe(dest("css"))

// sassファイルを監視する
const watchSassFile = () => watch("css/*.scss", compileSass);

// npx gulpコマンドを実行したら、watchSassFileを実行
exports.default = watchSassFile;