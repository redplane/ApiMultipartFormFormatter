// Import library.
exports = module.exports = {
    /*
    * Get configuration options.
    * */
    get: function () {

        let rules = [];

        //#region Ts loader

        // all files with a `.ts` or `.tsx` extension will be handled by `ts-loader`
        rules.push({
            test: /\.tsx?$/,
            loader: "babel-loader!ts-loader"
        });

        //#endregion

        //#region Babel loader

        rules.push({
            test: /\.js$/,
            exclude: /(node_modules)/,
            use: {
                loader: 'babel-loader',
                options: {
                    presets: [['env', {"modules": false}]]
                }
            }
        });

        //#endregion

        //#region Import jquery

        // Resolve JQuery.
        rules.push({
            test: require.resolve('jquery'),
            use: [{
                loader: 'expose-loader',
                options: 'jQuery'
            }, {
                loader: 'expose-loader',
                options: '$'
            }]
        });

        //#endregion

        //#region Sass loader

        rules.push({
            test: /\.scss$/,
            use: [{
                loader: "style-loader" // creates style nodes from JS strings
            }, {
                loader: "css-loader" // translates CSS into CommonJS
            }, {
                loader: "sass-loader" // compiles Sass to CSS
            }]
        });

        //#endregion

        //#region Css

        rules.push({
            test: /\.css$/,
            use: ['style-loader', 'css-loader']
        });

        //#endregion

        //#region Assets

        rules.push({
            test: /\.(png|jpg|gif|woff|woff2|eot|ttf|svg)$/,
            use: [
                {
                    loader: 'url-loader',
                    options: {
                        limit: 8192
                    }
                }
            ]
        });

        //#endregion

        //#region Html

        rules.push({
            test: /\.html$/, // Only .html files
            loader: 'html-loader' // Run html loader
        });

        //#endregion

        return rules;
    }
};