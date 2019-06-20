const autoprefixer = require('autoprefixer'),
      babel        = require('gulp-babel'),
      browsersync  = require('browser-sync').create(),
      concat       = require('gulp-concat'),
      gulp         = require('gulp'),
      imagemin     = require('gulp-imagemin'),
      mozjpeg      = require('imagemin-mozjpeg'),
      newer        = require('gulp-newer'),
      plumber      = require('gulp-plumber'),
      pngquant     = require('imagemin-pngquant'),
      postcss      = require('gulp-postcss'),
      rename       = require('gulp-rename'),
      sass         = require('gulp-sass'),
      uglify       = require('gulp-uglify');

const pathSrc = {
  sass: 'src/sass/',
  js: 'src/js/',
  img: 'src/img/'
};

const pathDist = {
  root: 'dist/',
  css: 'dist/css/',
  js: 'dist/js/',
  img: 'dist/img/'
};

const watchAlso = ['dist/**/*.html', 'dist/img/*'];

function browserSync(done) {
  browsersync.init({
    server: {
      baseDir: pathDist.root
    }
  });
  done();
}

function browserSyncReload(done) {
  browsersync.reload();
  done();
}

function css() {
  const plugins = [
    autoprefixer({ grid: true }),
  ];
  return gulp
    .src(pathSrc.sass + '**/*')
    .pipe(plumber({
      errorHandler: function(err) {
        console.log(err.messageFormatted);
        this.emit('end');
      }
    }))
    .pipe(sass({
      outputStyle: 'compressed'
    }))
    .pipe(postcss(plugins))
    .pipe(gulp.dest(pathDist.css))
    .pipe(browsersync.stream());
}

function scripts() {
  return (
    gulp
      .src(pathSrc.js + '**/*')
      .pipe(plumber())
      .pipe(babel())
      .pipe(concat('main.js'))
      .pipe(gulp.dest(pathDist.js))
      .pipe(
        rename({
          suffix: '.min'
        })
      )
      .pipe(uglify())
      .pipe(gulp.dest(pathDist.js))
      .pipe(browsersync.stream())
  );
}

function images() {
  return gulp
    .src(pathSrc.img + '**/*')
    .pipe(newer(pathDist.img))
    .pipe(plumber())
    .pipe(imagemin([
      pngquant({
        quality: [.64, .72]
      }),
      mozjpeg({
        quality:85,
        progressive: true
      }),
      imagemin.svgo(),
      imagemin.optipng(),
      imagemin.gifsicle()
    ]))
    .pipe(gulp.dest(pathDist.img));
}

function watchFiles() {
  gulp.watch(pathSrc.sass + '**/*', css);
  gulp.watch(pathSrc.js + '**/*', scripts);
  gulp.watch(pathSrc.img + '**/*', images);
  gulp.watch(watchAlso, browserSyncReload);
}

const build = gulp.parallel(css, images, scripts);
const watch = gulp.parallel(watchFiles, browserSync);

exports.css     = css;
exports.scripts = scripts;
exports.images  = images;
exports.build   = build;
exports.watch   = watch;
exports.default = watch;
