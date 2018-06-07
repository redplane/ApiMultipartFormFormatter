module.exports = (env, argv) => {
    const path = require('path');
    const moduleRuleOption = require('./webpack/rule.option');
    const modulePluginOption = require('./webpack/plugin.option');

    // Import webpack settings.
    const webpackOption = require('./webpack/option');

    // True if built is set to production mode.
    // False if built is set to development mode.
    const bProductionMode = 'production' === argv.mode.toLowerCase();

    // Build path options.
    let paths = {
        root: __dirname,
        source: webpackOption.paths.getSource(__dirname),
        app: webpackOption.paths.getApplication(__dirname),
        dist: webpackOption.paths.getDist(__dirname)
    };

    /*
    * Module export.
    * */
    return {
        context: webpackOption.paths.getSource(__dirname),
        entry: {
            'jQueryVendors': ['jquery', 'bluebird', 'bootstrap', 'admin-lte', 'moment', 'rxjs'],
            'angularVendors': [
                'angular', '@uirouter/angularjs', 'angular-block-ui', 'angular-toastr',
                'angular-translate', 'angular-translate-loader-static-files', 'angular-moment', 'angular-moment-picker'],
            'app': path.resolve(paths.app, 'app.js')
        },
        optimization: {
            runtimeChunk: 'single',
            splitChunks: {
                chunks: 'all',
                cacheGroups: {
                    default: {
                        enforce: true,
                        priority: 1
                    },
                    vendors: {
                        test: /[\\/]node_modules[\\/]/,
                        priority: 2,
                        name: 'vendors',
                        enforce: true,
                        chunks: 'async'
                    }
                }
            }
        },
        module: {
            rules: moduleRuleOption.get()
        },
        plugins: modulePluginOption.get(bProductionMode, paths),
        output: {
            path: path.resolve(paths.dist),
            filename: '[name].[hash].js'
        },
        resolve: {
            // Add `.ts` and `.tsx` as a resolvable extension.
            extensions: [".ts", ".tsx", ".js"]
        },

    };
};