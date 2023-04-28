const path = require('path');
var webpack = require('webpack');
var fs = require('fs');
const { VueLoaderPlugin } = require('vue-loader')

var appBasePath = './vue/';
var distPath = './dist/';

var jsEntries = {};

fs.readdirSync(appBasePath).forEach(function (name) {
    var indexFile = appBasePath + name + "/index.js";

    console.log(indexFile);
  if (fs.existsSync(indexFile)) {
    jsEntries[name] = indexFile;
  }
});

module.exports = {
  entry: jsEntries,
  output: {
    path: path.resolve(__dirname, distPath),
    filename: '[name].js'
  },
  resolve: {
    extensions: ['.js', '.ts', '.vue', '.json'],
    alias: {
        '$vue$': 'vue/dist/vue.esm-bundler.js',
        '@': path.join(__dirname, appBasePath)
    }
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: 'vue-loader'
      }
    ]
    },
    plugins: [
        new VueLoaderPlugin()
    ],
    devServer: {
        inline: true,
        hot: true,
        stats: 'minimal',
        contentBase: __dirname,
        overlay: true
    }
}