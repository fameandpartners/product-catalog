/// <binding ProjectOpened='Watch - Development' />
"use strict";

const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: { 'main': "./wwwroot/src/index.js" },
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "[name].js"
    },
    resolve: {
        extensions: ['.js', '.jsx']
    },
    devServer: {
        contentBase: ".",
        host: "localhost",
        port: 9099
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            'window.jQuery': "jquery"
        }),
        new MiniCssExtractPlugin({
            filename: "[name].css",
            chunkFilename: "[id].css"
        })
    ],
    module: {
        rules: [ 
            { 
                test: /\.js?$/, 
                use: {
                     loader: "babel-loader"
                }
            },
            {
                test: /\.(scss|css)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader", // Allow for the import of CSS into the JS Bundle so it can be processed.
                    "sass-loader",
                    "postcss-loader" // See postcss.config.js for 
                ]
            }
        ]
    }
};
